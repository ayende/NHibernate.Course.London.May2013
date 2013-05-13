using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Course.London.May2013.Models
{
	public class Category
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ICollection<Product> Products { get; set; }

		public Category()
		{
			Products = new List<Product>();
		}
	}

	public class CategoryMap : ClassMapping<Category>
	{
		public CategoryMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));
			Property(x => x.Name);

			Bag(x => x.Products, mapper =>
				{
					mapper.Table("ProductCategories");
					mapper.Key(key => key.Column("CategoryId"));
					mapper.Inverse(true);
				},
				relation =>
				{
					relation.ManyToMany(mapper => mapper.Column("ProductId"));
				});
		}
	}
}