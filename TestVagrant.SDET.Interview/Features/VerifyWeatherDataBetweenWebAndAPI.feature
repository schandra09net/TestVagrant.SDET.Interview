Feature: VerifyWeatherDataBetweenWebAndAPI
	This testcase will verify the weather humidity value from the NDTV weather and Openweather REST API

@Sanity
Scenario Outline: Check weather details for a city
	Given The NDTV page is opened
	Then Navigate to weather section
	And Search for a city <CityName> and add to map
	Then Get the forecast details of added city <CityName>
	And Invoke the OpenWeather rest api for the same city <CityName>
	Then Verify the details from web and api with offset <Offset>
	And Close the NDTV webpage

	Examples:
		| CityName  | Offset |
		| Mumbai    | 20     |
		| Bengaluru | 20     |
		| New Delhi | 20     |