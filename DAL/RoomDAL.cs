using DAL.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DAL
{
    public interface IRoomDAL
    {
        Task<int> CreateOrUpdate(Room room);
        Task<Room> GetById(int id);
        Task<RoomList> GetList(SortWithPageParameters sortWithPageParameters = null);
    }
    public class RoomDAL : BaseDAL, IRoomDAL
    {
        public RoomDAL(IConfiguration configuration) : base(configuration) { }


        #region CreateOrUpdate

        /// <summary>
        /// Create or update the room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task<int> CreateOrUpdate(Room room)
        {
            int newId = 0;
            int retVal = -1;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@RoomNumber",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = room.RoomNumber,
                            Direction = ParameterDirection.Input,
                        },

                        new SqlParameter(){
                            ParameterName ="@TypeId",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = room.TypeId,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Status",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = room.Status,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@LoggedInUserId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = room.CreatedBy,
                            Direction = ParameterDirection.Input,
                        },


                    };
            var outPutParameter = new SqlParameter()
            {
                ParameterName = "@NewId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };
            var returnParameter = new SqlParameter()
            {
                ParameterName = "@ReturnVal",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue,
            };
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Room_Save";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);
                    if (room.Id > 0)
                    {
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@Id",
                            IsNullable = true,
                            Value = room.Id,
                            Direction = ParameterDirection.Input,
                        });
                    }
                    command.Parameters.Add(outPutParameter);
                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnParameter.Value;
                    if (retVal == 0 && room.Id == 0)
                    {
                        newId = (int)outPutParameter.Value;
                    }
                }
            }
            return retVal;
        }

        #endregion

        #region GetById

        /// <summary>
        /// Get Room By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Room> GetById(int id)
        {
            Room room = null;

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Room_GetById";
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = id,
                        IsNullable = false
                    });
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        room = new Room()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            RoomNumber = dataReader["RoomNumber"].ToString(),
                            TypeId = Convert.ToInt32(dataReader["TypeId"]),
                            Status = Convert.ToString(dataReader["Status"])
                        };
                    }
                }
            }
            return room;
        }

        #endregion

        #region GetList

        public async Task<RoomList> GetList(SortWithPageParameters sortWithPageParameters = null)
        {
            RoomList roomList = new RoomList();
            if (sortWithPageParameters == null)
            {
                sortWithPageParameters = new SortWithPageParameters();
            }
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@SortParameter",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = sortWithPageParameters.SortParameter,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@SortDirection",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = sortWithPageParameters.SortDirection,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@PageNum",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = sortWithPageParameters.PageNumber.HasValue ? sortWithPageParameters.PageNumber : 1,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@PageSize",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = sortWithPageParameters.PageSize.HasValue ? sortWithPageParameters.PageSize : 10,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Search",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = sortWithPageParameters.SearchString,
                            Direction = ParameterDirection.Input,
                        }
                    };

            using (var dbConnection = CreateConnection())
            {
                using (var command = dbConnection.CreateCommand())
                {
                    dbConnection.Open();
                    command.CommandText = "sp_Room_GetList";
                    command.Parameters.AddRange(parms);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbConnection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        Room room = new Room()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            RoomNumber = dataReader["RoomNumber"].ToString(),
                            //TypeId = Convert.ToInt32(dataReader["TypeId"]),
                            RoomType = Convert.ToString(dataReader["RoomType"]),
                            Status = Convert.ToString(dataReader["Status"])
                        };
                        roomList.Rooms.Add(room);
                    }
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            roomList.TotalCount = Convert.ToInt32(dataReader["TotalCount"]);
                        }
                    }
                }
            }

            return roomList;
        }

        #endregion
    }

}
