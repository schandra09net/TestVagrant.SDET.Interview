using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestVagrant.SDET.Interview.Models;

namespace TestVagrant.SDET.Interview
{
    /// <summary>
    /// Provides the methods to get the weather details using OpenWeather REST API
    /// </summary>
    public class OpenWeather
    {
        #region Global declarations

        private static readonly HttpClient client = new HttpClient();

        #endregion Global declarations

        #region Public methods

        /// <summary>
        /// Provides the weather details for a city
        /// </summary>
        /// <param name="city">Provide the city name to get the details</param>
        /// <returns>Weather details for the provided city</returns>
        public static async Task<OpenWeatherResponse> GetWeatherDetailsForaCity(string city)
        {
            // Get the OpenWeather deatils from configuration file
            var appId = ConfigurationManager.AppSettings["OpenWeatherApiKey"];
            var appUrl = ConfigurationManager.AppSettings["OpenWeatherEndPoint"];
            var fullEndPoint = new StringBuilder();
            fullEndPoint.Append("https://api.openweathermap.org/data/2.5/weather?q=");
            fullEndPoint.Append(city);
            fullEndPoint.Append("&appid=" + "7fe67bf08c80ded756e598d6f8fedaea");

            // Invoke the API and get the result
            OpenWeatherResponse weatherResponse = null;
            var response = await client.GetAsync(fullEndPoint.ToString());
            if (response.IsSuccessStatusCode)
            {
                // Convert the API result to proper response Weather model
                var result = response.Content.ReadAsStringAsync().Result;
                weatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(result);
            }
            return weatherResponse;
        }

        /// <summary>
        /// Check the value is in a range
        /// </summary>
        /// <param name="original">Value to be verified</param>
        /// <param name="preOffSet">Minimum offset</param>
        /// <param name="postOffset">Maximum offset</param>
        /// <returns>True if the value is in range, else False</returns>
        public static bool VerifyResultsRange(int original, int preOffSet, int postOffset)
        {
            return (preOffSet <= original & original < postOffset) |
                (postOffset <= original & original < preOffSet);
        }

        #endregion Public methods
    }
}