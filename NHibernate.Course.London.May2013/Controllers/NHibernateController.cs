using System;
using System.Web.Mvc;
using NHibernate.Cfg;
using NHibernate.Course.London.May2013.Models;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
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

				var mapper = new ModelMapper();
				mapper.AddMappings(typeof (ProductMap).Assembly.GetTypes());
				cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

				return cfg.BuildSessionFactory();
			});

		public ISessionFactory SessionFactory { get { return sessionFactory.Value; } }

		protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
		}

		private ISession session;
		private ITransaction tx;

		public new ISession Session
		{
			get
			{
				if (session == null)
				{
					session = SessionFactory.OpenSession();
					tx = session.BeginTransaction();
				}
				return session;
			}
		}

		protected bool DoNotCommit { get; set; }

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.Exception != null)
				return;
			using (session)
			using (tx)
			{
				if (DoNotCommit)
					return;
				if (tx != null)
					tx.Commit();
			}

		}
	}
}