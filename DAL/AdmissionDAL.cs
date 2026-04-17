using DAL.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
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
                            ParameterName ="@AdmissionDate",
                            SqlDbType = SqlDbType.Date,
                            IsNullable=true,
                            Value = admission.AdmissionDate,
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
                            ParameterName ="@LoggedInUserId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = admission.CreatedBy,
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
                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnParameter.Value;
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
                            //PatientName=Convert.ToString(dataReader["PatientName"]),
                            RoomId = Convert.ToInt32(dataReader["RoomId"]),
                            //RoomNumber = Convert.ToString(dataReader["RoomNumber"]),
                            AdmissionDate = dataReader["AdmissionDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(dataReader["AdmissionDate"])) : null,
                            DischargeDate = dataReader["DischargeDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(dataReader["DischargeDate"])) : null,
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
                            PatientName = dataReader["PatientName"].ToString(),
                            RoomNumber = dataReader["RoomNumber"].ToString(),
                            RoomId = Convert.ToInt32(dataReader["RoomId"]),
                            AdmissionDate = dataReader["AdmissionDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(dataReader["AdmissionDate"])) : null,
                            DischargeDate = dataReader["DischargeDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(dataReader["DischargeDate"])) : null,
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
