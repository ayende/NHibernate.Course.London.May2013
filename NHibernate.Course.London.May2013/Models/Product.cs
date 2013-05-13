using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Course.London.May2013.Models
{
	public class Product
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Sku { get; set; }
	}

	public class ProductMap : ClassMapping<Product>
	{
		public ProductMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));
			Property(x => x.Name);
			Property(x => x.Sku);
		}
	}
}