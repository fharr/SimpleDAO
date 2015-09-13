# SimpleDAO

Provides very simple Repository and UnitOfWork interfaces and their generic implementations. These simple components are meant to be used within applications using an IoC container but you can also use them in all other cases.

It is the first brick of a larger toolset that will come later.

For the moment, there is only one implementation of these interfaces : EntityFramework. Do not hesitate to improve my work or provide other implementation (NHibernate for example).

## Installation

All the components of SimpleDAO are available through NuGet packages.

You can install the interface project with :

		PM> Install-Package SimpleDAO
		
You can install the EntityFramework implementation with :

		PM> Install-Package SimpleDAO.EntityFramework
		
Etc.

## Usage

### Your own implementation

These components are designed to entirely abstract the datastore implementation. The repository interface needs a domain Object to works :

		public FooRepository : IGenericRepository<FooDomain>
		{ [...] }

This domain object does not have to match perfectly the implementation entity, but a two-way mapping has to be possible.
		
### Entity Framework implementation

To abstract the entity framework objects (dbContext, dbSet and entities) from the rest of the application, you need to provide them to the generic entity framework implementation :

		public FooRepository : GenericRepository<FooEntity, FooDomain, FooUnitOfWork, FooDbContext>
		{
			public GenericRepository(FooUnitOfWork unitOfWork)
				: base(unitOfWork, (fooDomain, fooEntity) => fooDomain.Id == fooEntity.Id)
			{ }
			
			[...]
		}

When a call to unitOfWork.SaveChanges() is done, the pending changes of all the repositories initialized with the unitOfWork instance will be persisted to the underlying datastore.
		
The finder is needed to allow the generic repository to find the entity within the dbContext cache. In order to map your entity object to your domain object, your entity have to implement IMappableEntity :

		public partial class FooEntity : IMappableEntity<FooDomain>
		{ [...] }

This interface will force you to implement two methods needed by the GenericRepository. You could also use the AutoMappableEntity base class which already implements these two methods :
	
		public partial class FooEntity : AutoMappableEntity<FooEntity, FooDOmain>
		{ [...] }
		
This base class use the [AutoMapper](https://github.com/AutoMapper/AutoMapper) to realize these operations. If you need to custom the automapping, you can add to your Entity Partial class a static constructor. It has to call the base class Initialize() method to call its static constructor first:

		partial class FooEntity : AutoMappableEntity<FooEntity, FooDomain>
		{
			static FooEntity()
			{
				// Required before any mapping customization
				Initialize();

				Mapper.FindTypeMapFor<FooEntity, FooDomain>()
					.AddAfterMapAction((entity, domain) => (domain as FooDomain).FooProperty = (entity as FooEntity).FooList.Count);
			}
		}
		
## Tests

To ease the test your own implementation of SimpleDAO, you can use the SimpleDAO.Tests projects. You can create a new project that implement all the interfaces within SimpleDAO.Tests.DAL. You will also have to publish the SimpleDAO.Tests.Database. To do so, double click on the SimpleDAO.Tests.Database\Publish\LocalDatabase.publish and click on publish.

Finally, you can create a new class in the SimpleDAO.Tests project that implement GenericRepositoryTest and give it your own implementation of SimpleDAO.Tests.DAL.ITestUnitOfWork :

		[TestClass]
		public class EntityFrameworkRepositoryTest : GenericRepositoryTest<EFTestUnitOfWork>
		{ }
		
It will test all the basics functionnalities of the SimpleDAO interfaces. Just run all the tests to check if your implementation is correct.