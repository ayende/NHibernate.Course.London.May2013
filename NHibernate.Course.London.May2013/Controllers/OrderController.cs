
using System;
using System.Linq;
using NHibernate.Course.London.May2013.Models;
using NHibernate.Criterion;
using NHibernate.Linq;
using Order = NHibernate.Course.London.May2013.Models.Order;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class OrderController : NHibernateController
	{
		public object Load(int id)
		{
			var order = Session.QueryOver<Order>()
							   .Where(x => x.Id == id)
							   .Fetch(x => x.Customer).Eager
							   .SingleOrDefault();
			return Json(order);
		}

		public object Search(string name)
		{
			var customersNamed = QueryOver.Of<Customer>()
			                              .Where(x => x.FullName == name)
			                              .Select(x => x.Id);
			var y = Session.QueryOver<Order>()
					.Fetch(x=>x.Customer).Eager
					.WithSubquery.WhereProperty(x => x.Customer).In(customersNamed)
				   .List();
			return Json(y.Count);
		}

		public object Create(int custId)
		{
			var cust = Session.Load<Customer>(custId);
			var order = new Order
				{
					CreatedAt = DateTime.Now,
					Customer = cust,
					Total = 5
				};
			var id = Session.Save(order);

			for (int i = 0; i < 5; i++)
			{
				var orderLine = new OrderLine
					{
						Product = Session.Load<Product>(65536),
						Order = order,
						Quantity = i + 1
					};
				order.OrderLines.Add(orderLine);
				Session.Save(orderLine);
			}
			return Json(id);
		}
	}
}