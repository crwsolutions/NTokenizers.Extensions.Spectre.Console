// tests/NTokenizers.Extensions.Spectre.Console.ShowCase.Css/CssExample.cs
namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Css;

internal static class CssExample
{
    internal static string GetSampleCss() =>
        """
        /* CSS Showcase Example */
        @charset "UTF-8";

        /* CSS Variables */
        :root {
          --primary-color: #3498db;
          --secondary-color: #2ecc71;
          --accent-color: #e74c3c;
          --font-family: 'Arial', sans-serif;
          --border-radius: 4px;
          --box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        /* Reset & Base Styles */
        * {
          margin: 0;
          padding: 0;
          box-sizing: border-box;
        }

        body {
          font-family: var(--font-family);
          line-height: 1.6;
          color: #333;
          background-color: #f8f9fa;
        }

        /* Typography */
        h1, h2, h3, h4, h5, h6 {
          margin-bottom: 0.5em;
          font-weight: 700;
          line-height: 1.2;
        }

        p {
          margin-bottom: 1em;
        }

        /* Layout */
        .container {
          width: 100%;
          max-width: 1200px;
          margin: 0 auto;
          padding: 0 20px;
        }

        .grid {
          display: grid;
          grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
          gap: 2rem;
        }

        /* Components */
        .card {
          background: white;
          border-radius: var(--border-radius);
          box-shadow: var(--box-shadow);
          padding: 1.5rem;
          transition: transform 0.3s ease, box-shadow 0.3s ease;
        }

        .card:hover {
          transform: translateY(-5px);
          box-shadow: 0 8px 16px rgba(0,0,0,0.15);
        }

        /* Buttons */
        .btn {
          display: inline-block;
          padding: 0.75rem 1.5rem;
          border: none;
          border-radius: var(--border-radius);
          font-size: 1rem;
          font-weight: 600;
          text-decoration: none;
          cursor: pointer;
          transition: all 0.3s ease;
        }

        .btn-primary {
          background-color: var(--primary-color);
          color: white;
        }

        .btn-secondary {
          background-color: var(--secondary-color);
          color: white;
        }

        /* Animations */
        @keyframes fadeIn {
          from { opacity: 0; }
          to { opacity: 1; }
        }

        .fade-in {
          animation: fadeIn 0.5s ease-in;
        }

        /* Responsive */
        @media (max-width: 768px) {
          .grid {
            grid-template-columns: 1fr;
            gap: 1rem;
          }
          
          .container {
            padding: 0 15px;
          }
        }

        /* Pseudo-selectors */
        a:hover {
          color: var(--primary-color);
        }

        input:focus {
          outline: 2px solid var(--primary-color);
          outline-offset: 2px;
        }

        /* Attribute Selectors */
        [data-theme="dark"] {
          --bg-color: #1a1a1a;
          --text-color: #f0f0f0;
        }

        /* Complex Selector */
        .card > h3 + p {
          margin-top: 0.5rem;
          font-size: 0.9rem;
          color: #666;
        }

        /* CSS Grid Layout */
        .dashboard-grid {
          display: grid;
          grid-template-areas: 
            "header header header"
            "sidebar main aside"
            "footer footer footer";
          grid-template-columns: 200px 1fr 250px;
          grid-template-rows: auto 1fr auto;
          height: 100vh;
        }

        .header { grid-area: header; }
        .sidebar { grid-area: sidebar; }
        .main { grid-area: main; }
        .aside { grid-area: aside; }
        .footer { grid-area: footer; }

        /* Flexbox Example */
        .flex-container {
          display: flex;
          flex-wrap: wrap;
          justify-content: space-between;
          align-items: center;
        }

        .flex-item {
          flex: 1 1 300px;
          margin: 10px;
        }

        /* CSS Custom Properties with Fallback */
        .theme-text {
          color: var(--text-color, #333);
          background-color: var(--bg-color, #fff);
        }

        /* Media Queries with Complex Conditions */
        @media screen and (min-width: 1024px) and (max-width: 1200px) and (orientation: landscape) {
          .container {
            padding: 0 30px;
          }
        }
        """;
}