using Microsoft.EntityFrameworkCore;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI.Xaml;
using Windows.System;

namespace Session_4_Dennis_Hilfinger;

public partial class AllRaceResultsPage : ContentPage, IQueryAttributable
{
    DispatcherTimer timer = new DispatcherTimer();
    User user;
    public AllRaceResultsPage()
	{
		InitializeComponent();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += timerTick;
        timer.Start();
    }

    private void timerTick(object? sender, object e)
    {
        DateTime targetTime = new DateTime(2026, 9, 5, 6, 0, 0);
        DateTime currentTime = DateTime.Now;
        TimeSpan timeDiff = targetTime - currentTime;

        TimerLabel.Text = string.Format("{0} days {1} hours and {2} minutes until the race starts!",
            timeDiff.Days,
            timeDiff.Hours,
            timeDiff.Minutes);
    }

    private void Logout(object? sender, EventArgs e)
    {
        AppShell.Current.GoToAsync("//MainPage");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        user = (User)query["User"];
        FillSelections();
    }

    private void FillSelections()
    {
        using (var db = new MarathonDB())
        {
            var marathons = db.Marathons
                .Include(m => m.CountryCodeNavigation)
                .ToList();
            MarathonPicker.Items.Clear();
            foreach (var mar in marathons)
            {
                MarathonPicker.Items.Add($"{mar.YearHeld} - {mar.CountryCodeNavigation.CountryName}");
            }
            
            var eventTypes = db.EventTypes.ToList();
            EventTypePicker.Items.Clear();
            foreach (var et in eventTypes)
            {
                EventTypePicker.Items.Add(et.EventTypeName.ToString());
            }

            var genders = db.Genders.ToList();
            GenderPicker.Items.Clear();
            foreach(var g in genders)
            {
                GenderPicker.Items.Add(g.Gender1);
            }
            GenderPicker.Items.Add("Any");

            AgeCategoryPicker.Items.Clear();
            AgeCategoryPicker.Items.Add("Under 18");
            AgeCategoryPicker.Items.Add("18-29");
            AgeCategoryPicker.Items.Add("30-39");
            AgeCategoryPicker.Items.Add("40-55");
            AgeCategoryPicker.Items.Add("56-70");
            AgeCategoryPicker.Items.Add("Over 70");
            AgeCategoryPicker.Items.Add("All");
        }
    }


    private List<DateTime> GetAgeCategory(string category)
    {
        DateTime dateMin = DateTime.MinValue;
        DateTime dateMax = DateTime.MaxValue;
        switch (category)
        {
            case "Under 18":
                dateMin = DateTime.Now.AddYears(-0);
                dateMax = DateTime.Now.AddYears(-(17 + 1));
                break;
            case "18-29":
                dateMin = DateTime.Now.AddYears(-18);
                dateMax = DateTime.Now.AddYears(-(29 + 1));
                break;
            case "30-39":
                dateMin = DateTime.Now.AddYears(-30);
                dateMax = DateTime.Now.AddYears(-(39 + 1));
                break;
            case "40-55":
                dateMin = DateTime.Now.AddYears(-40);
                dateMax = DateTime.Now.AddYears(-(55 + 1));
                break;
            case "56-70":
                dateMin = DateTime.Now.AddYears(-56);
                dateMax = DateTime.Now.AddYears(-(70 + 1));
                break;
            case "Over 70":
                dateMin = DateTime.Now.AddYears(-70);
                dateMax = DateTime.Now.AddYears(-(200 + 1));
                break;
        }
        return new List<DateTime>() { dateMin, dateMax };
    }

    private void Search(object? sender, EventArgs e)
    {
        using (var db = new MarathonDB())
        {
            var ev = db.Events
                .Include(e => e.Marathon)
                .ThenInclude(m => m.CountryCodeNavigation)
                .ToList();
            if (MarathonPicker.SelectedItem != null)
            {
                string selectedMarathon = MarathonPicker.SelectedItem.ToString();
                int yearHeld = int.Parse(selectedMarathon.Split('-')[0].Trim());
                string Country = selectedMarathon.Split("-")[1].Trim();
                var marathon = ev.FirstOrDefault(e => e.Marathon.YearHeld == yearHeld && e.Marathon.CountryCodeNavigation.CountryName == Country);
                if (EventTypePicker.SelectedItem != null)
                {
                    string eventType = EventTypePicker.SelectedItem.ToString();

                }
                else
                {
                    DisplayAlert("Information", "Please select a race event to show results", "Ok");
                    return;
                }
            }
            else
            {
                DisplayAlert("Information", "Please select a marathon to show results", "Ok");
                return;
            }
            
        }

    }
}