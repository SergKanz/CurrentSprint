
namespace CurrentSprintClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class VSSprint
    {
        private static DateTime firstSprintStart = new DateTime(2010, 7, 25);
        private static int sprintLengthInWeeks = 3;
        private static int sprintLengthInDays = sprintLengthInWeeks * 7;

        public static int GetSprintForDate(DateTime today)
        {
            return Convert.ToInt32((today.Subtract(firstSprintStart)).TotalDays) / sprintLengthInDays;
        }

        public VSSprint()
            : this(DateTime.Now, GetSprintForDate(DateTime.Now))
        {
        }

        public VSSprint(DateTime now, int number)
        {
            DateTime today = now.Date;

            this.Number = number;
            DateTime sprintStart = firstSprintStart.AddDays(this.Number * sprintLengthInDays);
            DateTime calendarStart = sprintStart.AddDays(-7);
            this.StartWorkingDate = sprintStart.AddDays(1);
            this.EndWorkingDate = this.StartWorkingDate.AddDays(sprintLengthInDays - 3);
            this.WeekDates = new int[sprintLengthInWeeks + 2][];
            int shift = 0;
            for (int i = 0; i < sprintLengthInWeeks + 2; i++)
            {
                this.WeekDates[i] = new int[7];

                for (int j = 0; j < 7; j++)
                {
                    this.WeekDates[i][j] = calendarStart.AddDays(shift).Day;
                    ++shift;
                }
            }
            this.IsTodayIncluded = (today > calendarStart) && (today < calendarStart.AddDays(sprintLengthInDays + 2 * 7));
            if (this.IsTodayIncluded)
            {
                this.TodayWeek = Convert.ToInt32(today.Subtract(calendarStart).TotalDays) / 7 + 1;
                this.TodayDay = Convert.ToInt32(today.Subtract(calendarStart).TotalDays) % 7 + 1;
            }
            else
            {
                this.TodayWeek = 0;
                this.TodayDay = 0;
            }
        }

        public int Number { get; private set; }

        public int PreviousNum { get { return this.Number - 1; } }
        public int NextNum { get { return this.Number + 1; } }

        public DateTime StartWorkingDate { get; private set; }

        public DateTime EndWorkingDate { get; private set; }

        public int[][] WeekDates { get; private set; }

        public bool IsTodayIncluded { get; private set; }
        public int TodayWeek { get; private set; }
        public int TodayDay { get; private set; }
    }
}
