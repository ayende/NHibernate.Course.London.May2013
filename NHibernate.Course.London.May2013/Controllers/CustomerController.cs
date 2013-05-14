using NHibernate.Course.London.May2013.Models;
using NHibernate.Linq;
using System.Linq;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class CustomerController : NHibernateController
	{
		public object Get(int id)
		{
			var cust = Session.Query<Customer>()
							  .FetchMany(x => x.Addresses)
							  .FetchMany(x => x.EmergencyContactNumbers)
							  .Fetch(x => x.Animal)
							  .SingleOrDefault(x => x.Id == id);
			return Json(cust);
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
							  .SingleOrDefault(x => x.Id == id);

			Session.Query<Customer>()
				   .FetchMany(x => x.Addresses)
				   .SingleOrDefault(x => x.Id == id);

			Session.Query<Customer>()
							  .FetchMany(x => x.EmergencyContactNumbers)
							  .SingleOrDefault(x => x.Id == id);

			return Json(cust);
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