using System;
using System.Collections.Generic;
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
        public IEnumerable<string> VisibleLines
        {
            get
            {
                return this.scrollLines.GetRange(
                    this.currentScrollIndex,
                    Math.Min(scrollLines.Count - this.currentScrollIndex, this.numVisibleLines));
            }
        }

        /// <summary>
        /// Indicates if scroll is at start of all scroll lines
        /// </summary>
        public bool IsAtScrollStart => this.currentScrollIndex == 0;

        /// <summary>
        /// Indicates if scroll is at end of all scroll lines
        /// </summary>
        public bool IsAtScrollEnd
        {
            get
            {
                return scrollLines.Count - this.currentScrollIndex <= this.numVisibleLines;
            }
        }

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

            while (remainingText.Any())
            {
                if (remainingText.Length < numLineChars)
                {
                    this.scrollLines.Add(remainingText);
                    break;
                }

                // find next break from the end of the maximum line length
                int nextBreakPos = remainingText.LastIndexOf(' ', numLineChars);
                if (nextBreakPos == -1)
                {
                    // there's no remaining break; just split the word at the end of the line
                    nextBreakPos = numLineChars;
                }

                this.scrollLines.Add(remainingText.Substring(0, nextBreakPos));
                remainingText = remainingText.Substring(nextBreakPos).TrimStart();
            }

            this.ScrollToEnd();
        }
    }
}
