
using NHibernate.Course.London.May2013.Models;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class OrderController : NHibernateController
	{
		 public object Load(int id)
		 {
			 return Json(Session.Get<Order>(id));
		 }
	}
}