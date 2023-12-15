using IpToGeo.Dtos;

namespace IpToGeo.Services
{
    public interface IDataSourceService
    {
        public Task<IEnumerable<IpLocationDbSourceDto>> GetDataSource();

    }
}
