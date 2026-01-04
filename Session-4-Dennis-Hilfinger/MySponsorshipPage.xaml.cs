using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Windows.System;

namespace Session_4_Dennis_Hilfinger;

public partial class MySponsorshipPage : ContentPage, IQueryAttributable
{
    DispatcherTimer timer = new DispatcherTimer();
    User user;
	public MySponsorshipPage()
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
        FillData();
    }

    private void FillData()
    {
        using(var db = new MarathonDB())
        {
            var registration = db.Registrations
                .Include(r => r.Charity)
                .FirstOrDefault(r => r.RunnerId == user.Runners.First().RunnerId);
            CharityImg.Source = ImageSource.FromFile(registration.Charity.CharityLogo);
        }
    }

    private void Cancel(object sender, EventArgs e)
    {
        ShellNavigationQueryParameters userData = new ShellNavigationQueryParameters()
        {
            { "User", user }
        };
        AppShell.Current.GoToAsync("RunnerPage", userData);
    }
}