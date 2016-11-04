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
    class GridItem
    {
        public string Date { get; set; }
        public string DateAL { get; set; }
        public bool isDiffMonth { get; set; }
        public bool isHeader { get; set; }
        public DateTime curDate { get; set; }
		public clsLunar.LunarInfo lunarDate { get; set; }
        public bool isHaveEvent { get; set; }

		public GridItem(DateTime date, clsLunar.LunarInfo lunar, bool isdiff, bool ishead =false, bool isEvt = false)
        {
            curDate = date;
			lunarDate = lunar;
            Date = curDate.Day.ToString();
			if (lunarDate.dLunarDate.dDay == 1){
				DateAL = lunarDate.dLunarDate.dDay.ToString() + "/" + lunarDate.dLunarDate.dMonth.ToString();
			}
			else { 
				DateAL = lunarDate.dLunarDate.dDay.ToString();
			}
            isDiffMonth = isdiff;
            isHeader = ishead;
            isHaveEvent = isEvt;
        }

		public GridItem(string date, clsLunar.LunarInfo lunar, bool isdiff, bool ishead = false, bool isEvt = false)
        {
            Date = date;
			lunarDate = lunar;
			DateAL = lunarDate.dLunarDate.dDay.ToString();
            isDiffMonth = isdiff;
            isHeader = ishead;
            isHaveEvent = isEvt;
        }

		public GridItem(string date, bool isdiff, bool ishead = true, bool isEvt = false)
		{
			Date = date;
			DateAL = lunarDate.dLunarDate.dDay.ToString();
			isDiffMonth = isdiff;
			isHeader = ishead;
			isHaveEvent = isEvt;
		}
    }
}