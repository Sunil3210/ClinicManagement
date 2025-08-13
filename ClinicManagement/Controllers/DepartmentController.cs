using BLL;
using ClinicManagement.Request;
using ClinicManagement.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        public readonly IDepartmentBLL departmentBLL;

        public DepartmentController(IDepartmentBLL departmentBLL)
        {
            this.departmentBLL = departmentBLL;
        }

        //public async Task<BLLResponse> Create(DepartmentSaveRequest request)
        //{

        //}
    }
}
