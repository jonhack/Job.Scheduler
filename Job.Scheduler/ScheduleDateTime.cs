using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Scheduler
{
    public class ScheduleDateTime
    {
        public DateTime PreviousExecution { get; set; } 
        public bool jan { get; set; }
        public bool feb { get; set; }
        public bool mar { get; set; }
        public bool apr { get; set; }
        public bool may { get; set; }
        public bool jun { get; set; }
        public bool jul { get; set; }
        public bool aug { get; set; }
        public bool sep { get; set; }
        public bool oct { get; set; }
        public bool nov { get; set; }
        public bool dec { get; set; }

    }

    public class ScheduleDateTimeByDayOfWeek : ScheduleDateTime
    {
        string weekOfMonth { get; set; } //possible values (“1st” { get; set; } “2nd" { get; set; } “3rd" { get; set; } “4th" { get; set; } “Last”)
        public bool sun { get; set; }
        public bool mon { get; set; }
        public bool tue { get; set; }
        public bool wed { get; set; }
        public bool thu { get; set; }
        public bool fri { get; set; }
        public bool sat { get; set; }
        public DateTime startTime { get; set; }
        public DateTime startDate { get; set; }
        public DateTime stopDate { get; set; }
    }
}
