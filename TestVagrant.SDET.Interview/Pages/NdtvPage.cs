using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.Threading;
using System.Xml;

namespace TestVagrant.SDET.Interview
{
    /// <summary>
    /// Page Object model implementation for NDTV weather forecast
    /// </summary>
    [TestFixture]
    public class NdtvPage
    {
        #region Global declarations

        private IWebDriver ndtvWebDriver;
        private string homeURL;
        private WebDriverWait ndtvWebDriverWait = null;
        private static XmlDocument locatorsDoc = null;

        #endregion Global declarations

        #region Public methods

        /// <summary>
        /// Open and NDTV Home page
        /// </summary>
        public void OpenNdtv()
        {
            ndtvWebDriver = new ChromeDriver();
            homeURL = ConfigurationManager.AppSettings["NDTVUrl"];
            ndtvWebDriver.Navigate().GoToUrl("https://www.ndtv.com/");
            ndtvWebDriver.Manage().Window.Maximize();
            ndtvWebDriverWait = new WebDriverWait(ndtvWebDriver, TimeSpan.FromSeconds(15));
        }

        /// <summary>
        /// Navigate to NDTV weather section
        /// </summary>
        public void OpenWeatherSection()
        {
            var submenut = ndtvWebDriver.FindElement(By.Id(ReadElement("SubMenuLink")));
            ndtvWebDriverWait.Until(driver => submenut);
            submenut.Click();
            var weather = ndtvWebDriver.FindElement(By.XPath(ReadElement("WeatherLink")));
            ndtvWebDriverWait.Until(ExpectedConditions.ElementToBeClickable(weather));
            weather.Click();
        }

        /// <summary>
        /// Search for a city and pin in NDTV weather page
        /// </summary>
        /// <param name="cityName"></param>
        public void SearchForCityAndPin(string cityName)
        {
            Thread.Sleep(1000);
            var serachText = ndtvWebDriver.FindElement(By.Id(ReadElement("SearchCityTextBox")));
            ndtvWebDriverWait.Until(driver => serachText);
            serachText.SendKeys(cityName);

            var checkbox = ndtvWebDriver.FindElement(By.Id(cityName));
            ndtvWebDriverWait.Until(ExpectedConditions.ElementToBeClickable(checkbox));
            if (!checkbox.Selected)
            {
                checkbox.Click();
            }
            var cityText = ndtvWebDriver.FindElements(By.ClassName(ReadElement("CityCheckbox")));
            foreach (var element in cityText)
            {
                var text = element.Text;
                if (text == cityName)
                {
                    element.Click();
                    break;
                }
            }
        }

        /// <summary>
        /// Get the forecast deatils for a city
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public int GetForecastDetailsForCity(string cityName)
        {
            var webHumidity = "";
            var humidityText = ndtvWebDriver.FindElements(By.XPath(ReadElement("WeatherDetailsLabel")));
            foreach (var a in humidityText)
            {
                var text = a.Text;
                if (text.Contains("Humidity"))
                {
                    webHumidity = text.Split(":")[1].Replace("%", string.Empty);
                    break;
                }
            }

            return Convert.ToInt32(webHumidity);
        }

        /// <summary>
        /// Kill NDTV driver instance
        /// </summary>
        public void CloseTheDriver()
        {
            ndtvWebDriver.Quit();
        }

        #endregion Public methods

        #region Private methods
        
        /// <summary>
        /// Provides the element information from the locators file
        /// </summary>
        /// <param name="elementName">Input element</param>
        /// <returns>Element value</returns>
        private string ReadElement(string elementName)
        {
            string elementValue = "";
            if (locatorsDoc == null)
            {
                locatorsDoc = new XmlDocument();
                locatorsDoc.Load(AppContext.BaseDirectory + "PageLocators\\NDTVPage.xml");
            }
            var elt = locatorsDoc.SelectSingleNode("//Element[@name='" + elementName + "']") as XmlElement;
            if (elt != null)
            {
                elementValue = elt.GetAttribute("value");
            }

            return elementValue;
        }

        #endregion Private methods
    }
}