using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Color = Android.Graphics.Color;
using Object = Java.Lang.Object;

namespace MyCalendarView.Droid
{
    class GridItemAdapter : BaseAdapter
    {
        Context context;
        private JavaList<GridItem> items;
        private LayoutInflater inflater;
        private int currenntMonth;
        public GridItemAdapter(JavaList<GridItem> litems, Context c, int month)
        {
            items = litems;
            context = c;
            currenntMonth = month;
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override Object GetItem(int position)
        {
            return items.Get(position);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            }
            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.LichThangItem,parent, false);
            }

            TextView txtNgay = convertView.FindViewById<TextView>(Resource.Id.tvLichThang_Ngay);
            TextView txtNgayAL = convertView.FindViewById<TextView>(Resource.Id.tvLichThang_NgayAL);
            ImageView imgHaveEvt = convertView.FindViewById<ImageView>(Resource.Id.imgHaveEvent);

            txtNgay.Text = items[position].Date;
            txtNgayAL.Text = items[position].DateAL;
            if (items[position].isDiffMonth)
            {
                txtNgay.SetTextColor(Color.Gray);
                txtNgayAL.SetTextColor(Color.Gray);
                convertView.SetBackgroundColor(Color.WhiteSmoke);
            }
            else
            {
                convertView.SetBackgroundColor(Color.White);
            }
            if (items[position].isHeader)
            {
                txtNgay.Gravity = GravityFlags.Center;
                txtNgayAL.Visibility = ViewStates.Gone;
                if (items[position].Date == "C.N")
                {
                    txtNgay.SetTextColor(Color.Red);
                }
            }
            else if (items[position].curDate.DayOfWeek == DayOfWeek.Sunday)
            {
                txtNgay.SetTextColor(Color.Red);
                txtNgayAL.SetTextColor(Color.Red);
            }
            if (!items[position].isHaveEvent)
            {
                imgHaveEvt.Visibility = ViewStates.Gone;
            }
            return convertView;
        }
    }
}