using System;
using System.Globalization;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class HebrewCalenderManager
    {
        public string GetHebrewDateAsString(DateTime dateTime)
        {
            DateTime today = DateTime.Now;
            string dateAsString = GetHebrewJewishDateString(today, true);
            return dateAsString;
        }
        public static string GetHebrewJewishDateString(DateTime anyDate, bool addDayOfWeek)
        {
            System.Text.StringBuilder hebrewFormattedString = new System.Text.StringBuilder();

            // Create the hebrew culture to use hebrew (Jewish) calendar 
            CultureInfo jewishCulture = CultureInfo.CreateSpecificCulture("he-IL");
            jewishCulture.DateTimeFormat.Calendar = new HebrewCalendar();

            #region Format the date into a Jewish format 

            if (addDayOfWeek)
            {
                // Day of the week in the format " " 
                hebrewFormattedString.Append(anyDate.ToString("dddd", jewishCulture) + " ");
            }

            // Day of the month in the format "'" 
            hebrewFormattedString.Append(anyDate.ToString("dd", jewishCulture) + " ");

            // Month and year in the format " " 
            hebrewFormattedString.Append("" + anyDate.ToString("y", jewishCulture));
            #endregion

            return hebrewFormattedString.ToString();
        }
    }
}