using DAL;
using DAL.Entity;

namespace BLL
{
    public interface IAdmissionBLL
    {
        Task<int> CreateOrUpdate(Admission admission);
        Task<Admission> GetById(int id);
        Task<AdmissionList> GetList(SortWithPageParameters sortWithPageParameters);
    }
    public class AdmissionBLL:IAdmissionBLL
    {
        public readonly IAdmissionDAL admissionDAL;
        public AdmissionBLL(IAdmissionDAL admissionDAL)
        {
            this.admissionDAL = admissionDAL;
        }

        #region CreateOrUpdate
        public Task<int> CreateOrUpdate(Admission admission)
        {
            return admissionDAL.CreateOrUpdate(admission);
        }
        #endregion

        #region GetById
        public Task<Admission> GetById(int id)
        {
            return admissionDAL.GetById(id);
        }
        #endregion

        #region GetList
        public Task<AdmissionList> GetList(SortWithPageParameters sortWithPageParameters)
        {
            return admissionDAL.GetList(sortWithPageParameters);
        }
        #endregion
    }
}
