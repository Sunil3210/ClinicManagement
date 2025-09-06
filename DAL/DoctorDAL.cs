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
    public interface IDoctorDAL
    {
        Task<int> CreateOrUpdate(Doctor doctor);
        Task<Doctor> GetById(int id);
        Task<DoctorList> GetList(SortWithPageParameters sortWithPageParameters = null);
    }
    public class DoctorDAL : BaseDAL, IDoctorDAL
    {
        public DoctorDAL(IConfiguration configuration) : base(configuration) { }

        #region CreateOrUpdate
        public async Task<int> CreateOrUpdate(Doctor doctor)
        {
            int newId = 0;
            int retVal = -1;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@Name",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = doctor.Name,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Specialization",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = doctor.Specialization,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@DepartmentId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = doctor.DepartmentId,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Email",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = doctor.Email,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Phone",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = doctor.Phone,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@CreatedBy",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = doctor.CreatedBy,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@ModifiedBy",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = doctor.ModifiedBy,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Active",
                            SqlDbType = SqlDbType.Bit,
                            IsNullable=true,
                            Value = doctor.Active,
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
                    command.CommandText = "sp_Doctor_CreateOrUpdate";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);
                    if (doctor.Id > 0)
                    {
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@Id",
                            SqlDbType = SqlDbType.Int,
                            IsNullable = true,
                            Value = doctor.Id,
                            Direction = ParameterDirection.Input,
                        });
                    }
                    command.Parameters.Add(outPutParameter);
                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnParameter.Value;
                    if (retVal == 0 && doctor.Id == 0)
                    {
                        newId = (int)outPutParameter.Value;
                    }
                }
            }
            return retVal;
        }
        #endregion

        #region GetById
        public async Task<Doctor> GetById(int id)
        {
            Doctor doctor = null;
            

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Doctor_GetById";
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
                        doctor = new Doctor()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                            Specialization = dataReader["Specialization"].ToString(),
                            DepartmentId = Convert.ToInt32(dataReader["DepartmentId"]),
                            Email = dataReader["Email"].ToString(),
                            Phone = dataReader["Phone"].ToString(),
                            CreatedBy = Convert.ToInt32(dataReader["CreatedBy"]),
                            CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]),
                            ModifiedBy = dataReader["ModifiedBy"] == DBNull.Value ? null : Convert.ToInt32(dataReader["ModifiedBy"]),
                            ModifiedDate = dataReader["ModifiedDate"] == DBNull.Value ? null : Convert.ToDateTime(dataReader["ModifiedDate"]),
                            Active = Convert.ToBoolean(dataReader["Active"]),
                        };
                    }
                }
            }
            return doctor;
            
        }
        #endregion

        #region GetList
        public async Task<DoctorList> GetList(SortWithPageParameters sortWithPageParameters = null)
        {
            DoctorList doctorList = new DoctorList();
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
                    command.CommandText = "sp_Doctor_GetList";
                    command.Parameters.AddRange(parms);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbConnection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        Doctor doctor = new Doctor()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                            Specialization = dataReader["Specialization"].ToString(),
                            Email = dataReader["Email"].ToString(),
                            Phone = dataReader["Phone"].ToString(),
                            DepartmentName = dataReader["DepartmentName"].ToString(),
                        };
                        doctorList.Doctors.Add(doctor);
                    }
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            doctorList.TotalCount = Convert.ToInt32(dataReader["TotalCount"]);
                        }
                    }
                }
            }

            return doctorList;
        }
        #endregion
    }
}
