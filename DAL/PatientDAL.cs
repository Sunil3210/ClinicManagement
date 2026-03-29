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
    public interface IPatientDAL
    {
        Task<int> CreateOrUpdate(Patient patient);
        Task<Patient> GetById(int id);
        Task<PatientList> GetList(SortWithPageParameters sortWithPageParameters = null);
    }

    public class PatientDAL : BaseDAL, IPatientDAL
    {
        public PatientDAL(IConfiguration configuration) : base(configuration) { }

        #region Create

        /// <summary>
        /// Create Patient
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        public async Task<int> CreateOrUpdate(Patient patient)
        {
            int newId = 0;
            int retVal = -1;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@Name",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = patient.Name,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Email",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = patient.Email,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Age",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = patient.Age,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Gender",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = patient.Gender,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Phone",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = patient.Phone,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@Address",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = patient.Address,
                            Direction = ParameterDirection.Input,
                        },new SqlParameter(){
                            ParameterName ="@Aadhar",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=true,
                            Value = patient.Aadhar,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@LoggedInUserId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = patient.CreatedBy,
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
                    command.CommandText = "sp_Patient_Save";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);
                    if (patient.Id > 0)
                    {
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@Id",
                            SqlDbType = SqlDbType.Int,
                            IsNullable = true,
                            Value = patient.Id,
                            Direction = ParameterDirection.Input,
                        });
                    }
                    command.Parameters.Add(outPutParameter);
                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnParameter.Value;
                    if (retVal == 0 && patient.Id == 0)
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
        /// Get Patient By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Patient> GetById(int id)
        {
            Patient patient = null;

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "sp_Patient_GetById";
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
                        patient = new Patient()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                            Age = Convert.ToInt32(dataReader["Age"]),
                            Gender =Convert.ToChar(dataReader["gender"]),
                            Address = dataReader["address"].ToString(),
                            Email = dataReader["email"].ToString(),
                            Aadhar= dataReader["aadhar"].ToString(),
                            Phone = dataReader["Phone"].ToString()
                        };
                    }
                }
            }
            return patient;
        }

        #endregion

        #region GetList

        public async Task<PatientList> GetList(SortWithPageParameters sortWithPageParameters = null)
        {
            PatientList patientList = new PatientList();
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
                    command.CommandText = "sp_Patient_GetList";
                    command.Parameters.AddRange(parms);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbConnection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        Patient patient = new Patient()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                            Email = dataReader["Email"].ToString(),
                            Age = Convert.ToInt32(dataReader["Age"]),
                            Phone = dataReader["Phone"].ToString(),
                            Gender = Convert.ToChar(dataReader["Gender"]),
                        };
                        patientList.Patients.Add(patient);
                    }
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            patientList.TotalCount = Convert.ToInt32(dataReader["TotalCount"]);
                        }
                    }
                }
            }

            return patientList;
        }

        #endregion
    }
}
