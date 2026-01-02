using AutoMapper;
using Azure.Core;
using BLL;
using ClinicManagement.Filters;
using ClinicManagement.Request;
using ClinicManagement.Response;
using DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        public readonly IDepartmentBLL departmentBLL;
        public readonly IMapper mapper;

        public DepartmentController(IDepartmentBLL departmentBLL, IMapper mapper)
        {
            this.departmentBLL = departmentBLL;
            this.mapper = mapper;
        }

        #region CreateOrUpdate

        /// <summary>
        /// Creates or updates the Department
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AdminOnlyAttribute]
        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<BLLResponse> CreateOrUpdate(DepartmentSaveRequest request)
        {
            BLLResponse bLLResponse = null;

            var department = mapper.Map<DepartmentSaveRequest, Department>(request);
        
            try
            {
                var result = await departmentBLL.CreateOrUpdate(department);
                if (request.Id == 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.Created, "Department Created Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Department Already Exists");
                    }
                }
                else if (request.Id > 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK, "Department Updated Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Department Already Exists");
                    }
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "An Error Occurred while Updaing Department");
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
        public async Task<BLLResponse> GetById(int departmentId)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await departmentBLL.GetById(departmentId);
                if (result != null)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Department not exist");
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
        /// 
        [HttpGet]
        [Route("GetList")]
        public async Task<BLLResponse> GetList([FromQuery]SortWithPageParametersRequest sortWithPageParameters =null)
        {
            BLLResponse bLLResponse = null;

            //try
            //{
                var sortWithPageParm = mapper.Map<SortWithPageParametersRequest, SortWithPageParameters>(sortWithPageParameters);
                var result = await departmentBLL.GetList(sortWithPageParm);
                if (result.Departments.Count>0)
                {
                    bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Departments not exist");
                }
            //}
            //catch (Exception ex)
            //{

            //}
            return bLLResponse;
        }

        #endregion
    }
}
