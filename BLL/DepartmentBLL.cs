using DAL;
using DAL.Entity;

namespace BLL
{
    public interface IDepartmentBLL
    {
        Task<int> CreateOrUpdate(Department department);
        Task<Department> GetById(int id);
        Task<DepartmentList> GetList(SortWithPageParameters sortWithPageParameters);
    }
    public class DepartmentBLL : IDepartmentBLL
    {
        public readonly IDepartmentDAL departmentDAL;
        public DepartmentBLL(IDepartmentDAL departmentDAL)
        {
            this.departmentDAL = departmentDAL;
        }


        #region CreateOrUpdate

        /// <summary>
        /// 
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public Task<int> CreateOrUpdate(Department department)
        {
            return departmentDAL.CreateOrUpdate(department);
        }

        #endregion

        #region GetById

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Department> GetById(int id)
        {
            return departmentDAL.GetById(id);
        }

        #endregion

        #region GetList

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortWithPageParameters"></param>
        /// <returns></returns>
        public Task<DepartmentList> GetList(SortWithPageParameters sortWithPageParameters)
        {
            return departmentDAL.GetList(sortWithPageParameters);
        }

        #endregion
    }
}
