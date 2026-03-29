using AutoMapper;
using BLL;
using ClinicManagement.Extension;
using ClinicManagement.Request;
using ClinicManagement.Response;
using DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : BaseController
    {
        public readonly IPatientBLL patientBLL;
        public readonly IMapper mapper;
        public readonly IUserClaimService userClaimService;

        public PatientController(IPatientBLL patientBLL, IMapper mapper, IUserClaimService userClaimService)
        {
            this.patientBLL = patientBLL;
            this.mapper = mapper;
            this.userClaimService = userClaimService;
        }

        #region CreateOrUpdate

        /// <summary>
        /// Creates or updates the Patient
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<BLLResponse> CreateOrUpdate(PatientSaveRequest request)
        {
            BLLResponse bLLResponse = null;

            var patient = mapper.Map<PatientSaveRequest, Patient>(request);
            
            patient.CreatedBy = int.Parse(userClaimService.GetUserId());//loggedInUser
            try
            {
                var result = await patientBLL.CreateOrUpdate(patient);
                if (request.Id == 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.Created, "Patient Created Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Patient Already Exists.");
                    }
                }
                else if (request.Id > 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK, "Patient Updated Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(result, HttpStatusCode.InternalServerError, "Patient Already Exists");
                    }
                }
                else
                {
                    bLLResponse = CreateFailResponse(-1, HttpStatusCode.InternalServerError, "An Error Occurred while Updating Patient");
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
        /// Read Patient By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<BLLResponse> GetById(int patientId)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await patientBLL.GetById(patientId);
                var response = mapper.Map<Patient, PatientResponse>(result);
                if (response != null)
                {
                    bLLResponse = CreateSuccessResponse(response, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Patient not exist");
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
        /// Read Patient List With Pagination ,search and sorting
        /// </summary>
        /// <returns></returns>

        [Authorize]
        [HttpGet]
        [Route("GetList")]
        public async Task<BLLResponse> GetList([FromQuery] SortWithPageParametersRequest sortWithPageParameters = null)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var sortWithPageParm = mapper.Map<SortWithPageParametersRequest, SortWithPageParameters>(sortWithPageParameters);
                var result = await patientBLL.GetList(sortWithPageParm);
                if (result.Patients.Count > 0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Patients not exist");
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
