using System;
using System.IO;
using System.Collections.Generic;
using Examples.FirstProject.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Examples.FirstProject {
    class Program {
		private const string DbFile = "firstProgram.db";

		static void Main() {
			// create our NHibernate session factory
			var sessionFactory = CreateSessionFactory();

			using (var session = sessionFactory.OpenSession()) {
				// populate the database
				using (var transaction = session.BeginTransaction()) {
					// create a couple of Stores each with some Products and Employees
					Dictionary<string, Store> StoreList = new Dictionary<string, Store>() {
						{ "barginBasin", new Store { Name = "Bargin Basin"} },
						{ "superMart"  , new Store { Name = "SuperMart"   } }
					};

					Dictionary<string, Product> ProductList = new Dictionary<string, Product>() {
						{ "potatoes", new Product { Name = "Potatoes", Price = 3.60 } },
						{ "fish",     new Product { Name = "Fish",     Price = 4.49 } },
						{ "milk",     new Product { Name = "Milk",     Price = 0.79 } },
						{ "bread",    new Product { Name = "Bread",    Price = 1.29 } },
						{ "cheese",   new Product { Name = "Cheese",   Price = 2.10 } },
						{ "waffles",  new Product { Name = "Waffles",  Price = 2.41 } }
					};

					Dictionary<string, Employee> EmployeeList = new Dictionary<string, Employee>() {
						{ "daisy", new Employee { FirstName = "Daisy", LastName = "Harrison" } },
						{ "jack",  new Employee { FirstName = "Jack",  LastName = "Torrance" } },
						{ "sue",   new Employee { FirstName = "Sue",   LastName = "Walkters" } },
						{ "bill",  new Employee { FirstName = "Bill",  LastName = "Taft" } },
						{ "joan" , new Employee { FirstName = "Joan",  LastName = "Pope" } }
					};

					// add products to the stores, there's some crossover in the products in each
					// store, because the store-product relationship is many-to-many
					AddProductsToStore(StoreList["barginBasin"], ProductList["potatoes"],
									   ProductList["fish"], ProductList["milk"], ProductList["bread"], ProductList["cheese"]
									  );
					AddProductsToStore(StoreList["superMart"], ProductList["bread"],
									   ProductList["cheese"], ProductList["waffles"]
									  );

					// add employees to the stores, this relationship is a one-to-many, so one
					// employee can only work at one store at a time
					AddEmployeesToStore(StoreList["barginBasin"], EmployeeList["daisy"], EmployeeList["jack"], EmployeeList["sue"]);
					AddEmployeesToStore(StoreList["superMart"], EmployeeList["bill"], EmployeeList["joan"]);

					// save both stores, this saves everything else via cascading
					session.SaveOrUpdate(StoreList["barginBasin"]);
					session.SaveOrUpdate(StoreList["superMart"]);

					transaction.Commit();
				}
			}

			using (var session = sessionFactory.OpenSession()) {
				// retreive all stores and display them
				using (session.BeginTransaction()) {
					var stores = session.CreateCriteria(typeof(Store))
								 .List<Store>();

					foreach (var store in stores) {
						WriteStorePretty(store);
					}
				}
			}

			Console.ReadKey();
		}

		private static ISessionFactory CreateSessionFactory() {
			return Fluently.Configure()
				   .Database(SQLiteConfiguration.Standard
							 .UsingFile(DbFile))
				   .Mappings(m =>
							 m.FluentMappings.AddFromAssemblyOf<Program>())
				   .ExposeConfiguration(BuildSchema)
				   .BuildSessionFactory();
		}

		private static void BuildSchema(Configuration config) {
			// delete the existing db on each run
			if (File.Exists(DbFile))
				File.Delete(DbFile);

			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			new SchemaExport(config)
			.Create(false, true);
		}

		private static void WriteStorePretty(Store store) {
			Console.WriteLine(store.Name);
			Console.WriteLine("  Products:");

			foreach (var product in store.Products) {
				Console.WriteLine("    " + product.Name);
			}

			Console.WriteLine("  Staff:");

			foreach (var employee in store.Staff) {
				Console.WriteLine("    " + employee.FirstName + " " + employee.LastName);
			}

			Console.WriteLine();
		}

		public static void AddProductsToStore(Store store, params Product[] products) {
			foreach (var product in products) {
				store.AddProduct(product);
			}
		}

		public static void AddEmployeesToStore(Store store, params Employee[] employees) {
			foreach (var employee in employees) {
				store.AddEmployee(employee);
			}
		}
	}
}
