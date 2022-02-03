using MyWeatherApp.Helper;
using MyWeatherApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWeatherPage : ContentPage
    {
        public CurrentWeatherPage()
        {
            InitializeComponent();
            GetWeatherInfo();
        }

        private async void GetWeatherInfo()
        {
            var units = Preferences.Get("units", "default_value");
            unitsText.Text = units == "imperial" ? "F" : "C" ;
            var url = $"https://api.openweathermap.org/data/2.5/weather?q=Sydney&appid=07dfd4e58422eadf50777ee146646118&units={units}";
            var result = await ApiCaller.Get(url);
            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);

                    cityText.Text = weatherInfo.name.ToUpper();
                    temperatureText.Text = weatherInfo.main.temp.ToString("0");
                    humidityText.Text = $"{weatherInfo.main.humidity}%";
                    pressureText.Text = $"{weatherInfo.main.pressure} hpa";
                    windText.Text = $"{weatherInfo.wind.speed} m/s";
                    cloudinessText.Text = $"{weatherInfo.clouds.all}%";
                   
                    var dt = new DateTime().ToUniversalTime().AddSeconds(weatherInfo.dt);
                    dateText.Text = dt.ToString("MMM dd").ToUpper();
                    Console.WriteLine(dateText.Text);
                    GetForecast();

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Get Weather Info", "API current weather info not found", "OK");
            }

        
        }

        private async void GetForecast()
        {
            var units = Preferences.Get("units", "metric");
            unitsText.Text = units == "imperial" ? "F" : "C";
            var url = $"http://api.openweathermap.org/data/2.5/forecast?q=Sydney&appid=07dfd4e58422eadf50777ee146646118&units={units}";
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    List<List> allList = new List<List>();

                    foreach (var list in forcastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            allList.Add(list);
                    }

                    dayOneText.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd dd MMM");
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                    tempOneText.Text = allList[0].main.temp.ToString("0");

                    dayTwoText.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd dd MMM");
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                    tempTwoText.Text = allList[1].main.temp.ToString("0");

                    dayThreeText.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd dd MMM");
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                    tempThreeText.Text = allList[2].main.temp.ToString("0");

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Get Forecast", "API Forecast info not found", "OK");
            }
        }
    }
}