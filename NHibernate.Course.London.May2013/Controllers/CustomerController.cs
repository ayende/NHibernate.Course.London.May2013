using NHibernate.Course.London.May2013.Models;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class CustomerController : NHibernateController
	{
		 public object Get(int id)
		 {
			 using (var session = SessionFactory.OpenSession())
			 {
				 var cust = session.Get<Customer>(id);
				 return Json(cust);
			 }
		 }
	}
}