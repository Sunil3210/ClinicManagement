using DAL;
using DAL.Entity;

namespace BLL
{
    public interface IDepartmentBLL
    {
        Task<int> Create(Department department);
        Task<int> Update(Department department);
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


        #region Create
        /// <summary>
        /// 
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public Task<int> Create(Department department)
        {
            return departmentDAL.Create(department);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public Task<int> Update(Department department)
        {
            return departmentDAL.Update(department);
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
