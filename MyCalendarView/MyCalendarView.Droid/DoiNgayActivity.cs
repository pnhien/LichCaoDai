using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Lunar;

namespace MyCalendarView.Droid
{
    [Activity(Label = "DoiNgayActivity")]
    public class DoiNgayActivity : Activity
    {
        private DatePicker dpDL;
        DatePicker dpAL;
        private TextView labelNamAL;
		private bool isDLChange = false;
		private bool isALChange = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.ActionBar.Hide();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.DoiNgay);
            dpDL = FindViewById<DatePicker>(Resource.Id.dpDuongLich);
            dpAL = FindViewById<DatePicker>(Resource.Id.dpAmLich);
            Button btnToday = FindViewById<Button>(Resource.Id.btntoday);
            labelNamAL = FindViewById<TextView>(Resource.Id.tvNamAL);
            dpDL.CalendarViewShown = false;
            dpAL.CalendarViewShown = false;
			//dpAL.FindViewById(Resources.GetIdentifier("year", "id", "android")).Visibility = ViewStates.Gone;

            btnToday.Click += (s, e) =>
            {
                dpDL.DateTime = DateTime.Today;
            };
			dpDL.DateTime = DateTime.Today;
			DateTime t = ConvertToLunarDate(dpDL.DateTime.Day, dpDL.DateTime.Month - 1, dpDL.DateTime.Year);
			dpAL.DateTime = DateTime.Today.AddDays(5);
			dpAL.DateTime = t;

            dpDL.CalendarView.DateChange += CalendarViewOnDateChange;
			dpAL.CalendarView.DateChange += CalendarLunarViewOnDateChange;
        }

		private void CalendarLunarViewOnDateChange(object sender, CalendarView.DateChangeEventArgs dateChangeEventArgs)
		{
			//if (isDLChange)
			{
				ConvertToSolarDate(dateChangeEventArgs.DayOfMonth, dateChangeEventArgs.Month, dateChangeEventArgs.Year);
			}
		}

        private void CalendarViewOnDateChange(object sender, CalendarView.DateChangeEventArgs dateChangeEventArgs)
        {
			//if (!isDLChange)
			{
				ConvertToLunarDate(dateChangeEventArgs.DayOfMonth, dateChangeEventArgs.Month, dateChangeEventArgs.Year);
			}
        }

		private DateTime ConvertToSolarDate(int day, int month, int year)
		{
			DateTime temp = DateTime.Today;
			if (isALChange)
			{
				Console.WriteLine("To Solar:" +  day.ToString() + "--" + month.ToString());
				try
				{
					Lunar.clsLunar lunar = new clsLunar();
					Lunar.clsLunar.DoubleDateTime dl = lunar.Lunar2Solar((double)day, (double)month+1, (double)year, 1.0, 7);
					temp = new DateTime((int)dl.dYear, (int)dl.dMonth, (int)dl.dDay);
					if (temp != dpDL.DateTime)
					{
						//isALChange = false;
						isDLChange = false;
						dpDL.DateTime = temp;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(day.ToString() + "--" + month.ToString() + "ex:" + ex.Message);
				}
				Console.WriteLine("solar changed");
			}
			isALChange = true;
			return temp;
		}

		private DateTime ConvertToLunarDate(int day, int month, int year) {
			DateTime temp = DateTime.Today;
			if (isDLChange)
			{
				Console.WriteLine("To Lunar:" + day.ToString() + "--" + month.ToString());
				try
				{
					DateTime date = correctDateTime(day, month + 1, year);
					isDLChange = false;
					if (date != dpDL.DateTime)
						dpDL.DateTime = date;
					Lunar.clsLunar lunar = new clsLunar();
					Lunar.clsLunar.LunarInfo info = lunar.GetAllLunarInfo(date.Day, date.Month, date.Year, 7);
					temp = new DateTime((int)info.dLunarDate.dYear,
						(int)info.dLunarDate.dMonth, (int)info.dLunarDate.dDay);
					if (temp != dpAL.DateTime)
					{
						//isDLChange = false;
						isALChange = false;
						dpAL.DateTime = temp;
						labelNamAL.Text = info.sYearCanChi;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(day.ToString() + "--" + month.ToString() + "ex:" + ex.Message);
				}
			}
			isDLChange = true;
			return temp;
		}

        private DateTime correctDateTime(int dateOfMonth, int month, int year)
        {
            month = month > 12 ? 12 : month;
            month = month < 1 ? 1 : month;
            dateOfMonth = dateOfMonth > 31 ? 31 : dateOfMonth;
            dateOfMonth = dateOfMonth < 1 ? 1 : dateOfMonth;
            if (month == 2)
            {
                if (dateOfMonth >= 29 && DateTime.IsLeapYear(year))
                {
                    dateOfMonth = 29;
                }
                else if (dateOfMonth > 28 && !DateTime.IsLeapYear(year))
                {
                    dateOfMonth = 28;
                }
            }
            return new DateTime(year,month,dateOfMonth);
        }
    }
}