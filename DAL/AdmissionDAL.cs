using DAL.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IAdmissionDAL
    {
        Task<int> CreateOrUpdate(Admission admission);
        Task<Admission> GetById(int Id);
        Task<AdmissionList> GetList(SortWithPageParameters sortWithPageParameters = null);
    }
    public class AdmissionDAL : BaseDAL, IAdmissionDAL
    {
        public AdmissionDAL(IConfiguration configuration) : base(configuration){}

        #region CreateOrUpdate
        public async Task<int> CreateOrUpdate(Admission admission)
        {
            int newId = 0;
            int retVal = -1;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@PatientId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = admission.PatientId,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@RoomId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = admission.RoomId,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@DischargeDate",
                            SqlDbType = SqlDbType.Date,
                            IsNullable=true,
                            Value = admission.DischargeDate,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@CreatedBy",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = admission.CreatedBy,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@ModifiedBy",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = admission.ModifiedBy,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Active",
                            SqlDbType = SqlDbType.Bit,
                            IsNullable=true,
                            Value = admission.Active,
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
                    command.CommandText = "sp_Admission_CreateOrUpdate";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);
                    if (admission.Id > 0)
                    {
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@Id",
                            SqlDbType = SqlDbType.Int,
                            IsNullable = true,
                            Value = admission.Id,
                            Direction = ParameterDirection.Input,
                        });
                    }
                    command.Parameters.Add(outPutParameter);
                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnParameter.Value;
                    if (retVal == 0 && admission.Id == 0)
                    {
                        newId = (int)outPutParameter.Value;
                    }
                }
            }

            return retVal;

        }
        #endregion

        #region GetById
        public async Task<Admission> GetById(int id)
        {
            Admission admission = null;
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Admission_GetById";
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
                        admission = new Admission()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            PatientId = Convert.ToInt32(dataReader["PatientId"]),
                            RoomId = Convert.ToInt32(dataReader["RoomId"]),
                            AdmissionDate = Convert.ToDateTime(dataReader["AdmissionDate"]),
                            DischargeDate = dataReader["DischargeDate"] == DBNull.Value ? null : Convert.ToDateTime(dataReader["DischargeDate"]),
                            CreatedBy = Convert.ToInt32(dataReader["CreatedBy"]),
                            CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]),
                            ModifiedBy = dataReader["ModifiedBy"] == DBNull.Value ? null : Convert.ToInt32(dataReader["ModifiedBy"]),
                            ModifiedDate = dataReader["ModifiedDate"] == DBNull.Value ? null : Convert.ToDateTime(dataReader["ModifiedDate"]),
                            Active = Convert.ToBoolean(dataReader["Active"]),
                        };
                    }
                }
            }
            return admission;

        }
        #endregion

        #region GetList
        public async Task<AdmissionList> GetList(SortWithPageParameters sortWithPageParameters)
        {
            AdmissionList admissionList = new AdmissionList();
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
                    command.CommandText = "sp_Admission_GetList";
                    command.Parameters.AddRange(parms);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbConnection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        Admission admission = new Admission()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            PatientId = Convert.ToInt32(dataReader["PatientId"]),
                            Name = dataReader["Name"].ToString(),
                            RoomNumber = dataReader["RoomNumber"].ToString(),
                            RoomType = dataReader["RoomType"].ToString(),
                            Phone = dataReader["Phone"] == DBNull.Value ? null : dataReader["Phone"].ToString(),
                            Gender = Convert.ToChar(dataReader["Gender"]),
                            Age = Convert.ToInt32(dataReader["Age"])
                        };
                        admissionList.admissions.Add(admission);
                    }
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            admissionList.TotalCount = Convert.ToInt32(dataReader["TotalCount"]);
                        }
                    }
                }
            }

            return admissionList;
        }
        #endregion

    }
}
