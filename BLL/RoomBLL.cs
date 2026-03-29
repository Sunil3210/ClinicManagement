using DAL;
using DAL.Entity;

namespace BLL
{

    public interface IRoomBLL
    {
        Task<int> CreateOrUpdate(Room room);
        Task<Room> GetById(int id);
        Task<RoomList> GetList(SortWithPageParameters sortWithPageParameters = null);
    }
    public class RoomBLL : IRoomBLL
    {
        public readonly IRoomDAL roomDAL;
        public RoomBLL(IRoomDAL roomDAL)
        {
            this.roomDAL = roomDAL;
        }

        #region CreateOrUpdate

        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task<int> CreateOrUpdate(Room room)
        {
            return await roomDAL.CreateOrUpdate(room);
        }

        #endregion

        #region GetById

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Room> GetById(int id)
        {
            return await roomDAL.GetById(id);
        }

        #endregion

        #region GetList

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortWithPageParameters"></param>
        /// <returns></returns>
        public async Task<RoomList> GetList(SortWithPageParameters sortWithPageParameters = null)
        {
            return await roomDAL.GetList(sortWithPageParameters);
        }

        #endregion

    }
}
