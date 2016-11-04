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
            dpDL.CalendarView.DateChange += CalendarViewOnDateChange;
        }

        private void CalendarViewOnDateChange(object sender, CalendarView.DateChangeEventArgs dateChangeEventArgs)
        {
            Console.WriteLine(dateChangeEventArgs.DayOfMonth.ToString() + "--" + dateChangeEventArgs.Month.ToString());
            try
            {
                DateTime date = correctDateTime(dateChangeEventArgs.DayOfMonth, dateChangeEventArgs.Month+1, dateChangeEventArgs.Year);
                dpDL.DateTime = date;
                Lunar.clsLunar lunar = new clsLunar();
                Lunar.clsLunar.LunarInfo info = lunar.GetAllLunarInfo(date.Day, date.Month, date.Year, 7);
                dpAL.DateTime = new DateTime((int)info.dLunarDate.dYear,
                    (int)info.dLunarDate.dMonth, (int)info.dLunarDate.dDay);
                labelNamAL.Text = info.sYearCanChi;
            }
            catch (Exception ex)
            {

                Console.WriteLine(dateChangeEventArgs.DayOfMonth.ToString() + "--" + dateChangeEventArgs.Month.ToString() + "ex:"+ex.Message);
            }
            
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