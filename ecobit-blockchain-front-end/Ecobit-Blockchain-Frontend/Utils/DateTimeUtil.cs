using System;

namespace Ecobit_Blockchain_Frontend.Utils
{
    public static class DateTimeUtil
    {
        public static DateTime ConvertToDate(int time)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds( time );
            return dtDateTime;
        }

        public static DateTime? ConvertToNullableDate(int time)
        {
            if (time == 0)
            {
                return null;
            }

            return ConvertToDate(time);
        }
        
        public static int ConvertToTimestamp(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return  (int) dateTime.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            }
            
            return 0;
        }
    }
}     