using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Course.London.May2013.Models
{
	public class OrderLine
	{
		public virtual int Id { get; set; }
		public virtual int Quantity { get; set; }

		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }
	}

	public class OrderLineMap : ClassMapping<OrderLine>
	{
		public OrderLineMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));
			Property(x => x.Quantity);

			ManyToOne(x => x.Order, m => m.Column("OrderId"));
			ManyToOne(x => x.Product);
		}
	}
}