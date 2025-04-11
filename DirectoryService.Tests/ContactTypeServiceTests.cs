using AutoMapper;
using DirectoryService.DatabaseContext;
using DirectoryService.DTOs.ContactType;
using DirectoryService.Mapper;
using DirectoryService.Models;
using DirectoryService.Services.Concretes;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Tests.Services
{
    public class ContactTypeServiceTests
    {
        private readonly IMapper _mapper;

        public ContactTypeServiceTests()
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
        public async Task CreateContactTypeAsync_ShouldCreateAndReturnContactType()
        {
            var context = GetDbContext("CreateContactType");
            var service = new ContactTypeService(context, _mapper);
            var dto = new CreateContactTypeDTO { Name = "Email" };

            var result = await service.CreateContactTypeAsync(dto);

            Assert.True(result.IsSuccessful);
            Assert.Equal(201, result.Status);
            Assert.Equal("Email", result.Data.Name);
            Assert.NotEqual(Guid.Empty, context.ContactTypes.First().ID);
        }

        [Fact]
        public async Task DeleteContactTypeAsync_ShouldFail_WhenContactTypeNotFound()
        {
            var context = GetDbContext("DeleteContactType_NotFound");
            var service = new ContactTypeService(context, _mapper);
            var id = Guid.NewGuid();

            var result = await service.DeleteContactTypeAsync(id);

            Assert.False(result.IsSuccessful);
            Assert.Equal(404, result.Status);
            Assert.Equal("Contact type not found", result.Errors.FirstOrDefault());
        }

        [Fact]
        public async Task DeleteContactTypeAsync_ShouldDelete_WhenContactTypeExists()
        {
            var context = GetDbContext("DeleteContactType_Success");
            var contactType = new ContactType { ID = Guid.NewGuid(), Name = "Phone" };
            context.ContactTypes.Add(contactType);
            await context.SaveChangesAsync();

            var service = new ContactTypeService(context, _mapper);

            var result = await service.DeleteContactTypeAsync(contactType.ID);

            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.Status);
            Assert.Empty(context.ContactTypes.ToList());
        }

        [Fact]
        public async Task GetAllContactTypesAsync_ShouldReturnNoContent_WhenListIsEmpty()
        {
            var context = GetDbContext("GetAllContactTypes_Empty");
            var service = new ContactTypeService(context, _mapper);

            var result = await service.GetAllContactTypesAsync();

            Assert.True(result.IsSuccessful);
            Assert.Equal(204, result.Status);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetAllContactTypesAsync_ShouldReturnList_WhenExists()
        {
            var context = GetDbContext("GetAllContactTypes_Success");
            context.ContactTypes.Add(new ContactType { ID = Guid.NewGuid(), Name = "Location" });
            context.ContactTypes.Add(new ContactType { ID = Guid.NewGuid(), Name = "Email" });
            await context.SaveChangesAsync();

            var service = new ContactTypeService(context, _mapper);

            var result = await service.GetAllContactTypesAsync();

            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.Status);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, ct => ct.Name == "Location");
            Assert.Contains(result.Data, ct => ct.Name == "Email");
        }
    }

}