using System;
using System.Collections.Generic;

namespace WriteErace.Models;

public partial class Product
{
    public string Article { get; set; } = null!;

    public string? NameProducts { get; set; }

    public string? Unit { get; set; }

    public int? Cost { get; set; }

    public int? MaxDiscount { get; set; }

    public int IdManufacturer { get; set; }

    public int IdSupplier { get; set; }

    public int IdCategory { get; set; }

    public decimal? CurrentDiscount { get; set; }

    public int? Count { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual Manufacturer IdManufacturerNavigation { get; set; } = null!;

    public virtual Supplier IdSupplierNavigation { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
