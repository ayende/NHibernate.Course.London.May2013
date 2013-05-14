using NHibernate.Course.London.May2013.Models;
using NHibernate.Linq;
using System.Linq;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class CustomerController : NHibernateController
	{
		public object Get(int id)
		{
			var cust = Session.Get<Customer>(id);
			return Json(new
				{
					cust.FullName,
					cust.Version,
					cust.Addresses.Count
				});
		}

		public object Update(int id, string name, int version)
		{
			var cust = Session.Get<Customer>(id);
			cust.FullName = name;
			cust.Version = version;
			Session.Evict(cust);
			Session.Update(cust);
			return Json("Updated");
		}

		public object Query(int start = 0)
		{
			var cust = Session.Query<Customer>()
				   .Take(2)
				   .Skip(start)
				   .ToFuture();

			var total = Session.Query<Customer>().ToFutureValue(x => x.Count());

			return Json(new
				{
					Customers = cust.Select(x => x.FullName),
					Total = total.Value
				});
		}

		public object BetterGet(int id)
		{
			var cust = Session.Query<Customer>()
							  .Fetch(x => x.Animal)
							  .Where(x => x.Id == id)
							  .ToFuture();

			Session.Query<Customer>()
				   .FetchMany(x => x.Addresses)
				   .Where(x => x.Id == id)
				   .ToFuture();

			Session.Query<Customer>()
							  .FetchMany(x => x.EmergencyContactNumbers)
							  .Where(x => x.Id == id)
							  .ToFuture();

			return Json(cust.ToArray());
		}

		public object New(string name)
		{
			var germanShepherd = new GermanShepherd { Name = name + "s dog" };
			Session.Save(germanShepherd);
			var id = Session.Save(new Customer
				{
					FullName = name,
					Addresses =
						{
							{"Home", new Address{City = "Hadera", State = "Israel"}},
							{"Work", new Address{City = "London", State = "England"}},
						},
					EmergencyContactNumbers =
						{
							"01234",
							"48523",
							"82439"
						},
					Animal = germanShepherd
				});
			return Json(new { Id = id });
		}
	}
}