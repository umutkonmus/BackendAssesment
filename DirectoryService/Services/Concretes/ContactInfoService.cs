using AutoMapper;
using DirectoryService.DatabaseContext;
using DirectoryService.DTOs.ContactInfo;
using DirectoryService.Models;
using DirectoryService.Services.Abstracts;
using DirectoryService.Utils.Response;

namespace DirectoryService.Services.Concretes
{
    public sealed class ContactInfoService : IContactInfoService
    {
        private readonly PostgresDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContactInfoService(PostgresDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response<CreateContactInfoDTO>> CreateContactInfoAsync(CreateContactInfoDTO contactInfo)
        {
            var _contactInfo = _mapper.Map<ContactInfo>(contactInfo);
            _contactInfo.ID = Guid.NewGuid();
            await _dbContext.ContactInfos.AddAsync(_contactInfo);
            await _dbContext.SaveChangesAsync();
            return Response<CreateContactInfoDTO>.Success(_mapper.Map<CreateContactInfoDTO>(_contactInfo), (int)StatusCode.Created);
        }

        public async Task<Response<bool>> DeleteContactInfoAsync(Guid id)
        {
            var _contactInfo = _dbContext.ContactInfos.FirstOrDefault(x => x.ID == id);
            if (_contactInfo == null)
            {
                return Response<bool>.Fail("Contact info not found", (int)StatusCode.NotFound);
            }
            _dbContext.ContactInfos.Remove(_contactInfo);
            await _dbContext.SaveChangesAsync();
            return Response<bool>.Success(200);
        }

    }
}
