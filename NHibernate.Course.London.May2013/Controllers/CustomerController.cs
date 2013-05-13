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
			var id = Session.Save(new Customer
				{
					FullName = name,
					Addresses =
						{
							{"Home", new Address{City = "Hadera", State = "Israel"}},
							{"Work", new Address{City = "London", State = "England"}},
						}
				});
			return Json(new { Id = id });
		}
	}
}