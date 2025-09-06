using DAL;
using DAL.Entity;

namespace BLL
{
    public interface IDoctorBLL
    {
        Task<int> CreateOrUpdate(Doctor doctor);
        Task<Doctor> GetById(int id);
        Task<DoctorList> GetList(SortWithPageParameters sortWithPageParameters);
    }
    public class DoctorBLL : IDoctorBLL
    {
        public readonly IDoctorDAL doctorDAL;
        public DoctorBLL(IDoctorDAL doctorDAL)
        {
            this.doctorDAL = doctorDAL;
        }


        #region CreateOrUpdate

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns></returns>
        public Task<int> CreateOrUpdate(Doctor doctor)
        {
            return doctorDAL.CreateOrUpdate(doctor);
        }

        #endregion

        #region GetById

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Doctor> GetById(int id)
        {
            return doctorDAL.GetById(id);
        }

        #endregion

        #region GetList

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortWithPageParameters"></param>
        /// <returns></returns>
        public Task<DoctorList> GetList(SortWithPageParameters sortWithPageParameters)
        {
            return doctorDAL.GetList(sortWithPageParameters);
        }

        #endregion
    }
}
