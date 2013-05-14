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

		public virtual decimal Amount { get; set; }
		public virtual string Currency { get; set; }
	}

	public class OrderLineMap : ClassMapping<OrderLine>
	{
		public OrderLineMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));
			Property(x => x.Quantity, mapper =>
				{
					mapper.NotNullable(true);
					mapper.Index("foo");
				});
			Cache(x=>x.Usage(CacheUsage.NonstrictReadWrite));

			ManyToOne(x => x.Order, m => m.Column("OrderId"));
			ManyToOne(x => x.Product, mapper =>
				{
					mapper.Index("foo");
				});

			Join("OrderLinesCharges", mapper =>
				{
					mapper.Key(keyMapper => keyMapper.Column("OrderLineId"));


					mapper.Property(x => x.Amount);
					mapper.Property(x => x.Currency);
				});
		}
	}
}