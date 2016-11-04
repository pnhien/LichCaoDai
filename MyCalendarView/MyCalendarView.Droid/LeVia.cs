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
using SQLite;

namespace MyCalendarView.Droid
{
    [Table("SuKien")]
    public class SuKien
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int AmLich { get; set; }
        public string TenSuKien { get; set; }
        public string NghiLe { get; set; }
    }

    [Table("ThanhGiao")]
    public class ThanhGiao
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }
        public string NoiDung { get; set; }
        public string TacGia { get; set; }
        public string NguonGoc { get; set; }
        public int Loai { get; set; }
        public string Tag { get; set; }
    }
}