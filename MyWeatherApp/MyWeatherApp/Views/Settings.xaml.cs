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
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            if (Preferences.Get("units", "metric") == "metric")
            {
                unitsSwitch.IsToggled = false;
            } else
            {
                unitsSwitch.IsToggled = true;
            }
        }

        private void UnitsSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (unitsSwitch.IsToggled)
            {
                unitsLabel.Text = "Imperial";
                Preferences.Set("units", "imperial");
            }
            else
            {
                unitsLabel.Text = "Metric";
                Preferences.Set("units", "metric");
            }
        }
    }
}