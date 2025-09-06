using DAL.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IStaffDAL
    {
        Task<int> CreateOrUpdate(Staff staff);
        Task<Staff> GetById(int id);
        Task<StaffList> GetList(SortWithPageParameters sortWithPageParameters = null);
        Task<Staff> Validate(string email, string password);
    }
    public class StaffDAL : BaseDAL, IStaffDAL
    {
        public StaffDAL(IConfiguration configuration) : base(configuration) { }

        #region Create

        /// <summary>
        /// Create Staff
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        public async Task<int> CreateOrUpdate(Staff staff)
        {
            int newId = 0;
            int retVal = -1;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@Name",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = staff.Name,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Email",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = staff.Email,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Password",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = staff.Password,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Role",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = staff.Role,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@LoggedInUserId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = staff.CreatedBy,
                            Direction = ParameterDirection.Input,
                        }
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
                    command.CommandText = "sp_Staff_CreateOrUpdate";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);
                    if (staff.Id > 0)
                    {
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@Id",
                            SqlDbType = SqlDbType.Int,
                            IsNullable = true,
                            Value = staff.Id,
                            Direction = ParameterDirection.Input,
                        });
                    }
                    command.Parameters.Add(outPutParameter);
                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnParameter.Value;
                    if (retVal == 0 && staff.Id == 0)
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
        /// Get Department By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Staff> GetById(int id)
        {
            Staff staff = null;

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Staff_GetById";
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = id,
                        IsNullable = false
                    });
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        staff = new Staff()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                            Email = dataReader["email"].ToString(),
                            Role = dataReader["Role"].ToString()
                        };
                    }
                }
            }
            return staff;
        }

        #endregion

        #region GetList

        public async Task<StaffList> GetList(SortWithPageParameters sortWithPageParameters = null)
        {
            StaffList staffList = new StaffList();
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
                    command.CommandText = "sp_Staff_GetList";
                    command.Parameters.AddRange(parms);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbConnection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        Staff department = new Staff()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                            Email = dataReader["email"].ToString(),
                            Role = dataReader["Role"].ToString()
                        };
                        staffList.Staffs.Add(department);
                    }
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            staffList.TotalCount = Convert.ToInt32(dataReader["TotalCount"]);
                        }
                    }
                }
            }

            return staffList;
        }

        #endregion

        #region GetById

        /// <summary>
        /// Get Department By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Staff> Validate(string email, string password)
        {
            Staff staff = null;

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Staff_Validate";
                    var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@Email",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = email,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Password",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = password,
                            Direction = ParameterDirection.Input,
                        }
                    };
                    command.Parameters.AddRange(parms);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        staff = new Staff()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                            Email = dataReader["email"].ToString(),
                            Role = dataReader["Role"].ToString()
                        };
                    }
                }
            }
            return staff;
        }

        #endregion
    }
}
