using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.Controllers
{
	public static class ExtensionMethods
	{
		public static bool HasValue(this Guid guid)
		{
			return guid != Guid.Empty;
		}

		public static string AddBackSlash(this string path)
		{
			return (!path.Trim().EndsWith(@"\")) ? path + @"\" : path;
		}

		public static DateTime? ToDateTime(this string s)
		{
			DateTime result;
			if (DateTime.TryParse(s, out result))
				return result;
			return null;
		}

		public static DateTime? ToDate(this string s)
		{
			DateTime result;
			if (DateTime.TryParse(s, out result))
				return result.Date;
			return null;
		}

		public static int? ToInt(this string s)
		{
			int result;
			if (int.TryParse(s, out result))
				return result;

			return null;
		}

		public static DateTime? NullOrDate(this DateTime? d)
		{
			if (d == null)
				return d;

			return (d.Value.Date);
		}

		private static System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

		public static string ToReportParameter(this DateTime d)
		{
			return string.Format(culture, "{0:dd MMM yyyy}", d);
		}

		public static string ToReportParameter(this DateTime? d)
		{
			if (d == null || !d.HasValue)
				return string.Empty;

			return string.Format(culture, "{0:dd MMM yyyy}", d);
		}

		public static bool IsNumeric(this string value)
		{
			if (String.IsNullOrEmpty(value))
				return false;

			return !value.ToCharArray().Where(x => !Char.IsDigit(x)).Any();
		}


		public static DateTime MonthEnd(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
		}

		public static DateTime FirstDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}


	}
}