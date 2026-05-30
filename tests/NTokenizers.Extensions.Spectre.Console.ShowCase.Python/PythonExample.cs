namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Python;

internal static class PythonExample
{
    internal static string GetSamplePython() =>
        """
        # Hello World in Python
        # Demonstrates various token types
        import os
        from typing import List, Optional

        MAX_SIZE = 100
        is_active = True

        @dataclass
        class Config:
            name: str
            enabled: bool
            count: int
            debug: Optional[bool] = None

        def main():
            greeting = "Hello, World!"
            initial = 'H'
            pi = 3.14159
            items = ["one", "two", "three"]
            result = None

            # Iterate with index
            for index, item in enumerate(items):
                print(f"Item {index}: {item}")

            # List comprehension
            squared = [x ** 2 for x in range(10) if x > 0]

            # Dictionary
            config = {
                "name": greeting,
                "enabled": is_active,
                "count": len(items)
            }

            # Ternary operator
            status = "Has items" if items else "Empty"
            print(f"Status: {status}")

            # Walrus operator (:=)
            if (count := len(items)) > 0:
                print(f"Found {count} items")
        """;
}
