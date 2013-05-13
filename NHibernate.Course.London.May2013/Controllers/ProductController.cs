using NHibernate.Course.London.May2013.Models;

namespace NHibernate.Course.London.May2013.Controllers
{
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

		public object Save(string name, string sku, int count)
		{
			for (int i = 0; i < count; i++)
			{
				Session.Save(new Product
				{
					Name = name,
					Sku = sku
				});
			}
			return Json("ok");
		}
	}
}