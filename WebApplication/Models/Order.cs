using System;
using System.ComponentModel.DataAnnotations;
using Internal.Database;
namespace Internal.Models
{
    public class OrderModel
    {
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select Customer Name.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please select Employee Name.")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Please select Order Date.")]
        public DateTime OrderDate { get; set; }

        public int OrderID { get; set; }

        public int OrderedQuantity { get; set; }

        [Required(ErrorMessage = "Please select Product Category.")]
        public string ProductCategory { get; set; }

        [Required(ErrorMessage = "Please select Product Name.")]
        public string ProductName { get; set; }

        public int ShipperID { get; set; }

        public string ShipperName { get; set; }

        public decimal TotalCost { get; set; }

    }
}
