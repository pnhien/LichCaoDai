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
        public bool isHaveEvent { get; set; }

        public GridItem(DateTime date, string dateAL, bool isdiff, bool ishead =false, bool isEvt = false)
        {
            curDate = date;
            Date = curDate.Day.ToString();
            DateAL = dateAL;
            isDiffMonth = isdiff;
            isHeader = ishead;
            isHaveEvent = isEvt;
        }

        public GridItem(string date, string dateAL, bool isdiff, bool ishead = false, bool isEvt = false)
        {
            Date = date;
            DateAL = dateAL;
            isDiffMonth = isdiff;
            isHeader = ishead;
            isHaveEvent = isEvt;
        }

    }
}