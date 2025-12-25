using DAL.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IProfileDAL
    {
        Task<int> UploadImage(string fileName, int loggedInUserId);
        Task<int> ChangePassword(string oldPassword, string newPassword, int loggedInUserId);
    }
    public class ProfileDAL : BaseDAL, IProfileDAL
    {
        public ProfileDAL(IConfiguration configuration) : base(configuration) { }

        #region UploadImage

        /// <summary>
        /// Upload Image Path
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public async Task<int> UploadImage(string imagePath, int loggedInUserId)
        {
            int retVal = -1;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@ImagePath",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=false,
                            Value = imagePath,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@LoggedInUserId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = loggedInUserId,
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
                    command.CommandText = "sp_Staff_UploadStaffImage";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);

                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnParameter.Value;
                }
            }

            return retVal;
        }

        #endregion

        #region ChangePassword

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public async Task<int> ChangePassword(string oldPassword,string newPassword, int loggedInUserId)
        {
            int retVal = -1;
            var parms = new SqlParameter[]
                    {
                        new SqlParameter(){
                            ParameterName ="@OldPassword",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=false,
                            Value = oldPassword,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@NewPassword",
                            SqlDbType = SqlDbType.NVarChar,
                            IsNullable=false,
                            Value = newPassword,
                            Direction = ParameterDirection.Input,
                        },
                        new SqlParameter(){
                            ParameterName ="@LoggedInUserId",
                            SqlDbType = SqlDbType.Int,
                            IsNullable=true,
                            Value = loggedInUserId,
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
                    command.CommandText = "sp_Staff_ChangePassword";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parms);

                    command.Parameters.Add(returnParameter);
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnParameter.Value;
                }
            }

            return retVal;
        }

        #endregion
    }
}
