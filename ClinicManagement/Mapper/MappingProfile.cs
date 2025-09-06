using AutoMapper;
using ClinicManagement.Request;
using DAL.Entity;
namespace ClinicManagement.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdmissionSaveRequest, Admission>();
            CreateMap<DepartmentSaveRequest, Department>();
            CreateMap<DoctorSaveRequest, Doctor>();
            CreateMap<SortWithPageParametersRequest, SortWithPageParameters>();
        }
    }
}
