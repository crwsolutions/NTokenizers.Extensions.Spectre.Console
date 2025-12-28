using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Ai.Prompts
{
    /// <summary>
    /// Extension methods for configuring PromptBox properties in a fluent manner.
    /// </summary>
    public static class PromptBoxExtensions
    {
        /// <summary>
        /// Sets the style for the prompt text.
        /// </summary>
        /// <param name="obj">The prompt box.</param>
        /// <param name="style">The style to use for the prompt text or <see langword="null"/> to use the default style (Color.Red).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PromptBox PromptStyle(this PromptBox obj, Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.PromptStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the style for the regular text.
        /// </summary>
        /// <param name="obj">The prompt box.</param>
        /// <param name="style">The style to use for the regular text or <see langword="null"/> to use the default style (Style.Plain).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PromptBox TextStyle(this PromptBox obj, Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.TextStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the style for the wrap glyph.
        /// </summary>
        /// <param name="obj">The prompt box.</param>
        /// <param name="style">The style to use for the wrap glyph or <see langword="null"/> to use the default style (Color.Yellow).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PromptBox WrapGlyphStyle(this PromptBox obj, Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.WrapGlyphStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the box border style for the panel.
        /// </summary>
        /// <param name="obj">The prompt box.</param>
        /// <param name="boxBorder">The box border style to use or <see langword="null"/> to use the default style (BoxBorder.Rounded).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PromptBox BoxBorder(this PromptBox obj, BoxBorder boxBorder)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.BoxBorder = boxBorder;
            return obj;
        }

        /// <summary>
        /// Sets the border style for the panel.
        /// </summary>
        /// <param name="obj">The prompt box.</param>
        /// <param name="style">The border style to use or <see langword="null"/> to use the default style (Color.Green1).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PromptBox BorderStyle(this PromptBox obj, Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.BorderStyle = style;
            return obj;
        }

                /// <summary>
        /// Sets the placeholder text to display when the input is empty.
        /// </summary>
        /// <param name="obj">The prompt box.</param>
        /// <param name="placeholder">The placeholder text to use or <see langword="null"/> to use the default placeholder.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PromptBox Placeholder(this PromptBox obj, string placeholder)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.Placeholder = placeholder;
            return obj;
        }

        /// <summary>
        /// Sets the style for the placeholder text.
        /// </summary>
        /// <param name="obj">The prompt box.</param>
        /// <param name="style">The style to use for the placeholder text or <see langword="null"/> to use the default style (#777777).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PromptBox PlaceholderStyle(this PromptBox obj, Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.PlaceholderStyle = style;
            return obj;
        }
    }
}