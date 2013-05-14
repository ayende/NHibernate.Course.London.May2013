using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Course.London.May2013.Models
{
	public class Order
	{
		public virtual int Id { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual decimal Total { get; set; }
		public virtual DateTime CreatedAt { get; set; }
		public virtual ICollection<OrderLine> OrderLines { get; set; }
		public virtual Address ShippingAddress { get; set; }

		public virtual IDictionary Attributes { get; set; }

		public Order()
		{
			OrderLines = new List<OrderLine>();
			Attributes = new Hashtable();
		}
	}

	public class OrderMap : ClassMapping<Order>
	{
		public OrderMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));
			Property(x => x.Total);
			Property(x => x.CreatedAt, m => m.Update(false));
			Cache(x => x.Usage(CacheUsage.NonstrictReadWrite));
			ManyToOne(x => x.Customer);

			Component(x=>x.ShippingAddress, mapper =>
				{
					mapper.Property(x=>x.State);
					mapper.Property(x => x.City);
				});

			Component(order => order.Attributes, new
				{
					Active = false,
					Status = 0
				}, mapper =>
					{
						mapper.Property(x=>x.Active);
						mapper.Property(x=>x.Status);
					});

			Bag(x => x.OrderLines,
			    mapper =>
				    {
					    mapper.Key(key => key.Column("OrderId"));
					    mapper.Inverse(true);
						mapper.Cache(cacheMapper => cacheMapper.Usage(CacheUsage.NonstrictReadWrite));
				    },
			    relation => relation.OneToMany());
		}
	}
}