using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Course.London.May2013.Models
{
	public class Customer
	{
		public virtual int Id { get; set; }
		public virtual string FullName { get; set; }
		public virtual IDictionary<string, Address> Addresses { get; set; }
		public virtual IList<string> EmergencyContactNumbers { get; set; }
		public virtual Animal Animal { get; set; }
		public virtual  int Version { get; set; }
		public Customer()
		{
			EmergencyContactNumbers = new List<string>();
			Addresses = new Dictionary<string, Address>();
		}
	}

	public class Address
	{
		public string City { get; set; }
		public string State { get; set; }
	}

	public class CustomerMap : ClassMapping<Customer>
	{
		public CustomerMap()
		{
			Cache(x => x.Usage(CacheUsage.NonstrictReadWrite));
			Id(x => x.Id, mapper => mapper.Generator(new NativeGeneratorDef()));
			Property(x => x.FullName);

			ManyToOne(x => x.Animal);
			Version(x => x.Version, mapper => mapper.UnsavedValue(0));

			List(x => x.EmergencyContactNumbers, mapper =>
				{
					mapper.Table("EmergencyContactNumbers");
					mapper.Key(key => key.Column("CustomerId"));
					mapper.Cache(cacheMapper => cacheMapper.Usage(CacheUsage.NonstrictReadWrite));
				}, relation => relation.Element(mapper => mapper.Column("Phone")));

			Map(x => x.Addresses, mapper =>
				{
					mapper.Table("CustomerAddresses");
					mapper.Key(key => key.Column("CustomerId"));
					mapper.Cache(cacheMapper => cacheMapper.Usage(CacheUsage.NonstrictReadWrite));
				}, relation => relation.Component(mapper =>
					{
						mapper.Property(x => x.State);
						mapper.Property(x => x.City);
					}));

		}
	}
}