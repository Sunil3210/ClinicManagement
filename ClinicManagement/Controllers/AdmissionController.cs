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
    public class AdmissionController : BaseController
    {
        public readonly IAdmissionBLL admissionBLL;
        public readonly IMapper mapper;

        public AdmissionController(IAdmissionBLL admissionBLL, IMapper mapper)
        {
            this.admissionBLL = admissionBLL;
            this.mapper = mapper;
        }

        #region CreateOrUpdate

        /// <summary>
        /// Creates or updates the Admission
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<BLLResponse> CreateOrUpdate([FromBody]AdmissionSaveRequest request)
        {
            BLLResponse bLLResponse = null;

            var admission = mapper.Map<AdmissionSaveRequest, Admission>(request);

            try
            {
                var result = await admissionBLL.CreateOrUpdate(admission);
                if (request.Id == 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.Created, "Admission Created Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Admission Already Exists");
                    }
                }
                else if (request.Id > 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK, "Admission Updated Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Admission Already Exists");
                    }
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "An Error Occurred while Updaing Admission");
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
        public async Task<BLLResponse> GetById(int admissionId)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await admissionBLL.GetById(admissionId);
                if (result != null)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Admission not exist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var result = await admissionBLL.GetList(sortWithPageParm);
                if (result.admissions.Count > 0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Admissions not exist");
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
