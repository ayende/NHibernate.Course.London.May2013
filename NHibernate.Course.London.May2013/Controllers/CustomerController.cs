using NHibernate.Course.London.May2013.Models;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class CustomerController : NHibernateController
	{
		public object Get(int id)
		{
			var cust = Session.Get<Customer>(id);
			return Json(cust);
		}

		public object New(string name)
		{
			var id = Session.Save(new Customer { FullName = name });
			return Json(new { Id = id });
		}
	}
}