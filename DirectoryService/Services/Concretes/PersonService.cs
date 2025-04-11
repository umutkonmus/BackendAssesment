using AutoMapper;
using DirectoryService.DatabaseContext;
using DirectoryService.DTOs.Person;
using DirectoryService.Models;
using DirectoryService.Services.Abstracts;
using DirectoryService.Utils.Response;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Services.Concretes
{
    public sealed class PersonService : IPersonService
    {
        private readonly PostgresDbContext _dbContext;
        private readonly IMapper _mapper;

        public PersonService(PostgresDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response<CreatePersonDTO>> CreatePersonAsync(CreatePersonDTO person)
        {
            var _person = _mapper.Map<Person>(person);
            _person.ID = Guid.NewGuid();
            await _dbContext.Persons.AddAsync(_person);
            await _dbContext.SaveChangesAsync();
            return Response<CreatePersonDTO>.Success(_mapper.Map<CreatePersonDTO>(_person), (int)StatusCode.Created);
        }

        public async Task<Response<bool>> DeletePersonAsync(Guid id)
        {
            var _person = _dbContext.Persons.FirstOrDefault(x => x.ID == id);
            if (_person == null)
            {
                return Response<bool>.Fail("Person not found", (int)StatusCode.NotFound);
            }
            _dbContext.Persons.Remove(_person);
            await _dbContext.SaveChangesAsync();
            return Response<bool>.Success((int)StatusCode.Success);
        }

        public async Task<Response<List<PersonDTO>>> GetAllPersonsAsync()
        {
            var persons = await _dbContext.Persons.ToListAsync();
            if (persons == null)
                return Response<List<PersonDTO>>.Fail("No persons found", (int)StatusCode.NotFound);

            if (!persons.Any())
                return Response<List<PersonDTO>>.Success(new List<PersonDTO>(), (int)StatusCode.NoContent);

            return Response<List<PersonDTO>>.Success(_mapper.Map<List<PersonDTO>>(persons), (int)StatusCode.Success);
        }

        public async Task<Response<PersonWithContactInfoDTO>> GetPersonByIdAsync(Guid id)
        {
            var person = await _dbContext.Persons
                .Include(x => x.ContactInfos)
                .ThenInclude(x => x.Type)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (person == null)
                return Response<PersonWithContactInfoDTO>.Success(new PersonWithContactInfoDTO(),"Person not found", 204);

            return Response<PersonWithContactInfoDTO>.Success(_mapper.Map<PersonWithContactInfoDTO>(person), (int)StatusCode.Success);
        }
    }
}
