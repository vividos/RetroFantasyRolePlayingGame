using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Game.Core.Views
{
    /// <summary>
    /// View to show the message scroll. The view displays text lines from MessageScrollLines. The
    /// text lines can also be set using a Binding object, e.g. from a view model. The lines may
    /// contain the ~ character which modifies the current text color, e.g. "~1" to set color 1.
    /// The color is reset to the TextColor (default white) after each line.
    /// </summary>
    internal class MessageScrollView : Control
    {
        /// <summary>
        /// Color modifier character
        /// </summary>
        private const char ColorModifierChar = '~';

        /// <summary>
        /// Enumerable of all message scroll lines to display; this property may also be set using
        /// a Binding object.
        /// </summary>
        public IEnumerable<string> MessageScrollLines { get; set; }

        /// <summary>
        /// Creates a new message scroll view
        /// </summary>
        public MessageScrollView()
        {
            this.BackgroundColor = Color.Black;
            this.TextColor = Color.White;
            this.BorderThickness = 1;
        }

        /// <summary>
        /// Returns the control's children; always empty
        /// </summary>
        public override IEnumerable<Control> Children => Enumerable.Empty<Control>();

        /// <summary>
        /// Returns content size of the view
        /// </summary>
        /// <param name="context">gui context</param>
        /// <returns>content size</returns>
        public override Size GetContentSize(IGuiContext context)
        {
            return new Size(this.Width, this.Height);
        }

        /// <summary>
        /// Draws the message scroll view
        /// </summary>
        /// <param name="context">gui context</param>
        /// <param name="renderer">gui renderer</param>
        /// <param name="deltaSeconds">delta of seconds since last draw</param>
        public override void Draw(IGuiContext context, IGuiRenderer renderer, float deltaSeconds)
        {
            base.Draw(context, renderer, deltaSeconds);

            renderer.FillRectangle(this.ContentRectangle, BackgroundColor);

            IEnumerable<string> scrollLines = this.GetScrollLines();
            this.DrawTextLines(context, renderer, scrollLines);
        }

        /// <summary>
        /// Retrieves the message scroll lines to display. Checks the Bindings list if there's a
        /// binding for MessageScrollLines and accesses the content of the view model instead.
        /// </summary>
        /// <returns>enumerable of message scroll lines</returns>
        private IEnumerable<string> GetScrollLines()
        {
            var scrollLinesBindings = this.Bindings.Find(binding => binding.ViewProperty == nameof(this.MessageScrollLines));
            if (scrollLinesBindings == null)
            {
                return this.MessageScrollLines;
            }

            Debug.Assert(
                scrollLinesBindings.ViewModel.GetType()
                .GetProperty(scrollLinesBindings.ViewModelProperty).PropertyType == typeof(IEnumerable<string>),
                "bound property must be of type IEnumerable<string>");

            return
                scrollLinesBindings.ViewModel
                .GetType()
                .GetProperty(scrollLinesBindings.ViewModelProperty)
                .GetValue(scrollLinesBindings.ViewModel) as IEnumerable<string>;
        }

        /// <summary>
        /// Draws all text lines
        /// </summary>
        /// <param name="context">gui context</param>
        /// <param name="renderer">gui renderer</param>
        /// <param name="scrollLines">all scroll lines to draw</param>
        private void DrawTextLines(IGuiContext context, IGuiRenderer renderer, IEnumerable<string> scrollLines)
        {
            int lineIndex = 0;
            foreach (string line in scrollLines)
            {
                var currentColor = this.TextColor;

                if (!line.Contains(ColorModifierChar))
                {
                    renderer.DrawText(
                        context.DefaultFont,
                        line,
                        new Vector2(this.ContentRectangle.X, this.ContentRectangle.Y + lineIndex * 16.0f),
                        currentColor);
                }
                else
                {
                    this.DrawColoredText(context, renderer, line, lineIndex);
                }

                lineIndex++;
            }
        }

        /// <summary>
        /// Draws colored text line
        /// </summary>
        /// <param name="context"></param>
        /// <param name="renderer"></param>
        /// <param name="line"></param>
        /// <param name="lineIndex"></param>
        private void DrawColoredText(IGuiContext context, IGuiRenderer renderer, string line, int lineIndex)
        {
            // TODO implement
        }
    }
}
