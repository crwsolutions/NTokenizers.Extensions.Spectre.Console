namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Java;

internal static class JavaExample
{
    internal static string GetSampleJava() =>
        """
        // Hello World in Java
        // Demonstrates various token types
        import java.util.List;
        import java.util.ArrayList;

        public class Main {
            private static final int MAX_SIZE = 100;
            private static boolean isActive = true;

            public static void main(String[] args) {
                String greeting = "Hello, World!";
                char initial = 'H';
                double pi = 3.14159;
                Integer count = null;

                List<String> items = new ArrayList<>();
                items.add("one");
                items.add("two");
                items.add("three");

                for (int i = 0; i < items.size(); i++) {
                    System.out.println("Item " + i + ": " + items.get(i));
                }

                // Ternary operator example
                String result = (items.size() > 0) ? "Has items" : "Empty";
                System.out.println("Status: " + result);

                // Method reference with double colon
                items.forEach(System.out::println);
            }
        }
        """;
}
