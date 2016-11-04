using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Graphics;
using Lunar;
using SQLite;
using Path = System.IO.Path;
using Newtonsoft.Json;

namespace MyCalendarView.Droid
{
    [Activity(Label = "Lich Cao Dai", MainLauncher = true, Icon = "@drawable/CaoDaiIcon")]
    public class MainActivity : FragmentActivity, GestureDetector.IOnGestureListener
    {
        List<double> NgayChay = new List<double>() { 1, 8, 14, 15, 18, 23, 24, 28, 29, 30 };
        public List<SuKien> lLeVia = new List<SuKien>();
        public List<ThanhGiao> lThanhGiao = new List<ThanhGiao>();
        private List<string> lImg = new List<string>();
        private CustomGesture gestureListener;
        private DateTime currentDateTime;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.ActionBar.Hide();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LichAm);
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            currentDateTime = DateTime.Now;
            var dbFile = Path.Combine(docFolder, "ThanhGiao.db"); // FILE NAME TO USE WHEN COPIED
            if (!System.IO.File.Exists(dbFile))
            {
                var s = Application.Context.Assets.Open("ThanhGiao.db");  // DATA FILE RESOURCE ID
                FileStream writeStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(s, writeStream);
            }
            ReadDatabase();
            var view = this;
            gestureListener = new CustomGesture(this);
            gestureListener.OnSwipeDown += delegate (object sender, EventArgs args) { currentDateTime = currentDateTime.AddMonths(-1); SetCalendarView(view.FindViewById<View>(Resource.Layout.LichAm), currentDateTime); };
            gestureListener.OnSwipeLeft += delegate (object sender, EventArgs args) { currentDateTime = currentDateTime.AddDays(1); SetCalendarView(view.FindViewById<View>(Resource.Layout.LichAm), currentDateTime); };
            gestureListener.OnSwipeRight += delegate (object sender, EventArgs args) { currentDateTime = currentDateTime.AddDays(1); SetCalendarView(view.FindViewById<View>(Resource.Layout.LichAm), currentDateTime); };
            gestureListener.OnSwipeTop += delegate (object sender, EventArgs args) { currentDateTime = currentDateTime.AddMonths(-1); SetCalendarView(view.FindViewById<View>(Resource.Layout.LichAm), currentDateTime); };
            
            SetCalendarView(view.FindViewById<View>(Resource.Layout.LichAm), currentDateTime);
        }

        private bool isNgayChay(clsLunar.LunarInfo info)
        {
            if (info.dMonthLenght <= 29) //thang thiếu
            {
                NgayChay[NgayChay.Count - 1] = 27;
            }
            else// tháng đủ
            {
                NgayChay[NgayChay.Count - 1] = 30;
            }
            return NgayChay.Contains(info.dLunarDate.dDay);
        }

        void SetCalendarView(View view, DateTime date)
        {
            var labelTN = FindViewById<TextView>(Resource.Id.tvThanhNgon);
            var labelNguon = FindViewById<TextView>(Resource.Id.tvNguon);
            var labelThang = FindViewById<TextView>(Resource.Id.tvThang);
            var labelNgay = FindViewById<TextView>(Resource.Id.tvNgay);
            var labelThu = FindViewById<TextView>(Resource.Id.tvThu);
            var labelNgayAL = FindViewById<TextView>(Resource.Id.tvNgayAL);
            var labelThangDu = FindViewById<TextView>(Resource.Id.tvThangDu);
            var labelSuKien = FindViewById<TextView>(Resource.Id.tvSuKien);
            ImageView imgHinh = FindViewById<ImageView>(Resource.Id.imgHinh);
            Button btnLichNgay = FindViewById<Button>(Resource.Id.btnLichNgay);
            Button btnLichThang = FindViewById<Button>(Resource.Id.btnLichThang);
            Button btnDoiNgay = FindViewById<Button>(Resource.Id.btnDoiNgay);
            Button btnTienIch = FindViewById<Button>(Resource.Id.btnTienIch);
            /*labelTN.SetOnTouchListener(gestureListener);
            labelNguon.SetOnTouchListener(gestureListener);
            labelThang.SetOnTouchListener(gestureListener);
            labelNgay.SetOnTouchListener(gestureListener);
            labelThu.SetOnTouchListener(gestureListener);
            labelNgayAL.SetOnTouchListener(gestureListener);
            labelThangDu.SetOnTouchListener(gestureListener);
            labelSuKien.SetOnTouchListener(gestureListener);
            imgHinh.SetOnTouchListener(gestureListener);*/

            labelSuKien.Visibility = ViewStates.Gone;
            clsLunar lunar = new clsLunar();
            clsLunar.LunarInfo info = lunar.GetAllLunarInfo(date.Day, date.Month, date.Year, 7);
            labelNgayAL.Text = info.ToLunarDate();
            if (info.dMonthLenght > 29)
            {
                labelThangDu.Text = "Tháng Đủ";
                labelThangDu.SetTextColor(Android.Graphics.Color.Green);
            }
            else
            {
                labelThangDu.Text = "Tháng Thiếu";
                labelThangDu.SetTextColor(Android.Graphics.Color.Red);
            }
            if (info.dLeapMonth == info.dLunarDate.dMonth)
            {
                labelThangDu.Text += " - Tháng Nhuần";
            }
            if (isNgayChay(info))
            {
                labelThangDu.Text = "Ăn Chay - " + labelThangDu.Text;
                labelNgay.SetTextColor(Color.Pink);
                labelNgayAL.SetTextColor(Color.Pink);
            }
            else
            {
                labelNgay.SetTextColor(Color.White);
                labelNgayAL.SetTextColor(Color.White);
            }

            //imgHinh.Visibility = ViewStates.Gone;
            Random rand1 = new Random();
            int idx = rand1.Next(lThanhGiao.Count - 1);
            labelTN.Text = lThanhGiao[idx].NoiDung;//"Muốn nên Tiên, vô vi thanh tịnh\n Muốn về Thầy, nhất lịnh tưởng tin.\n Muốn tha, sớm tối cầu xin\n Muốn thành Bồ Tát, muôn nghìn công phu.";
            labelNguon.Text = lThanhGiao[idx].TacGia + " ";//"Thánh Ngôn Hiệp Tuyển";
            labelThang.Text = "Tháng " + date.Month.ToString() + " - " + date.Year.ToString();
            labelNgay.Text = date.Day.ToString();

            //info.dLunarDate.dDay = 15;
            //info.dLunarDate.dMonth = 10;
            SuKien sk = haveSuKien(info);
            //sk = lLeVia[0];
            if (sk != null)
            {
                labelSuKien.Visibility = ViewStates.Visible;
                labelSuKien.Text = sk.TenSuKien;
            }

            var imgId = getDrawableResources();
            imgHinh.SetImageResource(imgId);
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    labelThu.Text = "Thứ 2";
                    break;
                case DayOfWeek.Tuesday:
                    labelThu.Text = "Thứ 3";
                    break;
                case DayOfWeek.Wednesday:
                    labelThu.Text = "Thứ 4";
                    break;
                case DayOfWeek.Thursday:
                    labelThu.Text = "Thứ 5";
                    break;
                case DayOfWeek.Friday:
                    labelThu.Text = "Thứ 6";
                    break;
                case DayOfWeek.Saturday:
                    labelThu.Text = "Thứ 7";
                    break;
                case DayOfWeek.Sunday:
                    labelThu.Text = "Chủ Nhật";
                    break;
            }

            /*labelTN.Click += (s, e) =>
            {
                Toast.MakeText(this, "thanh ngon", ToastLength.Short).Show();
            };

            labelNguon.Click += (s, e) =>
            {
                Toast.MakeText(this, "Nguon", ToastLength.Short).Show();
            };

            imgHinh.Click += (s, e) =>
            {
                Toast.MakeText(this, "hinh anh", ToastLength.Short).Show();
            };*/

            btnLichNgay.Click += (s, e) =>
            {
                Toast.MakeText(this, "Lịch Ngày", ToastLength.Short).Show();
            };

            btnLichThang.Click += (s, e) =>
            {
                Toast.MakeText(this, "Lịch Tháng", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(LichThangActivity));
                
                intent.PutExtra("ListSuKien", JsonConvert.SerializeObject(lLeVia));
                StartActivity(intent);
            };

            btnDoiNgay.Click += (s, e) =>
            {
                Toast.MakeText(this, "Đổi Ngày", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(DoiNgayActivity));
                StartActivity(intent);
            };

            btnTienIch.Click += (s, e) =>
            {
                Toast.MakeText(this, "Tiện Ích", ToastLength.Short).Show();
            };
        }

        private int getDrawableResources()
        {
            foreach (var f in typeof(Resource.Drawable).GetFields())
            {
                if (f.Name.StartsWith("Main_"))
                {
                    lImg.Add(f.Name);
                }
            }
            if (lImg.Count > 0)
            {
                Random rand1 = new Random();
                int idx = rand1.Next(lImg.Count - 1);
                return (int)typeof(Resource.Drawable).GetField(lImg[idx]).GetValue(null);
            }
            return 0;
        }

        private string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ThanhGiao.db");
        private SQLiteConnection db;
        private void ReadDatabase()
        {
            db = new SQLiteConnection(dbPath);
            db.GetTableInfo("ThanhGiao");
            lThanhGiao = (from i in db.Table<ThanhGiao>() select i).ToList();

            db.GetTableInfo("SuKien");
            lLeVia = (from i in db.Table<SuKien>() select i).ToList();
        }

        private SuKien haveSuKien(clsLunar.LunarInfo info)
        {
            var b = lLeVia.Where(a => a.Ngay == (int)info.dLunarDate.dDay && a.Thang == (int)info.dLunarDate.dMonth && a.AmLich == 1).ToList();
            if (b.Count() > 0)
            {
                return b[0];
            }
            return null;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            Console.WriteLine("touching");
            gestureListener.getDetector().OnTouchEvent(e);
            return false;
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }

        public bool OnDown(MotionEvent e) { return true; }
        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY) { return true; }
        public void OnLongPress(MotionEvent e) { }
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) { return true; }
        public void OnShowPress(MotionEvent e) { }
        public bool OnSingleTapUp(MotionEvent e) { return false; }

    }
}


