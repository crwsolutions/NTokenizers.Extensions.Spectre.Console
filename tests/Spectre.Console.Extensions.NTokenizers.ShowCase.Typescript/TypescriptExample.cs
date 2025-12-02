namespace Spectre.Console.Extensions.NTokenizers.ShowCase.Typescript;

internal static class TypescriptExample
{
    internal static string GetSampleTypescript() =>
        """
        // Single-line comment: Declare variables
        const message = "Hello, \"world\"!"; // Escaped double quotes
        let age = 25; // Dynamic variable
        var isActive = true; // Boolean value

        /*
        Multi-line comment:
        This block showcases variable declaration,
        string manipulation, and escaping characters.
        */
        const multiLineString = `This is a multiline
        string with embedded \`special characters\`.`;

        function greet(name: string) {
            // String with single quotes and escaping
            const greeting = 'Hi, \'${name}\'!';
            return greeting;
        }

        // Control structure: if-else statement
        if (isActive && age > 18) {
            console.log(message);
            console.log(greet("Alice"));
        } else {
            console.warn('Inactive or age is below threshold.');
        }

        // Loop structure: for loop
        for (let i = 0; i < 5; i++) {
            console.log(`Count: ${i}`);
        }

        // Array and map
        const numbers: number[] = [1, 2, 3, 4, 5];
        const squared = numbers.map(num => num ** 2);

        // Output using template literals
        console.log(`Squared values: ${squared.join(", ")}`);

        // Exception handling: try-catch-finally
        try {
            throw new Error("An unexpected error occurred!");
        } catch (error: any) {
            console.error(error.message);
        } finally {
            console.info("Execution completed.");
        }
        """;
}
