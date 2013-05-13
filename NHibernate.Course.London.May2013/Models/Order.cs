using System;
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

		public Order()
		{
			OrderLines = new List<OrderLine>();
		}
	}

	public class OrderMap : ClassMapping<Order>
	{
		public OrderMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));
			Property(x => x.Total);
			Property(x => x.CreatedAt, m => m.Update(false));

			ManyToOne(x => x.Customer);

			Bag(x => x.OrderLines,
			    mapper =>
				    {
					    mapper.Key(key => key.Column("OrderId"));
					    mapper.Inverse(true);
				    },
			    relation => relation.OneToMany());
		}
	}
}