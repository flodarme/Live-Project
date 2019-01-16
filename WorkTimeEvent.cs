using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScheduleIt2.Models
{
    public class WorkTimeEvent : Event
    {
        [Display(Name = "Clock Function Status")]
        public ClockFunctionStatus? ClockFunctionStatus { get; set; }
    }

    public enum ClockFunctionStatus
    {
        ClockInSuccess,
        ClockInFail,
        ClockOutSuccess,
        ClockOutFail,
        ClockInUpdated,
        ClockOutUpdated
    }
}