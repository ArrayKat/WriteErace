using System;
using System.Collections.Generic;

namespace WriteErace.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly? DateOrder { get; set; }

    public DateOnly? DateDelivery { get; set; }

    public int IdAddress { get; set; }

    public int? Code { get; set; }

    public int IdStatus { get; set; }

    public int? IdClient { get; set; }

    public virtual Address IdAddressNavigation { get; set; } = null!;

    public virtual User? IdClientNavigation { get; set; }

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
