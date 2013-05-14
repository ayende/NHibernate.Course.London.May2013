
using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Course.London.May2013.Models;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Type;
using Order = NHibernate.Course.London.May2013.Models.Order;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class Custom
	{
		private static bool init;
		public static void Init()
		{
			if (init)
				return;
			init = true;
			ExpressionProcessor.RegisterCustomMethodCall(() => Sql(null),
			                                             expression =>
				                                             {
					                                             var expr = (ConstantExpression) expression.Arguments[0];
					                                             return new SQLCriterion(new SqlString((string) expr.Value),
					                                                                     new object[0], new IType[0]);
				                                             });

		}

		public static bool Sql(string sql)
		{
			return false;
		}
	}
	
	public class OrderController : NHibernateController
	{

		public object Load(int id)
		{
			var order = Session.Get<Order>(id);
			
			return Json(new
				{
					order.Total,
					order.OrderLines.Count
				});
		}



		public object Search(string name)
		{
			Custom.Init();
			var customersNamed = QueryOver.Of<Customer>()
			                              .Where(x => x.FullName == name)
			                              .Select(x => x.Id);

			var y = Session.QueryOver<Order>()
					.Fetch(x=>x.Customer).Eager
					.WithSubquery.WhereProperty(x => x.Customer).In(customersNamed)
					.Where(x=> Custom.Sql(" exists (select 1) "))
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
					Total = 5,
					Attributes =
						{
							{"Active", true},
							{"Status", 5}
						}
				};
			var id = Session.Save(order);

			for (int i = 0; i < 5; i++)
			{
				var orderLine = new OrderLine
					{
						Product = Session.Load<Product>(196608 ),
						Order = order,
						Quantity = i + 1
					};
				order.OrderLines.Add(orderLine);
				Session.Save(orderLine);
			}
			return Json(id);
		}

		public object DoLine()
		{
			Session.Save(new OrderLine
				{
					Amount = 5,
					Currency = "USd",
					Quantity = 51
				});
			return Json("ok");
		}
	}
}