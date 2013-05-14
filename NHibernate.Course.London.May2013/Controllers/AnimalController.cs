using System;
using NHibernate.Course.London.May2013.Models;

namespace NHibernate.Course.London.May2013.Controllers
{
	public class AnimalController : NHibernateController
	{
		 public object Create()
		 {
			 Session.Save(new Animal
				 {
					 Name = "Generic Animal"
				 });
			 Session.Save(new Dog
				 {
					 Barks = true,
					 Name = "Oscar"
				 });
			 Session.Save(new GermanShepherd
				 {
					 Name = "Arava",
					 Barks = true,
					 Purebread = true
				 });

			 Session.Save(new Cat
				 {
					 LastHairball = DateTime.Today,
					 Name = "Go Away!"
				 });

			 return Json("Done");
		 }

		 public object QueryAnimals()
		 {
			 return Json(Session.QueryOver<Animal>().List());
		 }

		 public object QueryDogs()
		 {
			 return Json(Session.QueryOver<Dog>().List());
		 }

		 public object QueryCats()
		 {
			 return Json(Session.QueryOver<Cat>().List());
		 }

		 public object QueryGermanShepherd()
		 {
			 return Json(Session.QueryOver<GermanShepherd>().List());
		 }
	}
}