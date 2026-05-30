namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Go;

internal static class GoExample
{
    internal static string GetSampleGo() =>
        """
        package main

        import (
        	"fmt"
        	"strings"
        )

        // Hello World in Go
        // Demonstrates various token types
        const maxRetries = 3

        type Config struct {
        	Name    string
        	Enabled bool
        	Count   int
        	Debug   *bool
        }

        func main() {
        	enabled := true
        	debug := false
        	name := "app"
        	ch := 'A'

        	config := Config{
        		Name:    name,
        		Enabled: enabled,
        		Count:   42,
        		Debug:   nil,
        	}

        	items := []string{"one", "two", "three"}
        	for i, item := range items {
        		fmt.Printf("Item %d: %s\n", i, item)
        	}

        	result := strings.Join(items, ", ")
        	if result != "" {
        		fmt.Println("Success:", result)
        	} else {
        		fmt.Println("Empty result")
        	}
        }
        """;
}
