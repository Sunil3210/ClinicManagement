using AutoMapper;
using ClinicManagement.Request;
using DAL.Entity;
namespace ClinicManagement.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DepartmentSaveRequest, Department>();
            CreateMap<SortWithPageParametersRequest, SortWithPageParameters>();
        }
    }
}
