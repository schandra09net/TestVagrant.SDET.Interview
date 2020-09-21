using NUnit.Framework;
using TechTalk.SpecFlow;

namespace TestVagrant.SDET.Interview.Steps
{
    [Binding]
    public class VerifyWeatherDataBetweenWebAndAPISteps
    {
        private static readonly NdtvPage ndtvPage = new NdtvPage();
        private int WebResult { get; set; }
        private int ApiResult { get; set; }

        [Given(@"The NDTV page is opened")]
        public void GivenTheNDTVPageIsOpened()
        {
            ndtvPage.OpenNdtv();
        }

        [Then(@"Navigate to weather section")]
        public void ThenNavigateToWeatherSection()
        {
            ndtvPage.OpenWeatherSection();
        }

        [Then(@"Search for a city (.*) and add to map")]
        public void ThenSearchForACityAndAddToMap(string cityName)
        {
            ndtvPage.SearchForCityAndPin(cityName);
        }

        [Then(@"Get the forecast details of added city (.*)")]
        public void ThenGetTheForecastDetailsOfAddedCityMumbai(string cityName)
        {
            WebResult = ndtvPage.GetForecastDetailsForCity(cityName);
        }

        [Then(@"Invoke the OpenWeather rest api for the same city (.*)")]
        public void ThenInvokeTheOpenWeatherRestApiForTheSameCityMumbai(string cityName)
        {
            var restOutput = OpenWeather.GetWeatherDetailsForaCity(cityName);
            ApiResult = restOutput.Result.main.humidity;
        }

        [Then(@"Verify the details from web and api with offset (.*)")]
        public void ThenVerifyTheDetailsFromWebAndApi(int offset)
        {
            var isInragne = OpenWeather.VerifyResultsRange(ApiResult, WebResult - offset, WebResult + offset);
            Assert.IsTrue(isInragne,
            string.Format("Humidity value from web and rest are not same. Web: {0} and Api: {1}",
            WebResult, ApiResult));
        }

        [Then(@"Close the NDTV webpage")]
        public void ThenCloseTheNDTVWebpage()
        {
            ndtvPage.CloseTheDriver();
        }
    }
}