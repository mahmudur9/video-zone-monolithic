using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Utils
{
    public class DateProcess
    {
        public static string GetDate(DateTime date)
        {
            string modifiedDate;

            TimeSpan timeSpan = DateTime.Now - date;
            if (timeSpan.TotalSeconds < 60)
            {
                modifiedDate = $"{(int)timeSpan.TotalSeconds} seconds ago";
            }
            else if (timeSpan.TotalMinutes < 60)
            {
                modifiedDate = $"{(int)timeSpan.TotalMinutes} minutes ago";
            }
            else if (timeSpan.TotalHours < 24)
            {
                modifiedDate = $"{(int)timeSpan.TotalHours} hours ago";
            }
            else if (timeSpan.TotalDays < 360)
            {
                modifiedDate = $"{(int)timeSpan.TotalDays} days ago";
            }
            else
            {
                modifiedDate = $"{(int)timeSpan.TotalDays / 360} years ago";
            }

            return modifiedDate;
        }
    }
}
