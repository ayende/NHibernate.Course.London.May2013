using System;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Course.London.May2013.Models
{
	public class Animal
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		public virtual string Behavior()
		{
			return "aniamal";
		}
	}

	public class Dog : Animal
	{
		public virtual bool Barks { get; set; }
	}

	public class GermanShepherd : Dog
	{
		public virtual bool Purebread { get; set; }

		public override string Behavior()
		{
			return "GermanShepherd";
		}
	}

	public class Cat : Animal
	{
		public virtual DateTime? LastHairball { get; set; }
	}

	public class AnimalMap : ClassMapping<Animal>
	{
		public AnimalMap()
		{
			Id(x=>x.Id, mapper => mapper.Generator(new HighLowGeneratorDef()));

			Property(x=>x.Name);
		}
	}

	public class DogMap : UnionSubclassMapping<Dog>
	{
		public DogMap()
		{
			Property(x => x.Barks);
		}
	}

	public class GermanShepherdMap : UnionSubclassMapping<GermanShepherd>
	{
		public GermanShepherdMap()
		{
			Property(x => x.Purebread);
		}
	}

	public class CatMap : UnionSubclassMapping<Cat>
	{
		public CatMap()
		{
			Property(x => x.LastHairball);
		}
	}
}