using System;
using System.Collections.Generic;
namespace Lunar
{
	public class clsLunar
	{
		private const double Pi = 3.14159265358979;
		private List<string> ThienCan = new List<string> { "Giáp", "Ất", "Bính", "Đinh", "Mậu", "Kỷ", "Canh", "Tân", "Nhâm", "Quý" };
		private List<string> DiaChi = new List<string> { "Tý", "Sửu", "Dần", "Mão", "Thìn", "Tỵ", "Ngọ", "Mùi", "Thân", "Dậu", "Tuất", "Hợi" };
		private List<string> VDayNameOfWeek = new List<string> { "Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy" };
		private List<string> TietKhi = new List<string> { "Xuân phân", "Thanh Minh", "Cốc vũ", "Lập hạ", "Tiểu mãn", "Mang chủng", "Hạ chí", "Tiểu thử", "Đại thử",
			"Lập thu", "Xử thử", "Bạch lộ", "Thu phân", "Hàn lộ", "Sương giáng", "Lập đông", "Tiểu tuyết", "Đại tuyết",
			"Đông chí", "Tiểu hàn", "Đại hàn", "Lập xuân", "Vũ thủy", "Kình trập" };

		public clsLunar()
		{

		}

		public struct LunarInfo
		{
			public string sDayNameOfWeek;//' Tên ngày trong tuần
			public DoubleDateTime dLunarDate;// ' Ngày, tháng, năm Âm lịch
			public double dDayOfLeap; //' Thuộc tháng nhuận = 1
			public double dLeap; //' Tháng nhuận = 0 là không có
			public double dLeapMonth;//' Nếu có tháng nhuận, trả về tháng đó
			public DoubleDateTime dLeap2SolarFrom;//' Tương ứng với ngày dương lịch: từ
			public DoubleDateTime dLeap2SolarTo;//' Tương ứng với ngày dương lịch: đến
			public double dMonthLenght;//' Số ngày trong tháng, nếu > 29 thì là tháng đủ
			public string sVMonthName;// ' Tên tháng gọi theo âm lịch
			public string sDayCanChi;
			public string sMonthCanChi;
			public string sYearCanChi;
			public string sHourCanChi;
			public string sTietKhi;
			public DoubleDateTime dStartSolarDateTietKhi;//' Ngày đầu tiết khí theo Dương lịch
			public DoubleDateTime dStartLunarDateTietKhi;// ' Ngày đầu tiết khí theo Âm lịch

			public string ToLunarDate()
			{
				//Định dạng ngày tháng theo âm lịch. Ví dụ: 20/11 năm Canh Dần
			    string result = "";
                if((int)dLunarDate.dDay == 15)
                    result = "Ngày Rằm-Tháng " + dLunarDate.dMonth + " năm " + sYearCanChi;
                else if ((int)dLunarDate.dDay == 1)
                    result = "Mùng 1-Tháng " + dLunarDate.dMonth + " năm " + sYearCanChi;
                else
                {
                    result = "Ngày " + dLunarDate.dDay + "-Tháng " + dLunarDate.dMonth + " năm " + sYearCanChi;
                }
			    return result;
			}

			public string ToLunarTietKhi()
			{
				//Định dạng ngày tháng theo âm lịch. Ví dụ: 20/11 năm Canh Dần
				return dStartLunarDateTietKhi.dDay + "/" + dStartLunarDateTietKhi.dMonth + " năm " + sYearCanChi;
			}
		}

		public struct DoubleDateTime
		{
			public double dDay;
			public double dMonth;
			public double dYear;

			public DoubleDateTime(double day = 0, double month = 0, double year = 0)
			{
				dDay = day;
				dYear = year;
				dMonth = month;
			}

		    public override string ToString()
		    {
		        return dDay + "/" + dMonth + "/" + dYear;
		    }
        }

		private double JuliusDays(double dDay, double dMonth, double dYear)
		{
			double dD, dM, dY;
			double result;

			try
			{
				dD = convertToInt((14 - dMonth) / 12);
				dY = dYear + 4800 - dD;
				dM = dMonth + 12 * dD - 3;
				result = dDay + convertToInt((153 * dM + 2) / 5) + 365 * dY + convertToInt(dY / 4) - convertToInt(dY / 100) + convertToInt(dY / 400) - 32045;
				if (result < 2299161)
				{
					result = dDay + convertToInt((153 * dM + 2) / 5) + 365 * dY + convertToInt(dY / 4) - 32083;
				}
				return result;
			}
			catch (Exception ex)
			{
				return -1;
			}
		}

		private DoubleDateTime JuliusDays2Date(double JDNumber)
		{
			double dA, dB, dC, dD;
			double dE, dM;
			try
			{
				double day, month, year;
				if (JDNumber < 2299161)
				{
					dA = convertToInt(JDNumber);
				}
				else {
					dM = convertToInt((JDNumber - 1867216.25) / 36524.25);
					dA = convertToInt(JDNumber) + 1 + dM - convertToInt(dM / 4);
				}
				dB = dA + 1524;
				dC = convertToInt((dB - 122.1) / 365.25);
				dD = convertToInt(365.25 * dC);
				dE = convertToInt((dB - dD) / 30.6001);
				day = convertToInt(dB - dD - convertToInt(30.6001 * dE));
				if (dE < 14)
				{
					month = dE - 1;
				}
				else {
					month = dE - 13;
				}
				if (month < 3)
				{
					year = dC - 4715;
				}
				else
				{
					year = dC - 4716;
				}
				return new DoubleDateTime(day, month, year);
			}
			catch (Exception ex)
			{
				return new DoubleDateTime(-1, -1, -1);
			}
		}

		private double GetNewMoonDay(double NMPos, double dTimeZone)
		{
			//'Return Julius Day at New Moon position from 01/01/1900
			double dT1, dT2, dT3;
			double dDr, dJD1, dM;
			double dMpr, dF, dC1;
			double dDelta, dJDNew;
			dT1 = NMPos / 1236.85;
			dT2 = dT1 * dT1;
			dT3 = dT2 * dT1;
			dDr = Pi / 180;
			dJD1 = 2415020.75933 + 29.53058868 * NMPos + 0.0001178 * dT2 - 0.000000155 * dT3;
			dJD1 = dJD1 + 0.00033 * Math.Sin((166.56 + 132.87 * dT1 - 0.009173 * dT2) * dDr);
			dM = 359.2242 + 29.10535608 * NMPos - 0.0000333 * dT2 - 0.00000347 * dT3;
			dMpr = 306.0253 + 385.81691806 * NMPos + 0.0107306 * dT2 + 0.00001236 * dT3;
			dF = 21.2964 + 390.67050646 * NMPos - 0.0016528 * dT2 - 0.00000239 * dT3;
			dC1 = (0.1734 - 0.000393 * dT1) * Math.Sin(dM * dDr) + 0.0021 * Math.Sin(2 * dDr * dM);
			dC1 = dC1 - 0.4068 * Math.Sin(dMpr * dDr) + 0.0161 * Math.Sin(dDr * 2 * dMpr);
			dC1 = dC1 - 0.0004 * Math.Sin(dDr * 3 * dMpr);
			dC1 = dC1 + 0.0104 * Math.Sin(dDr * 2 * dF) - 0.0051 * Math.Sin(dDr * (dM + dMpr));
			dC1 = dC1 - 0.0074 * Math.Sin(dDr * (dM - dMpr)) + 0.0004 * Math.Sin(dDr * (2 * dF + dM));
			dC1 = dC1 - 0.0004 * Math.Sin(dDr * (2 * dF - dM)) - 0.0006 * Math.Sin(dDr * (2 * dF + dMpr));
			dC1 = dC1 + 0.001 * Math.Sin(dDr * (2 * dF - dMpr)) + 0.0005 * Math.Sin(dDr * (2 * dMpr + dM));
			if (dT1 < -11)
			{
				dDelta = 0.001 + 0.000839 * dT1 + 0.0002261 * dT2 - 0.00000845 * dT3 - 0.000000081 * dT1 * dT3;
			}
			else {
				dDelta = -0.000278 + 0.000265 * dT1 + 0.000262 * dT2;
			}
			dJDNew = dJD1 + dC1 - dDelta;
			return convertToInt(dJDNew + 0.5 + dTimeZone / 24);
		}

		private double SunLongitude(double dJD, double dTimeZone)
		{
			double dT1, dT2, dDr;
			double dM, dL0, dDL;
			double dL;
			dT1 = (dJD - 2451545.5 - dTimeZone / 24) / 36525;
			dT2 = dT1 * dT1;
			dDr = Pi / 180;
			dM = 357.5291 + 35999.0503 * dT1 - 0.0001559 * dT2 - 0.00000048 * dT1 * dT2;
			dL0 = 280.46645 + 36000.76983 * dT1 + 0.0003032 * dT2;
			dDL = (1.9146 - 0.004817 * dT1 - 0.000014 * dT2) * Math.Sin(dDr * dM);
			dDL = dDL + (0.019993 - 0.000101 * dT1) * Math.Sin(dDr * 2 * dM) + 0.00029 * Math.Sin(dDr * 3 * dM);
			dL = dL0 + dDL;
			dL = dL * dDr;
			dL = dL - Pi * 2 * convertToInt(dL / (Pi * 2));
			return convertToInt(dL / Pi * 6);
		}

		private double SunLongitude2(double dJD)
		{
			double dT1, dT2, dDr;
			double dM, dL0, dDL;
			double dL;
			dT1 = (dJD - 2451545) / 36525;
			dT2 = dT1 * dT1;
			dDr = Pi / 180;
			dM = 357.5291 + 35999.0503 * dT1 - 0.0001559 * dT2 - 0.00000048 * dT1 * dT2;
			dL0 = 280.46645 + 36000.76983 * dT1 + 0.0003032 * dT2;
			dDL = (1.9146 - 0.004817 * dT1 - 0.000014 * dT2) * Math.Sin(dDr * dM);
			dDL = dDL + (0.019993 - 0.000101 * dT1) * Math.Sin(dDr * 2 * dM) + 0.00029 * Math.Sin(dDr * 3 * dM);
			dL = dL0 + dDL;
			dL = dL * dDr;
		    dL = dL - Pi*2 * convertToInt(dL/(Pi*2));
            /*if(dL / (Pi * 2) > 0 && dL / (Pi * 2) != (int)(dL / (Pi * 2)))
		    {
		        dL = dL - Pi*2*(int) (dL/(Pi*2));
		    } else if (dL / (Pi * 2) < 0 && dL / (Pi * 2) != (int)(dL / (Pi * 2)))
		    {
                dL = dL - Pi * 2 * ((int)(dL / (Pi * 2)) - 1);
            }
		    else
		    {
                dL = dL - Pi * 2 * (int)(dL / (Pi * 2));
            }*/
            return dL;
        }

		private double GetSunLongitude(double dJD, double dTimeZone)
		{
			return convertToInt(SunLongitude2(dJD - 0.5 - dTimeZone / 24) / Pi * 12);
		}

		private double GetLunarMonth11th(double dYear, double dTimeZone)
		{
			double dK, dOff, sunLong;
			double NM;
			dOff = JuliusDays(31, 12, dYear) - 2415021;
			dK = convertToInt(dOff / 29.530588853);
			NM = GetNewMoonDay(dK, dTimeZone);
			sunLong = SunLongitude(NM, dTimeZone);
			if (sunLong >= 9)
			{
				NM = GetNewMoonDay(dK - 1, dTimeZone);
			}
			return NM;
		}

		private double GetLeapMonthOffset(double dMonth11th, double dTimeZone)
		{
			double dK, dLast, dArc;
			int i;
			dK = convertToInt((dMonth11th - 2415021.07699869) / 29.530588853 + 0.5);
			dLast = 0;
			i = 1;
			dArc = SunLongitude(GetNewMoonDay(dK + i, dTimeZone), dTimeZone);
			do
			{
				dLast = dArc;
				i = i + 1;
				dArc = SunLongitude(GetNewMoonDay(dK + i, dTimeZone), dTimeZone);
			} while (dArc != dLast && i < 14);
			return i - 1;
		}

		public DoubleDateTime Solar2Lunar(double dDay, double dMonth, double dYear, double dTimeZone)
		{
			double dK, dDayNum, dMonthStart;
			double dF11, dS11, dLunarDay;
			double dLunarMonth, dLunarYear, dLunarLeap;
			double dDiff, dLeapMonthDiff;
			DoubleDateTime result = new DoubleDateTime();

			dDayNum = JuliusDays(dDay, dMonth, dYear);
			dK = convertToInt((dDayNum - 2415024.07699869) / 29.530588853);
			dMonthStart = GetNewMoonDay(dK + 1, dTimeZone);
			if (dMonthStart > dDayNum)
			{
				dMonthStart = GetNewMoonDay(dK, dTimeZone);
			}
			dF11 = GetLunarMonth11th(dYear, dTimeZone);
			dS11 = dF11;
			if (dF11 > dMonthStart)
			{
				dLunarYear = dYear;
				dF11 = GetLunarMonth11th(dYear - 1, dTimeZone);
			}
			else
			{
				dLunarYear = dYear + 1;
				dS11 = GetLunarMonth11th(dYear + 1, dTimeZone);
			}
			dLunarDay = dDayNum - dMonthStart + 1;
			dDiff = convertToInt((dMonthStart - dF11) / 29);
			dLunarLeap = 0;
			dLunarMonth = dDiff + 11;
			if ((dS11 - dF11) > 365)
			{
				dLeapMonthDiff = GetLeapMonthOffset(dF11, dTimeZone);
				if (dDiff >= dLeapMonthDiff)
				{
					dLunarMonth = dDiff + 10;
					if (dDiff == dLeapMonthDiff)
					{
						dLunarLeap = 1;
					}
				}
			}
			if (dLunarMonth > 12)
			{
				dLunarMonth = dLunarMonth - 12;
			}
			if (dLunarMonth >= 11 && dDiff < 4)
			{
				dLunarYear = dLunarYear - 1;
			}
			result = new DoubleDateTime(dLunarDay, dLunarMonth, dLunarYear);
			return result;
		}

		public DoubleDateTime Lunar2Solar(double dLunarDay, double dLunarMonth, double dLunarYear, double dLunarLeap, double dTimeZone)
		{
			double dK, dF11, dS11;
			double dOff, dLeapOff, dLeapMonth;
			double dMonthStart;
			DoubleDateTime result = new DoubleDateTime();
			if (dLunarMonth < 11)
			{
				dF11 = GetLunarMonth11th(dLunarYear - 1, dTimeZone);
				dS11 = GetLunarMonth11th(dLunarYear, dTimeZone);
			}
			else
			{
				dF11 = GetLunarMonth11th(dLunarYear, dTimeZone);
				dS11 = GetLunarMonth11th(dLunarYear + 1, dTimeZone);
			}
			dOff = dLunarMonth - 11;
			if (dOff < 0)
			{
				dOff = dOff + 12;
			}

			if ((dS11 - dF11) > 365)
			{
				dLeapOff = GetLeapMonthOffset(dF11, dTimeZone);
				dLeapMonth = dLeapOff - 2;
				if (dLeapMonth < 0)
				{
					dLeapMonth = dLeapMonth + 12;
				}
				if (dLunarLeap != 0 && dLunarMonth != dLeapMonth)
				{
					result = new DoubleDateTime(0, 0, 0);
				}
				else if (dLunarLeap != 0 || dLunarMonth != dLeapMonth)
				{
					dOff = dOff + 1;
				}
			}
			dK = convertToInt(0.5 + (dF11 - 2415021.07699869) / 29.530588853);
			dMonthStart = GetNewMoonDay(dK + dOff, dTimeZone);
			DoubleDateTime juliusDate = JuliusDays2Date(dMonthStart + dLunarDay - 1);
			result = new DoubleDateTime(juliusDate.dDay, juliusDate.dMonth, juliusDate.dYear);
			return result;
		}

		//'----------------------------------------------------------------------------------------
		//'* Test, Edit and Translate by vie87vn - www.caulacbovb.com
		//'----------------------------------------------------------------------------------------

		public LunarInfo GetAllLunarInfo(double dDay, double dMonth, double dYear, double dTimeZone)
		{
			int i;
			double dJD, dLLeap;
			double dLDay, dLMonth, dLYear;
			double dMonthLen;
			double dLM;
			int iPos, iCan, iChi;
			iPos = convertToInt((JuliusDays(dDay, dMonth, dYear) + 1) % 7);
			DoubleDateTime lunarDay = Solar2Lunar(dDay, dMonth, dYear, dTimeZone);
			dLDay = lunarDay.dDay;
			dLMonth = lunarDay.dMonth;
			dLYear = lunarDay.dYear;
			dLM = GetLeapMonthOffset(GetLunarMonth11th(dLYear, dTimeZone), dTimeZone);
			if (dLM > 2)
			{
				dLM = GetLeapMonthOffset(GetLunarMonth11th(dLYear - 1, dTimeZone), dTimeZone);
			}
			if (dLM < 13)
			{
				dLM = dLM - 2;
			}
			if (dLM <= 0)
			{
				dLM = dLM + 12;
			}
			dLLeap = dLM > 12 ? 0 : 1;
			double dSDay, dSMonth, dSYear;
			dSDay = Lunar2Solar(1, dLMonth, dLYear, dLLeap, dTimeZone).dDay;
			dSMonth = Lunar2Solar(1, dLMonth, dLYear, dLLeap, dTimeZone).dMonth;
			dSYear = Lunar2Solar(1, dLMonth, dLYear, dLLeap, dTimeZone).dYear;
			dJD = JuliusDays(dSDay, dSMonth, dSYear);
			i = 25;
			for (i = 25; i <= 31; i++)
			{
				if (Solar2Lunar(JuliusDays2Date(dJD + i).dDay, JuliusDays2Date(dJD + i).dMonth, JuliusDays2Date(dJD + i).dYear, dTimeZone).dDay == 1)
				{
					break;
				}
			}
			dMonthLen = i;
			LunarInfo result = new LunarInfo();
			result.sDayNameOfWeek = VDayNameOfWeek[iPos];
			result.dLunarDate = new DoubleDateTime(dLDay, dLMonth, dLYear);
			result.dLeap = dLLeap;
			result.dLeapMonth = dLM > 12 ? 0 : dLM;
			result.dMonthLenght = dMonthLen;
			iCan = convertToInt(JuliusDays(dDay, dMonth, dYear) + 9) % 10;
			iChi = convertToInt(JuliusDays(dDay, dMonth, dYear) + 1) % 12;
			result.sDayCanChi = ThienCan[iCan] + " " + DiaChi[iChi];
			iCan = (iCan * 2) % 10;
			iChi = 0;
			result.sHourCanChi = ThienCan[iCan] + " " + DiaChi[iChi];
			iCan = convertToInt((dLYear * 12 + dLMonth + 3) % 10);
			iChi = convertToInt(dLMonth - 11) < 0 ? convertToInt(dLMonth) + 1 : convertToInt(dLMonth) - 11;
			result.sMonthCanChi = ThienCan[iCan] + " " + DiaChi[iChi];
			iCan = convertToInt((dLYear + 6) % 10);
			iChi = convertToInt((dLYear + 8) % 12);
			result.sYearCanChi = ThienCan[iCan] + " " + DiaChi[iChi];
			result.dDayOfLeap = 0;
			if (dLLeap == 1)
			{
				result.dLeap2SolarFrom.dDay = Lunar2Solar(1, result.dLeapMonth, dLYear, dLLeap, dTimeZone).dDay;
				result.dLeap2SolarFrom.dMonth = Lunar2Solar(1, result.dLeapMonth, dLYear, dLLeap, dTimeZone).dMonth;
				result.dLeap2SolarFrom.dYear = Lunar2Solar(1, result.dLeapMonth, dLYear, dLLeap, dTimeZone).dYear;
				dJD = JuliusDays(result.dLeap2SolarFrom.dDay, result.dLeap2SolarFrom.dMonth, result.dLeap2SolarFrom.dYear);
				i = 25;
				for (i = 25; i <= 31; i++)
				{
					if (Solar2Lunar(JuliusDays2Date(dJD + i).dDay, JuliusDays2Date(dJD + i).dMonth, JuliusDays2Date(dJD + i).dYear, dTimeZone).dDay == 1)
					{
						break;
					}
				}
				dJD = dJD + i - 1;
				result.dLeap2SolarTo = new DoubleDateTime(JuliusDays2Date(dJD).dDay, JuliusDays2Date(dJD).dMonth, JuliusDays2Date(dJD).dYear);
				if (dMonth == result.dLeap2SolarFrom.dMonth)
				{
					if (dDay >= result.dLeap2SolarFrom.dDay)
					{
						result.dDayOfLeap = 1;
					}
					else {
						result.dDayOfLeap = 0;
					}
				}
				else if (dMonth == result.dLeap2SolarTo.dMonth)
				{
					if (dDay <= result.dLeap2SolarTo.dDay)
					{
						result.dDayOfLeap = 1;
					}
					else
					{
						result.dDayOfLeap = 0;
					}
				}
				else
				{
					result.dDayOfLeap = 0;
				}
			}
			else
			{
				result.dLeap2SolarFrom = new DoubleDateTime(0, 0, 0);
				result.dLeap2SolarTo = new DoubleDateTime(0, 0, 0);
			}

			iPos = convertToInt(GetSunLongitude(JuliusDays(dDay, dMonth, dYear) + 1, dTimeZone));
			result.sTietKhi = TietKhi[iPos];

			dJD = JuliusDays(dDay, dMonth, dYear) + 1;
			i = 0;
			for (i = 0; i <= 20; i++)
			{
				if (GetSunLongitude(dJD - i, dTimeZone) != iPos)
				{
					break;
				}
			}
			result.dStartSolarDateTietKhi = new DoubleDateTime(JuliusDays2Date(dJD - i).dDay, JuliusDays2Date(dJD - i).dMonth, JuliusDays2Date(dJD - i).dYear);
			DoubleDateTime lunarTemp = new DoubleDateTime();
			lunarTemp = Solar2Lunar(result.dStartSolarDateTietKhi.dDay, result.dStartSolarDateTietKhi.dMonth, result.dStartSolarDateTietKhi.dYear, dTimeZone);
			result.dStartLunarDateTietKhi = new DoubleDateTime(lunarTemp.dDay, lunarTemp.dMonth, lunarTemp.dYear);
			result.sVMonthName = VMonthName(dLMonth);
			return result;
		}


		private string VMonthName(double dMonth)
		{
			string vars = "giêng,hai,ba,tư,năm,sáu,bảy,tám,chín,mười,mười một,chạp";
			List<string> uu = new List<string>(vars.Split(','));
			return "Tháng " + uu[(int)dMonth - 1];
		}

	    private int convertToInt(double a)
	    {
	        if (a >= 0)
	        {
	            return (int) a;
	        }
	        else
	        {
	            return (int) (a - 1);
	        }
        }
	}
}

