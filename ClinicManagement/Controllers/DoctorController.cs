using AutoMapper;
using Azure.Core;
using BLL;
using ClinicManagement.Request;
using ClinicManagement.Response;
using DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : BaseController
    {
        public readonly IDoctorBLL doctorBLL;
        public readonly IMapper mapper;

        public DoctorController(IDoctorBLL doctorBLL, IMapper mapper)
        {
            this.doctorBLL = doctorBLL;
            this.mapper = mapper;
        }

        #region CreateOrUpdate

        /// <summary>
        /// Creates or updates the Doctor
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<BLLResponse> CreateOrUpdate(DoctorSaveRequest request)
        {
            BLLResponse bLLResponse = null;

            var doctor = mapper.Map<DoctorSaveRequest, Doctor>(request);

            try
            {
                var result = await doctorBLL.CreateOrUpdate(doctor);
                if (request.Id == 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.Created, "Doctor Created Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Doctor Already Exists");
                    }
                }
                else if (request.Id > 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK, "Doctor Updated Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Doctor Already Exists");
                    }
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "An Error Occurred while Updaing Doctor");
                }
            }
            catch (Exception ex)
            {
                //log the error
            }

            return bLLResponse;
        }

        #endregion

        #region GetById

        /// <summary>
        /// Read DepartmBy Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<BLLResponse> GetById(int doctorId)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await doctorBLL.GetById(doctorId);
                if (result != null)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Doctor not exist");
                }
            }
            catch (Exception ex)
            {

            }
            return bLLResponse;
        }

        #endregion

        #region GetList

        /// <summary>
        /// Read DepartmBy List With Pagination ,search and sorting
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetList")]
        public async Task<BLLResponse> GetList([FromQuery] SortWithPageParametersRequest sortWithPageParameters = null)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var sortWithPageParm = mapper.Map<SortWithPageParametersRequest, SortWithPageParameters>(sortWithPageParameters);
                var result = await doctorBLL.GetList(sortWithPageParm);
                if (result.Doctors.Count > 0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Doctors not exist");
                }
            }
            catch (Exception ex)
            {

            }
            return bLLResponse;
        }

        #endregion
    }
}
