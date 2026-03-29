using AutoMapper;
using BLL;
using ClinicManagement.Extension;
using ClinicManagement.Request;
using ClinicManagement.Response;
using DAL;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : BaseController
    {
        public readonly IRoomBLL roomBLL;
        public readonly IMapper mapper;
        public readonly IUserClaimService userClaimService;

        public RoomController(IRoomBLL roomBLL, IMapper mapper, IUserClaimService userClaimService)
        {
            this.roomBLL = roomBLL;
            this.mapper = mapper;
            this.userClaimService = userClaimService;
        }

        #region CreateOrUpdate

        /// <summary>
        /// Creates or updates the Staff
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<BLLResponse> CreateOrUpdate(RoomSaveRequest request)
        {
            BLLResponse bLLResponse = null;

            var room = mapper.Map<RoomSaveRequest, Room>(request);
            room.CreatedBy = int.Parse(userClaimService.GetUserId());//loggedInUser
            try
            {
                var result = await roomBLL.CreateOrUpdate(room);
                if (request.Id == 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.Created, "Room Created Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Room Already Exists.");
                    }
                }
                else if (request.Id > 0)
                {
                    if (result == 0)
                    {
                        bLLResponse = CreateSuccessResponse(result, HttpStatusCode.OK, "Room Updated Successfully");
                    }
                    else if (result == 1)
                    {
                        bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "Room Already Exists");
                    }
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.InternalServerError, "An Error Occurred while Updating Room");
                }
            }
            catch (Exception ex)
            {
                //log the error
            }

            return bLLResponse;
        }

        #endregion

        #region GetBYId

        /// <summary>
        /// Read Room By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Route("GetById")]
        [HttpGet]
        public async Task<BLLResponse> GetById(int id)
        {
            BLLResponse bLLResponse = null;

            try
            {
                var result = await roomBLL.GetById(id);
                if (result!=null)
                {
                    var roomResponse = mapper.Map<Room,RoomResponse>(result);
                    bLLResponse = CreateSuccessResponse(roomResponse, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "No records found");
                }
            }
            catch (Exception ex)
            {
                //log the error
            }

            return bLLResponse;
        }

        #endregion

        #region GetList

        /// <summary>
        /// Read Room List With Pagination ,search and sorting
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
                var result = await roomBLL.GetList(sortWithPageParm);
                if (result.Rooms.Count > 0)
                {
                    var response = mapper.Map<RoomList,RoomListResponse>(result);
                    bLLResponse = CreateSuccessResponse(response, HttpStatusCode.OK);
                }
                else
                {
                    bLLResponse = CreateFailResponse(null, HttpStatusCode.NotFound, "Rooms not exist");
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

