using AutoMapper;
using DirectoryService.DatabaseContext;
using DirectoryService.DTOs.ContactInfo;
using DirectoryService.Mapper;
using DirectoryService.Models;
using DirectoryService.Services.Concretes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DirectoryService.Tests
{
    public class ContactInfoServiceTests
    {
        private readonly IMapper _mapper;

        public ContactInfoServiceTests()
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
        public async Task CreateContactInfoAsync_ShouldCreateAndReturnContactInfo()
        {
            var context = GetDbContext("CreateContactInfo");
            var service = new ContactInfoService(context, _mapper);
            var personId = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            context.Persons.Add(new Person
            {
                ID = personId,
                FirstName = "John",
                LastName = "Doe",
                CompanyName = "Kocsistem"
            });

            context.ContactTypes.Add(new ContactType
            {
                ID = typeId,
                Name = "Phone"
            });

            await context.SaveChangesAsync();

            var dto = new CreateContactInfoDTO
            {
                PersonID = personId,
                TypeID = typeId,
                Value = "5551234567"
            };

            var result = await service.CreateContactInfoAsync(dto);

            Assert.True(result.IsSuccessful);
            Assert.Equal(201, result.Status);
            Assert.Equal("5551234567", result.Data.Value);
            Assert.Equal(personId, result.Data.PersonID);
            Assert.Equal(typeId, result.Data.TypeID);
        }

        [Fact]
        public async Task DeleteContactInfoAsync_ShouldFail_WhenNotFound()
        {
            var context = GetDbContext("DeleteContactInfo_NotFound");
            var service = new ContactInfoService(context, _mapper);
            var id = Guid.NewGuid();

            var result = await service.DeleteContactInfoAsync(id);

            Assert.False(result.IsSuccessful);
            Assert.Equal(404, result.Status);
            Assert.Equal("Contact info not found", result.Errors.First());
        }

        [Fact]
        public async Task DeleteContactInfoAsync_ShouldDelete_WhenExists()
        {
            var context = GetDbContext("DeleteContactInfo_Success");
            var contactInfo = new ContactInfo
            {
                ID = Guid.NewGuid(),
                PersonID = Guid.NewGuid(),
                TypeID = Guid.NewGuid(),
                Value = "test@example.com"
            };

            context.ContactInfos.Add(contactInfo);
            await context.SaveChangesAsync();

            var service = new ContactInfoService(context, _mapper);

            var result = await service.DeleteContactInfoAsync(contactInfo.ID);

            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.Status);
            Assert.Empty(context.ContactInfos.ToList());
        }
    }
}
