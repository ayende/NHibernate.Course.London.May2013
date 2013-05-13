using NHibernate.Course.London.May2013.Models;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class ProductController : NHibernateController
	{
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