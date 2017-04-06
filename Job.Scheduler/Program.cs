using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Job.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe_1a = new DateTime(2017, 1, 30, 9, 0, 0);
            var pe_1b = new DateTime(2017, 3, 27, 9, 0, 0);
            var pe_1c = DateTime.MinValue; //new DateTime(2017, 3, 31, 9, 0, 0);
            var result_1a = SchedulerSet1(pe_1a, true, false, true, false, false, true, false, false, true, false, false, false, "Last", false, true, false, false, false, true, false, new TimeSpan(9, 0, 0), new DateTime(2017, 1, 1), null);
            var result_1b = SchedulerSet1(pe_1b, true, false, true, false, false, true, false, false, true, false, false, false, "Last", false, true, false, false, false, true, false, new TimeSpan(9, 0, 0), new DateTime(2017, 1, 1), null);
            var result_1c = SchedulerSet1(pe_1c, true, false, true, false, false, true, false, false, true, false, false, false, "Last", false, true, false, false, false, true, false, new TimeSpan(9, 0, 0), new DateTime(2017, 1, 1), null);
            Console.WriteLine($"1a. Previous Execution: {pe_1a} || Next Execution {result_1a}");
            Console.WriteLine($"1b. Previous Execution: {pe_1b} || Next Execution {result_1b}");
            Console.WriteLine($"1c. Previous Execution: {pe_1c} || Next Execution {result_1c}");

            var pe_2a = new DateTime(2017, 1, 28, 9, 0, 0);
            var pe_2b = new DateTime(2017, 3, 1, 9, 0, 0);
            var pe_2c = DateTime.MinValue;
            var result_2a = SchedulerSet2(pe_2a, true, false, true, false, false, true, false, false, true, false, false, false, "1,7,14,21,28", new TimeSpan(9, 0, 0), new DateTime(2017, 1, 1), null);
            var result_2b = SchedulerSet2(pe_2b, true, false, true, false, false, true, false, false, true, false, false, false, "1,7,14,21,28", new TimeSpan(9, 0, 0), new DateTime(2017, 1, 1), null);
            var result_2c = SchedulerSet2(pe_2c, true, false, true, false, false, true, false, false, true, false, false, false, "1,7,14,21,28", new TimeSpan(9, 0, 0), new DateTime(2017, 1, 1), null);
            Console.WriteLine($"2a. Previous Execution: {pe_2a} || Next Execution {result_2a}");
            Console.WriteLine($"2b. Previous Execution: {pe_2b} || Next Execution {result_2b}");
            Console.WriteLine($"2c. Previous Execution: {pe_2c} || Next Execution {result_2c}");

            Console.Write("Press the Any key to exit");
            Console.ReadKey();
        }
        
        // Returns a DateTime object representing the next Execution date. 
        // Returns an empty Datetime if there are no more execuation dates.
        static DateTime SchedulerSet1(
            DateTime previousExecution,
            bool jan, bool feb, bool mar, bool apr, bool may, bool jun, bool jul, bool aug, bool sep, bool oct, bool nov,
            bool dec,
            string weekOfMonth, //possible values (“1st”, “2nd", “3rd", “4th", “Last”)
            bool sun, bool mon, bool tue, bool wed, bool thu, bool fri, bool sat,
            TimeSpan startTime,
            DateTime startDate,
            DateTime? stopDate)
        {
            if (stopDate != null && stopDate < DateTime.UtcNow) return DateTime.MinValue; // No more scheduled runs.
            
            var months = new bool[12] { jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec };
            var daysOfWeek = new bool[7] { sun, mon, tue, wed, thu, fri, sat };
            var searchStartDate = (previousExecution == DateTime.MinValue) ? startDate : previousExecution.AddDays(1); //Possibly use DateTime.UtcNow

            // End of Range of 366 * 8 to cover the edge case of when it is only scheduled to run on leap days
            // and it needs to run over the instance of their being no leap year for 8 years (Centuries not divisible by 400)
            // In most other cases it should find a result within the first 364 days.
            var searchRangeEnd = (stopDate == null) ? 366*8 : Math.Min(((DateTime)stopDate - searchStartDate).TotalDays,366 * 8);

            for (var i = 0; i <= searchRangeEnd; i++)
            {
                var checkDate = searchStartDate.AddDays(i);

                if (!months[checkDate.Month - 1]) continue;

                var isValidWeek = false;
                if (weekOfMonth == "Last")
                {
                    var dom = DateTime.DaysInMonth(checkDate.Year, checkDate.Month);
                    if (checkDate.Day > dom - 7 && checkDate.Day <= dom) isValidWeek = true;
                }
                else
                {
                    var w = int.Parse(weekOfMonth.Substring(0, 1));
                    if (checkDate.Day > (w - 1)*7 && checkDate.Day <= w*7) isValidWeek = true;
                }
                if (!isValidWeek) continue;

                if (daysOfWeek[(int) checkDate.DayOfWeek])
                {
                    return new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, startTime.Hours, startTime.Minutes, startTime.Seconds, startTime.Milliseconds);
                }
            }
            return DateTime.MinValue;
        }

        static DateTime SchedulerSet2(
            DateTime previousExecution,
            bool jan, bool feb, bool mar, bool apr, bool may, bool jun, bool jul, bool aug, bool sep, bool oct, bool nov, bool dec,
            string days,
            TimeSpan startTime,
            DateTime startDate,
            DateTime? stopDate)
        {
            if (stopDate != null && stopDate < DateTime.UtcNow) return DateTime.MinValue; // No more scheduled runs.

            var months = new bool[12] { jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec };
            var setDays = days.Split(',').Select(Int32.Parse).ToList();
            var searchStartDate = (previousExecution == DateTime.MinValue) ? startDate : previousExecution.AddDays(1); //Possibly use DateTime.

            // End of Range of 366 * 8 to cover the edge case of when it is only scheduled to run on leap days
            // and it needs to run over the instance of their being no leap year for 8 years (Centuries not divisible by 400)
            // In most other cases it should find a result within the first 364 days.
            var searchRangeEnd = (stopDate == null) ? 366 * 8 : Math.Min(((DateTime)stopDate - searchStartDate).TotalDays, 366 * 8);

            for (var i = 0; i <= searchRangeEnd; i++)
            {
                var checkDate = searchStartDate.AddDays(i);

                if (!months[checkDate.Month - 1]) continue;
                
                if (setDays.Any(d => d == checkDate.Day))
                {
                    return new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, startTime.Hours, startTime.Minutes, startTime.Seconds, startTime.Milliseconds);
                }
            }
            return DateTime.MinValue;
        }
    }
}
