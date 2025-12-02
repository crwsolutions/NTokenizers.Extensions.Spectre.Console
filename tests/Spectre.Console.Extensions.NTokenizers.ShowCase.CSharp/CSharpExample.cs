namespace Spectre.Console.Extensions.NTokenizers.ShowCase.CSharp;

internal static class CSharpExample
{
    internal static string GetSampleCSharp() =>
        """
        using System;

        // Main method
        public readonly record struct class Program {
            public static void Main() {
                Console.WriteLine("Hello, World!");
            }
        }
        """;
}
