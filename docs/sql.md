---
layout: default
title: "Sql"
---

# SQL Syntax Highlighting

The `AnsiConsoleSqlExtensions` class provides extension methods for `IAnsiConsole` to render SQL syntax-highlighted output.

## Methods

### WriteSqlAsync (Stream, default styles)

Writes SQL content from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteSqlAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing SQL content.

**Returns:** A task representing the asynchronous operation.

### WriteSqlAsync (Stream, custom styles)

Writes SQL content from a stream to the console with custom styling asynchronously.

```csharp
Task<string> WriteSqlAsync(this IAnsiConsole ansiConsole, Stream stream, SqlStyles sqlStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing SQL content.
- `sqlStyles`: The SQL styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation.

### WriteSql (Stream, default styles)

Writes SQL content from a stream to the console with default styling synchronously.

```csharp
string WriteSql(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing SQL content.

**Returns:** The processed SQL content as a string.

### WriteSql (Stream, custom styles)

Writes SQL content from a stream to the console with custom styling synchronously.

```csharp
string WriteSql(this IAnsiConsole ansiConsole, Stream stream, SqlStyles sqlStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing SQL content.
- `sqlStyles`: The SQL styles to use for syntax highlighting.

**Returns:** The processed SQL content as a string.

### WriteSql (string, default styles)

Writes SQL content from a string to the console with default styling.

```csharp
void WriteSql(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `value`: The SQL content as a string.

### WriteSql (string, custom styles)

Writes SQL content from a string to the console with custom styling.

```csharp
void WriteSql(this IAnsiConsole ansiConsole, string value, SqlStyles sqlStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `value`: The SQL content as a string.
- `sqlStyles`: The SQL styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var sqlQuery = """
    -- Create a table with various data types and constraints
    CREATE TABLE Employees (
        EmployeeID INT PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Position NVARCHAR(50),
        Department NVARCHAR(50),
        Salary DECIMAL(10, 2),
        HireDate DATE DEFAULT GETDATE(),
        Active BIT DEFAULT 1
    );

    -- Insert sample data into the Employees table
    INSERT INTO Employees (EmployeeID, FirstName, LastName, Position, Department, Salary)
    VALUES 
    (1, 'Alice', 'Smith', 'Software Engineer', 'IT', 75000.00),
    (2, 'Bob', 'Johnson', 'Data Scientist', 'Analytics', 82000.50);

    -- Select data with a complex query
    SELECT 
        E.Department,
        COUNT(E.EmployeeID) AS TotalEmployees,
        AVG(E.Salary) AS AverageSalary
    FROM 
        Employees E
    WHERE 
        E.Salary > 70000
    GROUP BY 
        E.Department
    HAVING 
        AVG(E.Salary) > 80000
    ORDER BY 
        AverageSalary DESC;
    """;

AnsiConsole.Console.WriteSql(sqlQuery);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customSqlStyles = SqlStyles.Default;
customSqlStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteSql(sqlQuery, customSqlStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteSqlAsync(stream);
```
