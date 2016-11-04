using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lunar;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Lunar.clsLunar lunar = new clsLunar();
            clsLunar.LunarInfo info = lunar.GetAllLunarInfo(dateTimePicker1.Value.Day, dateTimePicker1.Value.Month, dateTimePicker1.Value.Year, 7);
            label1.Text = info.ToLunarDate() + "--" + info.ToLunarTietKhi();
            label1.Text += "- " + info.sDayNameOfWeek + ", ngày " + "\r\n";
            label1.Text += "- Ngày âm lịch: " + info.ToLunarDate() + "\r\n";
            label1.Text += "- Giờ: " + info.sHourCanChi + " - Ngày: " + info.sDayCanChi + " - Tháng: " + info.sMonthCanChi + " - Năm: " + info.sYearCanChi + "\r\n";
            label1.Text += info.dDayOfLeap == 1 ? "- Thuộc tháng nhuận" : "- Không thuộc tháng nhuận";
            label1.Text +=  "\r\n";
            label1.Text += info.dLeap == 0
                ? "- Không có tháng nhuận"
                : "- Tháng nhuận: tháng " + info.dLeapMonth + " (Từ ngày: " + info.dLeap2SolarFrom.ToString() +
                  " đến ngày: " + info.dLeap2SolarTo.ToString() + " Dương lịch)";
            label1.Text += "\r\n";
            label1.Text += info.dMonthLenght > 29
                ? "- " + info.sVMonthName + " có " + info.dMonthLenght + " ngày (tháng đủ)"
                : "- " + info.sVMonthName + " có " + info.dMonthLenght + " ngày (tháng thiếu)";
            label1.Text += "\r\n";
            label1.Text += "- Thuộc tiết khí: " + info.sTietKhi + "\r\n";
            label1.Text += "- Ngày đầu tiết khí Dương lịch: " + info.dStartSolarDateTietKhi.ToString() + "\r\n";
            label1.Text += "- Ngày đầu tiết khí Âm lịch: " + info.ToLunarTietKhi();
        }
    }
}
