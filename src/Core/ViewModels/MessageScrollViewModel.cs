using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Game.Core.ViewModels
{
    /// <summary>
    /// View model for the message scroll. The message scroll contains a number of scroll lines,
    /// of which a number of lines are currently visible. The lines can be scrolled up and down.
    /// When adding text to the message scroll, the text is word-wrapped when the text is too
    /// long. The text may contain the special "~X" control character sequence that switches the
    /// current text color to color X (with X between 0 and 9).
    /// </summary>
    internal class MessageScrollViewModel
    {
        /// <summary>
        /// Color modifier character
        /// </summary>
        private const char ColorModifierChar = '~';

        /// <summary>
        /// All scroll lines
        /// </summary>
        private readonly List<string> scrollLines = new List<string>();

        /// <summary>
        /// Number of visible lines in the view
        /// </summary>
        private readonly int numVisibleLines;

        /// <summary>
        /// Number of characters in one line
        /// </summary>
        private readonly int numLineChars;

        /// <summary>
        /// Current scroll line index (start of lines to display)
        /// </summary>
        private int currentScrollIndex = 0;

        /// <summary>
        /// Returns all currently visible lines
        /// </summary>
        public IEnumerable<string> VisibleLines =>
            this.scrollLines.GetRange(
                this.currentScrollIndex,
                Math.Min(this.scrollLines.Count - this.currentScrollIndex, this.numVisibleLines));

        /// <summary>
        /// Indicates if scroll is at start of all scroll lines
        /// </summary>
        public bool IsAtScrollStart => this.currentScrollIndex == 0;

        /// <summary>
        /// Indicates if scroll is at end of all scroll lines
        /// </summary>
        public bool IsAtScrollEnd => this.scrollLines.Count - this.currentScrollIndex <= this.numVisibleLines;

        /// <summary>
        /// Creates a new view model for the message scroll view
        /// </summary>
        /// <param name="numVisibleLines">number of visible lines</param>
        /// <param name="numLineChars">max. number of character in one line</param>
        public MessageScrollViewModel(int numVisibleLines, int numLineChars)
        {
            this.numVisibleLines = numVisibleLines;
            this.numLineChars = numLineChars;
        }

        /// <summary>
        /// Scrolls up one line
        /// </summary>
        public void ScrollUp()
        {
            if (this.currentScrollIndex > 0)
            {
                this.currentScrollIndex--;
            }
        }

        /// <summary>
        /// Scrolls down one line
        /// </summary>
        public void ScrollDown()
        {
            if (this.scrollLines.Count - this.currentScrollIndex < this.numVisibleLines)
            {
                this.currentScrollIndex++;
            }
        }

        /// <summary>
        /// Scrolls to the end of the scroll lines
        /// </summary>
        public void ScrollToEnd()
        {
            if (this.scrollLines.Count < this.numVisibleLines)
            {
                // not enough lines
                this.currentScrollIndex = 0;
                return;
            }

            this.currentScrollIndex = this.scrollLines.Count - this.numVisibleLines;
        }

        /// <summary>
        /// Adds text to scroll view, possibly using word-break to split long lines. The text may
        /// contain special "~X" characters that specify that color X should be used (X between 0
        /// and 9).
        /// </summary>
        /// <param name="text">text to add to scroll</param>
        public void AddText(string text)
        {
            string remainingText = text;
            string currentColorText = string.Empty;

            while (remainingText.Any())
            {
                int remainingTextLength = remainingText.Length - 2 * text.Count(ch => ch == ColorModifierChar);
                if (remainingTextLength < this.numLineChars)
                {
                    this.scrollLines.Add(remainingText);
                    break;
                }

                // find next break from the end of the maximum line length
                int nextBreakPos = this.FindNextBreakPos(remainingText, ref currentColorText);
                if (nextBreakPos == -1)
                {
                    // there's no remaining break; just split the word at the end of the line
                    nextBreakPos = this.numLineChars;
                }

                this.scrollLines.Add(remainingText.Substring(0, nextBreakPos));
                remainingText = currentColorText + remainingText.Substring(nextBreakPos).TrimStart();
            }

            this.ScrollToEnd();
        }

        /// <summary>
        /// Finds the next break position, by checking the text for space characters and returning
        /// the last one. Color modifier chars are skipped in the search.
        /// </summary>
        /// <param name="text">text to check for the next break position</param>
        /// <param name="currentColorText">updated current color text</param>
        /// <returns>next break position, or -1 when none was found</returns>
        private int FindNextBreakPos(string text, ref string currentColorText)
        {
            if (!text.Contains(ColorModifierChar))
            {
                return text.LastIndexOf(' ', this.numLineChars);
            }

            int breakPos = -1;
            int offset = 0;
            for (int index = 0; index < text.Length && index <= this.numLineChars + offset; index++)
            {
                char ch = text[index];
                if (ch == ' ')
                {
                    breakPos = index;
                }
                else if (ch == ColorModifierChar)
                {
                    Debug.Assert(
                        index + 1 < text.Length,
                        "after the ~ the color character must follow");

                    if (index + 1 < text.Length)
                    {
                        currentColorText = text.Substring(index, 2);

                        if (currentColorText.ToLowerInvariant() == "~f")
                        {
                            currentColorText = string.Empty;
                        }

                        // from now on, the line can be 2 chars longer than before
                        offset += 2;
                    }
                }
            }

            return breakPos;
        }
    }
}
