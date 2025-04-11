using AutoMapper;
using DirectoryService.DatabaseContext;
using DirectoryService.DTOs.Person;
using DirectoryService.Mapper;
using DirectoryService.Models;
using DirectoryService.Services.Concretes;
using DirectoryService.Utils.Response;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DirectoryService.Tests.Services
{
    public class PersonServiceTests
    {
        private readonly IMapper _mapper;

        public PersonServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private PostgresDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PostgresDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new PostgresDbContext(options);
        }

        [Fact]
        public async Task CreatePersonAsync_ShouldReturnSuccess_WhenValidInput()
        {
            var context = GetDbContext("CreatePersonDb");
            var service = new PersonService(context, _mapper);

            var dto = new CreatePersonDTO { FirstName = "John", LastName = "Doe", CompanyName = "Kocsistem" };

            var result = await service.CreatePersonAsync(dto);

            Assert.True(result.IsSuccessful);
            Assert.Equal(201, result.Status);
            Assert.Equal("John", result.Data.FirstName);
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldReturnFail_WhenPersonNotFound()
        {
            var context = GetDbContext("DeletePersonDb_Fail");
            var service = new PersonService(context, _mapper);

            var result = await service.DeletePersonAsync(Guid.NewGuid());

            Assert.False(result.IsSuccessful);
            Assert.Equal(404, result.Status);
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldReturnSuccess_WhenPersonExists()
        {
            var context = GetDbContext("DeletePersonDb_Success");
            var person = new Person { ID = Guid.NewGuid(), FirstName = "Ali", LastName = "Veli" , CompanyName = "Kocsistem"};
            context.Persons.Add(person);
            await context.SaveChangesAsync();

            var service = new PersonService(context, _mapper);

            var result = await service.DeletePersonAsync(person.ID);

            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.Status);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ShouldReturnNoContent_WhenEmpty()
        {
            var context = GetDbContext("GetAll_Empty");
            var service = new PersonService(context, _mapper);

            var result = await service.GetAllPersonsAsync();

            Assert.True(result.IsSuccessful);
            Assert.Empty(result.Data);
            Assert.Equal(204, result.Status);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ShouldReturnSuccess_WhenDataExists()
        {
            var context = GetDbContext("GetAll_WithData");
            context.Persons.Add(new Person { ID = Guid.NewGuid(), FirstName = "John", LastName = "Doe" , CompanyName = "Kocsistem" });
            await context.SaveChangesAsync();

            var service = new PersonService(context, _mapper);

            var result = await service.GetAllPersonsAsync();

            Assert.True(result.IsSuccessful);
            Assert.Single(result.Data);
            Assert.Equal(200, result.Status);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnEmptyDto_WhenNotFound()
        {
            var context = GetDbContext("GetById_NotFound");
            var service = new PersonService(context, _mapper);

            var result = await service.GetPersonByIdAsync(Guid.NewGuid());

            Assert.True(result.IsSuccessful);
            Assert.Equal(204, result.Status);
            Assert.NotNull(result.Data);
            Assert.IsType<PersonWithContactInfoDTO>(result.Data);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnPersonWithContactInfo_WhenExists()
        {
            var context = GetDbContext("GetById_Success");

            var personId = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            var contactType = new ContactType
            {
                ID = typeId,
                Name = "Phone"
            };
            context.ContactTypes.Add(contactType);

            var person = new Person
            {
                ID = personId,
                FirstName = "Sultan",
                LastName = "Sancar",
                CompanyName = "Kocsistem"
            };
            context.Persons.Add(person);
            await context.SaveChangesAsync();

            var contactInfo = new ContactInfo
            {
                ID = Guid.NewGuid(),
                PersonID = personId,
                TypeID = typeId,
                Type = contactType,
                Value = "1234567890"
            };
            context.ContactInfos.Add(contactInfo);
            await context.SaveChangesAsync();

            var service = new PersonService(context, _mapper);
            var result = await service.GetPersonByIdAsync(personId);

            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.Status);
            Assert.NotNull(result.Data);
            Assert.Equal("Sultan", result.Data.FirstName);
            Assert.Single(result.Data.ContactInfos);
            Assert.Equal("1234567890", result.Data.ContactInfos[0].Value);
            Assert.Equal("Phone", result.Data.ContactInfos[0].TypeName);
        }

    }
}
