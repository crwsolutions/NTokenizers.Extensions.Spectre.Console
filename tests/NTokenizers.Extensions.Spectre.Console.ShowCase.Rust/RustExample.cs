namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Rust;

internal static class RustExample
{
    internal static string GetSampleRust() =>
        """
        // Hello World in Rust
        // Demonstrates various token types
        use std::collections::HashMap;

        const MAX_SIZE: usize = 100;
        static IS_ACTIVE: bool = true;

        #[derive(Debug, Clone)]
        struct Config {
            name: String,
            enabled: bool,
            count: i32,
            debug: Option<bool>,
        }

        fn main() {
            let greeting = "Hello, World!";
            let initial: char = 'H';
            let pi: f64 = 3.14159;
            let items = vec!["one", "two", "three"];
            let mut result: Option<String> = None;

            // Iterate with index
            for (index, item) in items.iter().enumerate() {
                println!("Item {}: {}", index, item);
            }

            // HashMap example
            let mut map = HashMap::new();
            map.insert("key", 42);
            map.insert("count", items.len());

            // Pattern matching with fat arrow
            let status = match items.len() {
                0 => "Empty",
                1..=3 => "Few items",
                _ => "Many items",
            };

            // Lifetime annotation
            let binding = &greeting[..];
            println!("Status: {}", status);
        }

        // Function with lifetime parameter
        fn longest<'a>(x: &'a str, y: &'a str) -> &'a str {
            if x.len() > y.len() { x } else { y }
        }
        """;
}
