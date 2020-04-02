using System;
using System.Linq;
using System.Text.Json;
using iTrellisProject.Repositories;

namespace iTrellisProject.Models
{
    /**
     * Domain model for products
     */
    public class Product
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int manufacturerId { get; set; }
        public int inventoryQuantity { get; set; }
        public bool shipOnWeekends { get; set; }
        public int maxBusinessDaysToShip { get; set; }

        public Manufacturer manufacturer
        {
            get { return ManufacturerRepository.find(manufacturerId); }
        }

        /**
         * Return the date that a package should arrive, if it ships on shippingDate
         */
        public DateTime getArrivalDate(DateTime shippingDate)
        {
            int remainingShippingDays = maxBusinessDaysToShip - 1;
            bool valid = validShippingDate(shippingDate);
            while (remainingShippingDays > 0 || !valid)
            {
                shippingDate = shippingDate.AddDays(1);
                valid = validShippingDate(shippingDate);
                
                if (valid)
                {
                    remainingShippingDays--;
                }
            }

            return shippingDate;
        }
        
        /**
         * Determine whether a provided day is a no-shipping holiday
         */
        public bool validShippingDate(DateTime shippingDate)
        {
            if (!shipOnWeekends && (shippingDate.DayOfWeek == DayOfWeek.Saturday ||
                                    shippingDate.DayOfWeek == DayOfWeek.Sunday)) return false;

            if (HolidayRepository.all()
                .Any(holiday => holiday.month == shippingDate.Month && holiday.day == shippingDate.Day)) return false;

            if (manufacturer.holidays.Any(holiday =>
                holiday.month == shippingDate.Month && holiday.day == shippingDate.Day)) return false;
            
            return true;
        }
    }
    
    /**
     * View model for products (keeping these in the same file for the sake of simplicity)
     */
    [Serializable]
    public class ProductViewModel
    {
        public int productId { get; set; }
        public string productLink { get; set; }
        public string productName { get; set; }
        public string manufacturerName { get; set; }
        public int inventoryQuantity { get; set; }
        public string shipOnWeekends { get; set; }
        public string shippingHolidays { get; set; }
        public int maxBusinessDaysToShip { get; set; }
        public string estimatedArrival { get; set; }

        public ProductViewModel() {}
        
        public ProductViewModel(Product product)
        {
            productId = product.productId;
            productLink = $"/products/{productId}";
            productName = product.productName;
            manufacturerName = product.manufacturer.name;
            inventoryQuantity = product.inventoryQuantity;
            shipOnWeekends = product.shipOnWeekends ? "Yes" : "No";
            shippingHolidays = JsonSerializer.Serialize(product.manufacturer.holidays.Concat(HolidayRepository.all()).ToList());
            maxBusinessDaysToShip = product.maxBusinessDaysToShip;
            estimatedArrival = product.getArrivalDate(DateTime.Now).ToLongDateString();
        }
    }
}