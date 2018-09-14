using System;

namespace Application.Util
{
    public static class DateTimeUtil
    {
        /// <summary>
        /// Converts a date to a timestamp
        /// </summary>
        /// <param name="dateTime">The date that needs to be converted</param>
        /// <returns>a timestamp</returns>
        public static int ConvertToTimestamp(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return  (Int32)(dateTime.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            }
            
            return 0;
        }

    }
}