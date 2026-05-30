namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Kotlin;

internal static class KotlinExample
{
    internal static string GetSampleKotlin() =>
        """
        // Hello World in Kotlin
        // Demonstrates various token types
        package com.example.app

        import kotlin.math.PI

        const val MAX_SIZE: Int = 100
        val isActive: Boolean = true

        @Deprecated("Use new API")
        data class Config(
            val name: String,
            val enabled: Boolean,
            val count: Int,
            val debug: Boolean? = null
        )

        fun main() {
            val greeting = "Hello, World!"
            val initial: Char = 'H'
            val items = listOf("one", "two", "three")
            var result: String? = null

            // Iterate with index
            for ((index, item) in items.withIndex()) {
                println("Item $index: $item")
            }

            // Null safety with safe call
            result = items.getOrNull(0) ?: "default"
            println("First: $result")

            // Lambda with double colon reference
            items.forEach(::println)

            // When expression
            val status = when (items.size) {
                0 -> "Empty"
                in 1..3 -> "Few items"
                else -> "Many items"
            }
        }
        """;
}
