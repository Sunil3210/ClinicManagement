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
    public interface ILookupDAL
    {
        Task<List<SelectItem>> GetDepartments();
        Task<List<SelectItem>> GetDoctors();
    }
    public class LookupDAL:BaseDAL,ILookupDAL
    {
        public LookupDAL(IConfiguration configuration) : base(configuration) { }

        #region GetDepartments

        /// <summary>
        /// Get Lookup Data for departments
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectItem>> GetDepartments()
        {
            List<SelectItem> selectItems = new List<SelectItem>();
            
            using (var dbConnection = CreateConnection())
            {
                using (var command = dbConnection.CreateCommand())
                {
                    dbConnection.Open();
                    command.CommandText = "sp_Lkp_Departments";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbConnection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        SelectItem item = new SelectItem()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                        };
                        selectItems.Add(item);
                    }
                }
            }

            return selectItems;
        }
        #endregion

        #region GetDepartments

        /// <summary>
        /// Get Lookup Data for departments
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectItem>> GetDoctors()
        {
            List<SelectItem> selectItems = new List<SelectItem>();

            using (var dbConnection = CreateConnection())
            {
                using (var command = dbConnection.CreateCommand())
                {
                    dbConnection.Open();
                    command.CommandText = "sp_Lkp_Doctors";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbConnection;
                    var dataReader = await command.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        SelectItem item = new SelectItem()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                        };
                        selectItems.Add(item);
                    }
                }
            }

            return selectItems;
        }
        #endregion
    }
}
