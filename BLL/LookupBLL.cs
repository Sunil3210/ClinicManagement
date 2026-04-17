using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ILookupBLL 
    {
        Task<List<SelectItem>> GetDepartments();
        Task<List<SelectItem>> GetDoctors();
        Task<List<SelectItem>> GetRoomType();
        Task<List<SelectItem>> GetAvailableRoomsByType(int? typeId);
    }

    public class LookupBLL: ILookupBLL
    {
        public readonly ILookupDAL lookupDAL;
        public LookupBLL(ILookupDAL lookupDAL)
        {
            this.lookupDAL = lookupDAL;
        }

        #region GetDepartments

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public Task<List<SelectItem>> GetDepartments()
        {
            return lookupDAL.GetDepartments();
        }

        #endregion

        #region GetDoctors

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<List<SelectItem>> GetDoctors()
        {
            return lookupDAL.GetDoctors();
        }

        #endregion

        #region GetRoomType

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectItem>> GetRoomType()
        {
            return await lookupDAL.GetRoomType();
        }

        #endregion

        #region GetAvailableRoomsByType

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectItem>> GetAvailableRoomsByType(int? typeId)
        {
            return await lookupDAL.GetAvailableRoomsByType(typeId);
        }

        #endregion
    }
}
