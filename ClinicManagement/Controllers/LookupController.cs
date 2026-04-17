using BLL;
using ClinicManagement.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : BaseController
    {
        public readonly ILookupBLL lookupBLL;

        public LookupController(ILookupBLL lookupBLL)
        {
            this.lookupBLL = lookupBLL;
        }

        #region GetDepartments

        /// <summary>
        /// Read Departments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDepartments")]
        public async Task<BLLResponse> GetDepartments()
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await lookupBLL.GetDepartments();
                if (result.Count > 0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Data not exist");
                }
            }
            catch (Exception ex)
            {

            }
            return bLLResponse;
        }

        #endregion

        #region GetDoctors

        /// <summary>
        /// Read DepartmBy List With Pagination ,search and sorting
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDoctors")]
        public async Task<BLLResponse> GetDoctors()
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await lookupBLL.GetDoctors();
                if (result.Count > 0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Data not exist");
                }
            }
            catch (Exception ex)
            {

            }
            return bLLResponse;
        }

        #endregion

        #region GetRoomType

        /// <summary>
        /// Read RoomType List for drop downs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRoomType")]
        public async Task<BLLResponse> GetRoomType()
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await lookupBLL.GetRoomType();
                if (result.Count > 0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Data not exist");
                }
            }
            catch (Exception ex)
            {

            }
            return bLLResponse;
        }

        #endregion

        #region GetAvailableRoomsByType

        /// <summary>
        /// Read RoomType List for drop downs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAvailableRoomsByType")]
        public async Task<BLLResponse> GetAvailableRoomsByType(int? typeId)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await lookupBLL.GetAvailableRoomsByType(typeId);
                if (result.Count > 0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Data not exist");
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
