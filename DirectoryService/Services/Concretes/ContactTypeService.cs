using AutoMapper;
using DirectoryService.DatabaseContext;
using DirectoryService.DTOs.ContactType;
using DirectoryService.Services.Abstracts;
using DirectoryService.Utils.Response;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Services.Concretes
{
    public sealed class ContactTypeService : IContactTypeService
    {
        private readonly PostgresDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContactTypeService(PostgresDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response<CreateContactTypeDTO>> CreateContactTypeAsync(CreateContactTypeDTO contactType)
        {
            var _contactType = _mapper.Map<Models.ContactType>(contactType);
            _contactType.ID = Guid.NewGuid();
            await _dbContext.ContactTypes.AddAsync(_contactType);
            await _dbContext.SaveChangesAsync();
            return Response<CreateContactTypeDTO>.Success(_mapper.Map<CreateContactTypeDTO>(_contactType), (int)StatusCode.Created);

        }

        public async Task<Response<bool>> DeleteContactTypeAsync(Guid id)
        {
            var _contactType = _dbContext.ContactTypes.FirstOrDefault(x => x.ID == id);
            if (_contactType != null)
                return Response<bool>.Fail("Contact type not found", (int)StatusCode.NotFound);

            _dbContext.ContactTypes.Remove(_contactType);
            await _dbContext.SaveChangesAsync();
            return Response<bool>.Success(200);
        }

        public async Task<Response<List<ContactTypeDTO>>> GetAllContactTypesAsync()
        {
            var _contactTypes = await _dbContext.ContactTypes.ToListAsync();
            if (_contactTypes == null)
                return Response<List<ContactTypeDTO>>.Fail("No contact types found", (int)StatusCode.NotFound);
            if (!_contactTypes.Any())
                return Response<List<ContactTypeDTO>>.Success(new List<ContactTypeDTO>(), (int)StatusCode.NoContent);
            return Response<List<ContactTypeDTO>>.Success(_mapper.Map<List<ContactTypeDTO>>(_contactTypes),(int)StatusCode.Success);
        }
    }
}
