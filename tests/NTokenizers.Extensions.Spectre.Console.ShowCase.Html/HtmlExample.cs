namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Html;

internal static class HtmlExample
{
    internal static string GetSampleHtml() =>
        """
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body { font-family: Arial, sans-serif; }
                .container { max-width: 600px; margin: 0 auto; }
                button { padding: 10px 15px; background: #007bff; color: white; }
            </style>
        </head>
        <body>
            <div class="container">
                <h1>Hello World</h1>
                <p>This is a sample HTML page.</p>
                <button onclick="alert('Clicked!')">Click Me</button>
            </div>
            <script>
                document.addEventListener('DOMContentLoaded', () => {
                    console.log('Page loaded');
                });
            </script>
        </body>
        </html>
        """;
}
