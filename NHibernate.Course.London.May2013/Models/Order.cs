using System;
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
	}

	public class OrderMap : ClassMapping<Order>
	{
		public OrderMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));
			Property(x => x.Total);
			Property(x => x.CreatedAt, m => m.Update(false));

			ManyToOne(x => x.Customer);
		}
	}
}