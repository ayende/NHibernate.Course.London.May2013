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

		public Customer()
		{
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
			Id(x => x.Id, mapper => mapper.Generator(new NativeGeneratorDef()));
			Property(x => x.FullName);

			Map(x => x.Addresses, mapper =>
				{
					mapper.Table("CustomerAddresses");
					mapper.Key(key => key.Column("CustomerId"));
				}, relation => relation.Component(mapper =>
					{
						mapper.Property(x => x.State);
						mapper.Property(x => x.City);
					}));

		}
	}
}