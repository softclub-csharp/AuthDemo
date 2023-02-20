using AutoMapper;
using Domain.Dtos;
using Domain.Filters;
using Domain.Wrapper;
using Infrastructure.Data;

namespace Infrastructure.Services;


public interface  ICountryService
{
    Task<PagedResponse<List<CountryDto>>>GetCountries(CountryFilter filter);
}
public class CountryService : ICountryService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CountryService(DataContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PagedResponse<List<CountryDto>>> GetCountries(CountryFilter filter)
    {
        var query = _context.Countries.AsQueryable();

        if (filter.Name != null)
            query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

        var totalRecords = query.Count();
         var filtered = query.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).OrderBy(x=>x.Id).ToList();

         var mapped = _mapper.Map<List<CountryDto>>(filtered);

         return new PagedResponse<List<CountryDto>>(mapped, totalRecords, filter.PageNumber, filter.PageSize);
         // 150 records
         //pageNumber = 3
         //pageSize = 10

    }
}