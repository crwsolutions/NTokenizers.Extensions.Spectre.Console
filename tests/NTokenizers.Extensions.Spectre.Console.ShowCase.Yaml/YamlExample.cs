namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Yaml;

internal static class YamlExample
{
    internal static string GetSampleYaml() =>
        """
        %YAML 1.2
        %TAG !yaml! tag:yaml.org,2002:

        --- # Document Start (optional for the first one)
        name: Alice
        age: 30
        --- # Second Document Start
        name: Bob
        age: 25
        ... # Document End (optional for the last one)

        # Block Mapping
        person: &p
          name: "Alice"
          age: 30
          hobbies:
            - reading
            - coding
            - chess

        # Block Sequence
        fruits:
          - apple
          - banana
          - cherry

        # Flow Sequence
        numbers: [1, 2, 3, 4]

        # Flow Mapping
        employee: { name: "Bob", position: "Developer" }

        # Key / Value explicit
        ? question_key
        : answer_value

        # Anchors & Aliases
        manager: *p

        # Tag examples
        !customTag "some value"
        !!str "string as type"
        """;
}
