using System;

namespace NHibernate.Course.London.May2013.Models
{
	public class Animal
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}

	public class Dog : Animal
	{
		public virtual bool Barks { get; set; }
	}

	public class GermanShepherd : Dog
	{
		public virtual bool Purebread { get; set; }
	}

	public class Cat : Animal
	{
		public virtual DateTime? LastHairball { get; set; }
	}
}