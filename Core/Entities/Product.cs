using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Product : BaseClass
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        // using notations to indicate that the price can have 18 digits before point and 2 after point
        // it's important to mention that it indicates how the price will be stored in the Product table.
        // There is another way to do this, and we use it in the BussinesLogic(BussinesLogic/Data/Configuration) project
        // [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}
