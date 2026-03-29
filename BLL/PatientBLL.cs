using DAL;
using DAL.Entity;

namespace BLL
{
    public interface IPatientBLL
    {
        Task<int> CreateOrUpdate(Patient patient);
        Task<Patient> GetById(int id);
        Task<PatientList> GetList(SortWithPageParameters sortWithPageParameters = null);
    }
    public class PatientBLL : IPatientBLL
    {
        public readonly IPatientDAL patientDAL;
        public PatientBLL(IPatientDAL patientDAL)
        {
            this.patientDAL = patientDAL;
        }

        #region CreateOrUpdate

        /// <summary>
        /// 
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        public async Task<int> CreateOrUpdate(Patient patient)
        {
            return await patientDAL.CreateOrUpdate(patient);
        }

        #endregion

        #region GetById

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Patient> GetById(int id)
        {
            return await patientDAL.GetById(id);
        }

        #endregion

        #region GetById

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PatientList> GetList(SortWithPageParameters sortWithPageParameters=null)
        {
            return await patientDAL.GetList(sortWithPageParameters);
        }

        #endregion
    }
}
