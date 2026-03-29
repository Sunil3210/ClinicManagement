using AutoMapper;
using ClinicManagement.Request;
using ClinicManagement.Response;
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
            CreateMap<StaffSaveRequest, Staff>();
            CreateMap<Staff, StaffResponse>();
            CreateMap<Doctor, DoctorResponse>();
            CreateMap<PatientSaveRequest, Patient>();
            CreateMap<Patient, PatientResponse>();

        }
    }
}
