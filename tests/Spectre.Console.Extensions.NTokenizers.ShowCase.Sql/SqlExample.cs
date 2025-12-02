namespace Spectre.Console.Extensions.NTokenizers.ShowCase.Sql;

internal static class SqlExample
{
    internal static string GetSampleSql() =>
        """
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
        (2, 'Bob', 'Johnson', 'Data Scientist', 'Analytics', 82000.50),
        (3, 'Charlie', 'Williams', 'Product Manager', 'Marketing', 91000.25);

        -- Select data with a complex query, including a JOIN, GROUP BY, and HAVING
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

        -- Update the table with a condition
        UPDATE Employees
        SET Active = 0
        WHERE HireDate < '2020-01-01' OR Position = 'Product Manager';
        """;
}
