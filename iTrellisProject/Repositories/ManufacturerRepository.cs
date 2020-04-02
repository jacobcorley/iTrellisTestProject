using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using iTrellisProject.Models;

namespace iTrellisProject.Repositories
{
    /**
     * If this data were coming from a database, a repository class wouldn't be necessary
     * However, since this is coming from a file, I'd like to keep the file-parsing logic separate from the model itself
     */
    public static class ManufacturerRepository
    {
        public static List<Manufacturer> manufacturers;

        public static void hydrate()
        {
            if (manufacturers == null)
            {
                var manufacturerFilename = AppDomain.CurrentDomain.BaseDirectory + "Manufacturers.json";
                var bytes = File.ReadAllText(manufacturerFilename, Encoding.Default);
                manufacturers = JsonSerializer.Deserialize<List<Manufacturer>>(bytes);
            }
        }

        public static Manufacturer find(int id)
        {
            try
            {
                return manufacturers.First(m => m.manufacturerId == id);
            }
            catch (InvalidOperationException e)
            {
                throw new Exception($"No manufacturer found for manufacturerId {id} -- {e.Message}");
            }
        }
    }
}