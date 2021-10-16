using Newtonsoft.Json;
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
        #region Root & Results

        public class Coord
        {
            public double Lon { get; set; }
            public double Lat { get; set; }
        }

        public class Weather
        {
            public int Id { get; set; }
            public string Main { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
        }

        public class Main
        {
            public int Temp { get; set; }
            public double FeelsLike { get; set; }
            public double TempMin { get; set; }
            public double TempMax { get; set; }
            public int Pressure { get; set; }
            public int Humidity { get; set; }
        }

        public class Wind
        {
            public double Speed { get; set; }
            public int Deg { get; set; }
            public double Gust { get; set; }
        }

        public class Rain
        {
            public double _1h { get; set; }
        }

        public class Clouds
        {
            public int All { get; set; }
        }

        public class Sys
        {
            public int Type { get; set; }
            public int Id { get; set; }
            public string Country { get; set; }
            public int Sunrise { get; set; }
            public int Sunset { get; set; }
        }

        public class Root
        {
            public Coord Coord { get; set; }
            public List<Weather> Weather { get; set; }
            public string Base { get; set; }
            public Main Main { get; set; }
            public int Visibility { get; set; }
            public Wind Wind { get; set; }
            public Rain Rain { get; set; }
            public Clouds Clouds { get; set; }
            public int Dt { get; set; }
            public Sys Sys { get; set; }
            public int Timezone { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public int Cod { get; set; }
        }

        #endregion
    */
    
    }
}
