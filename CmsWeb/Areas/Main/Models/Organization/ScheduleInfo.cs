using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSPresenter;
using UtilityExtensions;

namespace CmsWeb.Models.OrganizationPage
{
    public class ScheduleInfo
    {
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        private string _Time;
        public string Time
        {
            get
            {
                if (!_Time.HasValue())
                    return "8:00 AM";
                return _Time;
            }
            set
            {
                _Time = value;
            }
        }
        public int AttendCreditId { get; set; }
        public SelectList DaysOfWeek()
        {
            return new SelectList(new[] {
                new { Text = "Sun", Value = "0" },
                new { Text = "Mon", Value = "1" },
                new { Text = "Tue", Value = "2" },
                new { Text = "Wed", Value = "3" },
                new { Text = "Thu", Value = "4" },
                new { Text = "Fri", Value = "5" },
                new { Text = "Sat", Value = "6" },
                new { Text = "Any", Value = "10" }
                }, "Value", "Text", DayOfWeek.ToString());
        }
        public SelectList AttendCredits()
        {
            return new SelectList(CodeValueController.AttendCredits(),
                "Id", "Value", AttendCreditId.ToString());
        }
        public string DisplayAttendCredit
        {
            get
            {
                return (from i in CodeValueController.AttendCredits()
                        where i.Id == AttendCreditId
                        select i.Value).Single();
            }
        }
        public string DisplayDay
        {
            get
            {
                return (from i in DaysOfWeek()
                        where i.Value == DayOfWeek.ToString()
                        select i.Text).Single();
            }
        }
        public string Display
        {
            get
            {
                return "{0}, {1}, {2}".Fmt(DisplayDay, Time, DisplayAttendCredit);
            }
        }
        public string Value
        {
            get
            {
                return NewMeetingTime.ToString("M/d/yy,h:mm tt,") + AttendCreditId;
            }
        }
        public DateTime NewMeetingTime
        {
            get
            {
                DateTime dt;
                if (DayOfWeek < 9)
                {
                    dt = Util.Now.Date;
                    dt = dt.AddDays(-(int)dt.DayOfWeek); // prev sunday
                    dt = dt.AddDays(DayOfWeek);
                    if (dt > Util.Now.Date)
                        dt = dt.AddDays(-7);
                }
                else
                    dt = Util.Now.Date;
                var tm = Time.ToDate().Value.TimeOfDay;
                dt = dt.Add(tm);
                return dt;
            }
        }
    }
}