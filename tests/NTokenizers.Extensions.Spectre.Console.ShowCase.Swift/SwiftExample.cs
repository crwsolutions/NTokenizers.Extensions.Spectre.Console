namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Swift;

internal static class SwiftExample
{
    internal static string GetSampleSwift() =>
        """
        // Hello World in Swift
        // Demonstrates various token types
        import Foundation

        let maxSize: Int = 100
        let isActive: Bool = true

        @available(iOS 13.0, *)
        struct Config {
            let name: String
            let enabled: Bool
            let count: Int
            let debug: Bool?
        }

        @main
        struct App {
            static func main() {
                let greeting = "Hello, World!"
                let initial: Character = "H"
                let pi: Double = 3.14159
                let items = ["one", "two", "three"]
                var result: String? = nil

                // Iterate with index
                for (index, item) in items.enumerated() {
                    print("Item \\(index): \\(item)")
                }

                // Dictionary
                var config: [String: Any] = [
                    "name": greeting,
                    "enabled": isActive,
                    "count": items.count
                ]

                // Optional binding
                if let first = items.first {
                    result = first
                    print("First: \\(result)")
                }

                // Ternary operator
                let status = items.isEmpty ? "Empty" : "Has items"
                print("Status: \\(status)")

                // Guard statement
                guard let count = config["count"] as? Int else {
                    return
                }
            }
        }
        """;
}
