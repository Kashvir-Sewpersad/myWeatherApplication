using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static myWeatherApplication.WeatherInfo;

namespace myWeatherApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupAutoComplete();  
        }

        string APIKey = "c4b007fac292ef6cb7a42f910bb1e6dc";

        
        private System.Windows.Forms.Timer searchTimer;  
        private ListBox suggestionBox;                   

        private void SetupAutoComplete()
        {
           
            searchTimer = new System.Windows.Forms.Timer();
            searchTimer.Interval = 400;
            searchTimer.Tick += SearchTimer_Tick;

           
            suggestionBox = new ListBox();
            suggestionBox.Visible = false;
            suggestionBox.Font = TBCity.Font;
            suggestionBox.Click += SuggestionBox_Click;
            suggestionBox.KeyDown += SuggestionBox_KeyDown;

          
            this.Controls.Add(suggestionBox);
            suggestionBox.BringToFront();

           
            TBCity.TextChanged += TBCity_TextChanged;
            TBCity.KeyDown += TBCity_KeyDown;
            TBCity.LostFocus += TBCity_LostFocus;
        }

        
        private void TBCity_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            if (TBCity.Text.Length >= 2)        
                searchTimer.Start();
            else
                HideSuggestions();
        }

        
        private async void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            await FetchCitySuggestions(TBCity.Text);
        }

        private async Task FetchCitySuggestions(string query)
        {
            try
            {
                string url = $"http://api.openweathermap.org/geo/1.0/direct?q={Uri.EscapeDataString(query)}&limit=5&appid={APIKey}";

                using (WebClient web = new WebClient())
                {
                    string json = await web.DownloadStringTaskAsync(url);
                    var cities = JsonConvert.DeserializeObject<List<CitySearchResult>>(json);

                    if (cities == null || cities.Count == 0)
                    {
                        HideSuggestions();
                        return;
                    }

                  
                    suggestionBox.Items.Clear();
                    foreach (var city in cities)
                    {
                        string display = string.IsNullOrEmpty(city.state)
                            ? $"{city.name}, {city.country}"
                            : $"{city.name}, {city.state}, {city.country}";
                        suggestionBox.Items.Add(display);
                    }

                    
                    Point loc = TBCity.Parent.PointToScreen(new Point(TBCity.Left, TBCity.Bottom));
                    loc = this.PointToClient(loc);
                    suggestionBox.Location = loc;
                    suggestionBox.Width = TBCity.Width;
                    suggestionBox.Height = Math.Min(cities.Count * 18 + 6, 100);
                    suggestionBox.Visible = true;
                    suggestionBox.BringToFront();
                }
            }
            catch { HideSuggestions(); }
        }

      
        private void SuggestionBox_Click(object sender, EventArgs e)
        {
            AcceptSuggestion();
        }

        
        private void SuggestionBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) AcceptSuggestion();
            if (e.KeyCode == Keys.Escape) HideSuggestions();
        }

      
        private void TBCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && suggestionBox.Visible && suggestionBox.Items.Count > 0)
            {
                suggestionBox.Focus();
                suggestionBox.SelectedIndex = 0;
            }
            if (e.KeyCode == Keys.Enter) getWeather();
            if (e.KeyCode == Keys.Escape) HideSuggestions();
        }

        private void TBCity_LostFocus(object sender, EventArgs e)
        {
            
            System.Threading.Tasks.Task.Delay(200).ContinueWith(_ =>
            {
                this.Invoke((Action)(() =>
                {
                    if (!suggestionBox.Focused) HideSuggestions();
                }));
            });
        }

        private void AcceptSuggestion()
        {
            if (suggestionBox.SelectedItem != null)
            {
              
                string selected = suggestionBox.SelectedItem.ToString();
                TBCity.Text = selected.Split(',')[0].Trim();
                HideSuggestions();
                TBCity.SelectionStart = TBCity.Text.Length;
                getWeather();
            }
        }

        private void HideSuggestions()
        {
            suggestionBox.Visible = false;
            suggestionBox.Items.Clear();
        }

     
        void getWeather()
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    
                    string url = string.Format(
                        "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric",
                        TBCity.Text, APIKey);

                    var json = web.DownloadString(url);
                    WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                    picIcon.ImageLocation = "https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png";
                    labCondition.Text = Info.weather[0].main;
                    labDetails.Text = Info.weather[0].description;
                    labSunset.Text = convertDateTime(Info.sys.sunset).ToShortTimeString();
                    labSunrise.Text = convertDateTime(Info.sys.sunrise).ToShortTimeString();
                    labWindSpeed.Text = Info.wind.speed.ToString();
                    labPressure.Text = Info.main.pressure.ToString();

                    
                    labTempMin.Text = Math.Round(Info.main.temp_min, 1) + " °C";
                    labTempMax.Text = Math.Round(Info.main.temp_max, 1) + " °C";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not retrieve weather data.\n" + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        DateTime convertDateTime(long millisec)
        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(millisec).ToLocalTime();
            return day;
        }

        
        private void btnSearch_Click(object sender, EventArgs e) => getWeather();
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void labSunrise_Click(object sender, EventArgs e) { }
        private void picIcon_Click(object sender, EventArgs e) { }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}