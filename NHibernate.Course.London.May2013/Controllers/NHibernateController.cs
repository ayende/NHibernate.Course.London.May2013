using System;
using System.Web.Mvc;
using NHibernate.Cfg;
using NHibernate.Course.London.May2013.Models;
using NHibernate.Dialect;
using Environment = System.Environment;

namespace NHibernate.Course.London.May2013.Controllers
{
	public abstract class NHibernateController : Controller
	{
		/*	 Session Factory
		 *	 - Manage the life time of connection to db
		 *	 - Connection String
		 *	 - Mapping - what tables to what objects?
		 *	 - What db are we working on (sql, oracle, mysql)
		 *	 - End point for creating sessions
		 *	 - Singleton!
		 *	 - Expensive to create
		 *	 - Thread safe
		 *	 - Exception safe
		 *	
		 *	 Session 
		 *	 - Similar to a db connection
		 *	 - Limited life scope
		 *	 - Single threaded
		 *	 - NOT exception safe
		 *	 - Cheap to create
		 *	 - Unit of Work
		 */

		private static readonly Lazy<ISessionFactory> sessionFactory = new Lazy<ISessionFactory>(() =>
			{
				var cfg = new Configuration()
					.DataBaseIntegration(db =>
						{
							db.Dialect<MsSql2008Dialect>();
							db.ConnectionStringName = Environment.MachineName;
							db.SchemaAction = SchemaAutoAction.Update;
						})
					.AddAssembly(typeof(Customer).Assembly); 
				return cfg.BuildSessionFactory();
			});

		public ISessionFactory SessionFactory {get { return sessionFactory.Value; }}

		protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
		}
	}
}