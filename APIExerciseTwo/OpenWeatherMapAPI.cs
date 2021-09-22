using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace APIExerciseTwo
{
    class OpenWeatherMapAPI
    {

        HttpClient Client;
        readonly string API_Key;

        /// <summary>
        /// Constructor requires an APIKey for OpenWeatherMapAPI
        /// </summary>
        /// <param name="apiKey"></param>
        public OpenWeatherMapAPI(string apiKey)
        {
            Client = new HttpClient();
            API_Key = apiKey;

        }

        public OpenWeatherMapAPI()
        {

            API_Key = "261c6078621346b5f498b4d9d656d959";
            Client = new HttpClient();
        }



        public void GetWeather(string cityName)
        {
            //TODO: Refactor to return weather data in a string array
            try
            {
                var cityFound = Client.GetStringAsync($"http://api.openweathermap.org/data/2.5/weather?q={cityName}&&units=imperial&appid={API_Key}").Result;
                Console.WriteLine("\n" + JObject.Parse(cityFound).GetValue("main").ToString());
            }
            catch (Exception)
            {
                Console.WriteLine("\nI'm sorry, I couldn't locate your requested city.");
            }

        }

        /*

        https://openweathermap.org/current

        // api call for weather by city name

        api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key}

         */

    }
}
