using System;
using System.Collections.Generic;
using System.Linq;

namespace iTrellisProject.Models
{
    /**
     * Domain model for manufacturers (no view model)
     */
    [Serializable]
    public class Manufacturer
    {
        public int manufacturerId { get; set; }
        public string name { get; set; }
        public List<Holiday> holidays { get; set; }
        
        /**
         * Determine whether the provided day is a holiday for this manufacturer
         * Holidays should be entered as "month/day", and should be entered via another route that can sanitize them
         * As it's not part of this project, I'm not including a UI to handle this, the data's fine as-is
         */
        public bool isHoliday(DateTime date)
        {
            return holidays.Any(holiday => holiday.month == date.Month && holiday.day == date.Day);
        }
    }
}