using DAL;
using DAL.Entity;

namespace BLL
{
    public interface IStaffBLL
    {
        Task<int> CreateOrUpdate(Staff staff);
        Task<Staff> GetById(int id);
        Task<StaffList> GetList(SortWithPageParameters sortWithPageParameters = null);
        Task<Staff> Validate(string email, string password);
    }
    public class StaffBLL : IStaffBLL
    {
        public readonly IStaffDAL staffDAL;

        public StaffBLL(IStaffDAL staffDAL)
        {
            this.staffDAL = staffDAL;
        }

        #region CreateOrUpdate

        /// <summary>
        /// 
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        public Task<int> CreateOrUpdate(Staff staff)
        {
            return staffDAL.CreateOrUpdate(staff);
        }

        #endregion

        #region GetById

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Staff> GetById(int id)
        {
            return staffDAL.GetById(id);
        }

        #endregion

        #region GetList

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortWithPageParameters"></param>
        /// <returns></returns>
        public Task<StaffList> GetList(SortWithPageParameters sortWithPageParameters)
        {
            return staffDAL.GetList(sortWithPageParameters);
        }

        #endregion

        #region GetById

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Staff> Validate(string email,string password)
        {
            return staffDAL.Validate(email,password);
        }

        #endregion
    }
}
