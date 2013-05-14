using System.Collections.Generic;
using System.Collections.ObjectModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Course.London.May2013.Models
{
	public class Product
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Sku { get; set; }

		public virtual ICollection<Category> Categories { get; set; }
		public virtual IDictionary<string, string> Attributes { get; set; }

		public Product()
		{
			Categories = new Collection<Category>();
			Attributes =new Dictionary<string, string>();
		}
	}

	public class ProductMap : ClassMapping<Product>
	{
		public ProductMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));
			Property(x => x.Name);
			Property(x => x.Sku);

			Map(x=>x.Attributes, mapper =>
				{
					mapper.Table("ProductAttributes");
					mapper.Key(key => key.Column("ProductId"));
					mapper.Lazy(CollectionLazy.NoLazy);
					mapper.Fetch(CollectionFetchMode.Join);
				},relation =>
					{
						relation.Element(mapper => mapper.Column("Val"));
					});

			Set(x => x.Categories, mapper =>
				{
					mapper.Table("ProductCategories");
					mapper.Key(key => key.Column("ProductId"));
				},
				relation =>
				{
					relation.ManyToMany(mapper => mapper.Column("CategoryId"));
				});

		}
	}
}