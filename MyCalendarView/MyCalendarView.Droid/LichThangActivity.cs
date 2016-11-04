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
using Java.IO;
using Lunar;
using Newtonsoft.Json;
using Console = System.Console;

namespace MyCalendarView.Droid
{
    [Activity(Label = "LichThang")]
    public class LichThangActivity : Activity, GestureDetector.IOnGestureListener
    {
        GridView gv;
        private JavaList<GridItem> lItems;
        private DateTime currentDateTime;
        private CustomGesture gestureListener;
        List<SuKien>  lEvents = new List<SuKien>();
		TextView txtSuKien;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.ActionBar.Hide();
            // Create your application here
            currentDateTime = DateTime.Now;
            SetContentView(Resource.Layout.LichThang);
            gv = FindViewById<GridView>(Resource.Id.gridview);
			txtSuKien = FindViewById<TextView>(Resource.Id.txtLichThang_TenSuKien);
            lEvents = JsonConvert.DeserializeObject<List<SuKien>>(Intent.GetStringExtra("ListSuKien"));
            gestureListener = new CustomGesture(this);
            gv.SetOnTouchListener(gestureListener);
            gestureListener.OnSwipeDown += delegate (object sender, EventArgs args) { currentDateTime = currentDateTime.AddYears(-1); LoadMonthCalendar(); };
            gestureListener.OnSwipeLeft += delegate (object sender, EventArgs args) { currentDateTime = currentDateTime.AddMonths(1); LoadMonthCalendar(); };
            gestureListener.OnSwipeRight += delegate (object sender, EventArgs args) { currentDateTime = currentDateTime.AddMonths(-1); LoadMonthCalendar(); };
            gestureListener.OnSwipeTop += delegate (object sender, EventArgs args) { currentDateTime = currentDateTime.AddYears(1); LoadMonthCalendar(); };
            LoadMonthCalendar();
        }

        private void LoadMonthCalendar()
        {
            Console.WriteLine("load");
            gv.Adapter = new GridItemAdapter(getDatesOfMonth(currentDateTime.Month, currentDateTime.Year), this,
                DateTime.Now.Month);
            gv.ItemClick += GridviewOnItemClick;
            TextView tvThang = FindViewById<TextView>(Resource.Id.tvLichThang_Thang);
            tvThang.Text = "Tháng " + currentDateTime.Month + "/" + currentDateTime.Year;
        }

        private void GridviewOnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
			SuKien sk = haveSuKien(lItems[itemClickEventArgs.Position].lunarDate, lItems[itemClickEventArgs.Position].curDate);
			if (sk != null)
			{
				txtSuKien.Text = sk.TenSuKien;
			}
			else {
				txtSuKien.Text = "";
			}
        }

        private JavaList<GridItem> getDatesOfMonth(int month, int year)
        {
            lItems = new JavaList<GridItem>();
            /*for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                lItems.Add(new GridItem(i.ToString(), 0));
            }*/
            DateTime firstDayOfMonth = new DateTime(year,month,1);
            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime lastSunday;
            int delta = DayOfWeek.Monday - firstDayOfMonth.DayOfWeek;
            DateTime firstMonday = firstDayOfMonth.AddDays(delta);
            if (lastDayOfMonth.DayOfWeek == DayOfWeek.Sunday)
            {
                lastSunday = lastDayOfMonth;
            }
            else
            {
                delta = lastDayOfMonth.DayOfWeek - DayOfWeek.Sunday;
                lastSunday = lastDayOfMonth.AddDays(7 - delta);
            }

            lItems.Add(new GridItem("Hai", false, true));
            lItems.Add(new GridItem("Ba", false, true));
            lItems.Add(new GridItem("Tư", false, true));
            lItems.Add(new GridItem("Năm", false, true));
            lItems.Add(new GridItem("Sáu", false, true));
            lItems.Add(new GridItem("Bảy", false, true));
            lItems.Add(new GridItem("C.N", false, true));


            for (int i = 0; i <= (lastSunday - firstMonday).Days; i++)
            {
                DateTime d = firstMonday.AddDays(i);
                clsLunar lunar = new clsLunar();
                clsLunar.LunarInfo info = lunar.GetAllLunarInfo(d.Day, d.Month, d.Year, 7);
                bool isDiff = false;
                bool isHaveEvt = false;
                if (d.Month != month)
                    isDiff = true;
                if (haveSuKien(info, d) != null)
                {
                    isHaveEvt = true;
                }
                lItems.Add(new GridItem(d, info, isDiff,false,isHaveEvt));
            }
            return lItems;
        }

        public bool OnDown(MotionEvent e){ return true;}
        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY){ return true;}
        public void OnLongPress(MotionEvent e) { }
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) { return true;}
        public void OnShowPress(MotionEvent e) { }
        public bool OnSingleTapUp(MotionEvent e) { return false;}

        public override bool OnTouchEvent(MotionEvent e)
        {
            Console.WriteLine("touch");
            gestureListener.getDetector().OnTouchEvent(e);
            return false;
        }

        private SuKien haveSuKien(clsLunar.LunarInfo info, DateTime d)
        {
            // need to get event by monthly, by yearly or no repeat event
            var b = lEvents.Where(a => (a.AmLich == 1 && a.Ngay == (int)info.dLunarDate.dDay && a.Thang == (int)info.dLunarDate.dMonth)).ToList();// get Lunar Events
            b.AddRange(lEvents.Where(a => a.AmLich == 0 && a.Ngay == d.Day && a.Thang == d.Month).ToList()); //Get Solar Events

            if (b.Count() > 0)
            {
                return b[0];
            }
            return null;
        }

    }
}