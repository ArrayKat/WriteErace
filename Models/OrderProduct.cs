using System;
using System.Collections.Generic;

namespace WriteErace.Models;

public partial class OrderProduct
{
    public int Id { get; set; }

    public int IdOrder { get; set; }

    public string ArticleProducts { get; set; } = null!;

    public int CountProducts { get; set; }

    public virtual Product ArticleProductsNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
