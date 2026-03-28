using AutoMapper;
using BLL;
using ClinicManagement.Request;
using ClinicManagement.Response;
using DAL.Entity;
using DAL;
using Microsoft.AspNetCore.Http;
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
        /// Read DepartmBy List With Pagination ,search and sorting
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
    }
}
