using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using iTrellisProject.Models;

namespace iTrellisProject.Repositories
{
    /**
     * If this data were coming from a database, a repository class wouldn't be necessary
     * However, since this is coming from a file, I'd like to keep the file-parsing logic separate from the model itself
     */
    public static class HolidayRepository
    {
        private static List<Holiday> _holidays;

        public static void hydrate()
        {
            if (_holidays == null)
            {
                var productFilename = AppDomain.CurrentDomain.BaseDirectory + "Holidays.json";
                var bytes = File.ReadAllText(productFilename, Encoding.Default);
                _holidays = JsonSerializer.Deserialize<List<Holiday>>(bytes);
            }
        }

        public static List<Holiday> all()
        {
            return _holidays;
        }
    }
}