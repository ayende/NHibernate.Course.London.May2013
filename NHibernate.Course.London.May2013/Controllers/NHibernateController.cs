using System;
using System.Text;
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
				Configuration cfg = new Configuration()
					.DataBaseIntegration(db =>
						{
							db.Dialect<MsSql2008Dialect>();
							db.ConnectionStringName = Environment.MachineName;
							db.SchemaAction = SchemaAutoAction.Update;
							db.BatchSize = 250;
						})
					.AddAssembly(typeof (Customer).Assembly);

				var mapper = new ModelMapper();

				mapper.AfterMapClass += 
					(inspector, type, customizer) =>
						 customizer.Table(Inflector.Net.Inflector.Pluralize(type.Name));

				mapper.AddMappings(typeof (ProductMap).Assembly.GetTypes());
				cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

				return cfg.BuildSessionFactory();
			});

		private ISession session;
		private ITransaction tx;

		public ISessionFactory SessionFactory
		{
			get { return sessionFactory.Value; }
		}

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

		protected override JsonResult Json(object data, string contentType, Encoding contentEncoding,
		                                   JsonRequestBehavior behavior)
		{
			return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
		}

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