using AutoMapper;
using BLL;
using ClinicManagement.Request;
using ClinicManagement.Response;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : BaseController
    {
        public readonly IStaffBLL staffBLL;
        public readonly IMapper mapper;

        public StaffController(IStaffBLL staffBLL, IMapper mapper)
        {
            this.staffBLL = staffBLL;
            this.mapper = mapper;
        }

        #region CreateOrUpdate

        /// <summary>
        /// Creates or updates the Staff
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<BLLResponse> CreateOrUpdate(StaffSaveRequest request)
        {
            BLLResponse bLLResponse = null;

            var staff = mapper.Map<StaffSaveRequest, Staff>(request);
            staff.CreatedBy = 1;//loggedInUser
            try
            {
                var result = await staffBLL.CreateOrUpdate(staff);
                if (request.Id == 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.Created, "Staff Created Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Staff Already Exists");
                    }
                }
                else if (request.Id > 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK, "Staff Updated Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Staff Already Exists");
                    }
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "An Error Occurred while Updaing Staff");
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
        //[Authorize(Roles = "Nurse")]
        //[Authorize(Roles = "Admin,Manager")]
        //[Authorize(Policy = "AdminOrManager")]
        [HttpGet]
        [Route("GetById")]
        public async Task<BLLResponse> GetById(int staffId)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await staffBLL.GetById(staffId);
                if (result != null)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Staff not exist");
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
        /// Read Staff List With Pagination ,search and sorting
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
                var result = await staffBLL.GetList(sortWithPageParm);
                if (result.Staffs.Count > 0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Staffs not exist");
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
