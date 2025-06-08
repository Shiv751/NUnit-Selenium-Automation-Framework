using Newtonsoft.Json.Linq;
using System.Configuration;

namespace NUnitAutomationFramework.Utility
{
    /// <summary>
    /// The `Parser` class provides methods to retrieve configuration properties from a JSON file.
    /// </summary>
    public class Parser
    {
        public static string env = ConfigHelper.GetAppSetting("Environment");

        /// <summary>
        /// The GetProperty method retrieves the value of a specified property from a JSON file based on the current environment.
        /// </summary>
        /// <param name="data">The key of the property to retrieve.</param>
        /// <returns>Returns the value of the specified property.</returns>
        public static string GetProperty(string data)
        {
            try
            {
                string currentdirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
                                          ?? throw new Exception("Failed to determine the current directory.");
                string jsonstring = File.ReadAllText(Path.Combine(currentdirectory, "Properties.json"));
                var json = JToken.Parse(jsonstring);
                var token = (json?.SelectToken($"{env}.{data}")) ?? throw new Exception($"Property '{data}' not found for environment '{env}'.");
                return token.ToString();
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred in the GetProperty method, please check the parameters name", e);
            }
        }
    }

    /// <summary>
    /// The `TestData` class provides methods to retrieve test data from a JSON file.
    /// </summary>
    public static class TestData
    {
        private static string env = ConfigHelper.GetAppSetting("Environment");
        private static string currentdirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
                                          ?? throw new Exception("Failed to determine the current directory.");
        private static string jsonstring = File.ReadAllText(Path.Combine(currentdirectory, "Constants", "Testdata.json"));

        /// <summary>
        /// The List method retrieves a list of users from a JSON file based on the current environment.
        /// </summary>
        /// <param name="List">The key of the user list to retrieve.</param>
        /// <returns>Returns a comma-separated string of usernames.</returns>
        public static string List(string List)
        {
            try
            {
                var json = JToken.Parse(jsonstring);
                var token = (json?.SelectToken($"{env}.{List}")) ?? throw new Exception($"User list '{List}' not found for environment '{env}'.");
                List<string> usernames = token.ToObject<List<string>>() ?? new List<string>();
                return string.Join(", ", usernames);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in List method: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// The GetTestData method retrieves test data from a JSON file based on the current environment and the specified key.
        /// </summary>
        /// <param name="key">The key of the test data to retrieve.</param>
        /// <returns>Returns the value of the specified test data.</returns>
        public static string GetTestData(string key)
        {
            try
            {
                var json = JToken.Parse(jsonstring);
                var token = (json?.SelectToken($"{env}.{key}")) ?? throw new Exception($"Test data '{key}' not found for environment '{env}'.");
                return token.Type == JTokenType.Array ? string.Join(", ", token.ToObject<string[]>()) : token.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetTestData method: {ex.Message}", ex);
            }
        }
    }
}
