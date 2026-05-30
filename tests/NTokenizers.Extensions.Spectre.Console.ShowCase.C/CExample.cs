namespace NTokenizers.Extensions.Spectre.Console.ShowCase.C;

internal static class CExample
{
    internal static string GetSampleC() =>
        """
        #include <stdio.h>
        #include <stdlib.h>
        #include <string.h>

        // Hello World in C
        // Demonstrates various token types
        #define MAX_SIZE 100
        #define PI 3.14159

        typedef struct {
            char name[50];
            int age;
            double score;
            int *next;
        } Person;

        int main() {
            const int max_count = 42;
            double average = 0.0;
            char grade = 'A';
            int *ptr = NULL;

            Person persons[MAX_SIZE];
            int count = 0;

            // Initialize array
            for (int i = 0; i < max_count; i++) {
                persons[i].age = i + 1;
                persons[i].score = (double)i * PI;
                persons[i].next = NULL;
                strcpy(persons[i].name, "Unknown");
            }

            // Calculate average with pointer arithmetic
            for (int i = 0; i < max_count; i++) {
                average += persons[i].score;
            }
            average /= max_count;

            // Ternary operator example
            int result = (average > 0) ? (int)average : 0;

            // Arrow operator with struct pointer
            Person *current = &persons[0];
            printf("Name: %s, Age: %d\n", current->name, current->age);

            // Switch statement with colon
            switch (result) {
                case 0:
                    printf("No data\n");
                    break;
                default:
                    printf("Result: %d\n", result);
                    break;
            }

            return 0;
        }
        """;
}
