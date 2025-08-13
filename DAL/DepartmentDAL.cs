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
    public interface IDepartmentDAL
    {
        Task<int> Create(Department department);
        Task<int> Update(Department department);
        Task<Department> GetById(int id);
        Task<DepartmentList> GetList(SortWithPageParameters sortWithPageParameters = null);
    }
    public class DepartmentDAL : BaseDAL, IDepartmentDAL
    {
        public DepartmentDAL(IConfiguration configuration) : base(configuration) { }

        #region Create

        /// <summary>
        /// Create Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public async Task<int> Create(Department department)
        {
            int newId = 0;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@Name",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = department.Name,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Id",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = department.Id>0?department.Id:null,
                            Direction = ParameterDirection.Input,
                        }
                    };
            var outPutParameter = new SqlParameter()
            {
                ParameterName = "@NewId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Department_CreateOrUpdate";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);
                    command.Parameters.Add(outPutParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                }
            }
            return newId;

        }

        #endregion

        #region Update

        /// <summary>
        /// Update Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public async Task<int> Update(Department department)
        {
            int retVal = -1;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@Name",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = department.Name,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Id",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = department.Id>0?department.Id:null,
                            Direction = ParameterDirection.Input,
                        }
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
                    command.CommandText = "sp_Department_CreateOrUpdate";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);
                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
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
        public async Task<Department> GetById(int id)
        {
            Department department = null;

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Department_GetById";
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
                        department = new Department()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                        };
                    }
                }
            }
            return department;
        }

        #endregion

        #region GetList

        public async Task<DepartmentList> GetList(SortWithPageParameters sortWithPageParameters = null)
        {
            DepartmentList departmentList = new DepartmentList();
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
                            SqlDbType = SqlDbType.Int,
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
                    command.CommandText = "sp_Department_GetList";
                    command.Parameters.AddRange(parms);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbConnection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        Department department = new Department()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                        };
                        departmentList.Departments.Add(department);
                    }
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            departmentList.TotalCount = Convert.ToInt32(dataReader["TotalCount"]);
                        }
                    }
                }
            }

            return departmentList;
        }

        #endregion
    }
}
