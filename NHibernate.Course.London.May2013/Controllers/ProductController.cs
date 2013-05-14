using System.Linq;
using NHibernate.Course.London.May2013.Models;
using NHibernate.Linq;

namespace NHibernate.Course.London.May2013.Controllers
{
	public static class QueryExtensions
	{
		public static IQueryable<Product> WhereUserIsActive(this IQueryable<Product> q)
		{
			return q.Where(x => x.Attributes["User"] == "IsActive");
		}
	}

	public class ProductController : NHibernateController
	{
		public object CreateCat(string name)
		{
			var id = Session.Save(new Category { Name = name });
			return Json(id);
		}

		public object AddProdToCat(int pid, int catId)
		{
			var category = Session.Get<Category>(catId);
			var prod = Session.Get<Product>(pid);

			prod.Categories.Add(category);
			category.Products.Add(prod);

			return Json("done");

		}

		public object Load(int id)
		{
			var x = Session.Get<Product>(id);
			return Json(x.Attributes);
		}

		public object QueryByProp(string prop, string val)
		{
			var q = Session.Query<Product>()
						   .FetchMany(x => x.Attributes)
						   .WhereUserIsActive()
						   .ToList();
			return Json(q.Select(x=>x.Attributes));
		}

		public object Save(string name, string sku, int count)
		{
			Session.Save(new Product
				{
					Name = name,
					Sku = sku,
					Attributes =
						{
							{"Volume", "59"},
							{"MegaPixels", "3.5Px"}
						}
				});
			return Json("ok");
		}
	}
}