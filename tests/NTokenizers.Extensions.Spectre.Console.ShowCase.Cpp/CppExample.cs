namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Cpp;

internal static class CppExample
{
    internal static string GetSampleCpp() =>
        """
        #include <iostream>
        #include <vector>

        // Hello World in C++
        // Demonstrates various token types
        int main() {
            const int max_size = 42;
            double pi = 3.14159;
            char grade = 'A';
            bool is_valid = true;
            int* ptr = nullptr;

            std::vector<int> numbers{1, 2, 3};
            int sum = 0;

            for (int i = 0; i < max_size; i++) {
                sum += numbers[i];
            }

            int result = (sum > 0) ? sum : -sum;
            std::cout << "Sum: " << sum << std::endl;

            return 0;
        }
        """;
}
