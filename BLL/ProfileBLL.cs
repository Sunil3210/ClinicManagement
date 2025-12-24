using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IProfileBLL
    {
        Task<int> UploadImage(string imagePath, int loggedInUserId);
    }
    public class ProfileBLL : IProfileBLL
    {
        public readonly IProfileDAL profileDAL;

        public ProfileBLL(IProfileDAL profileDAL)
        {
            this.profileDAL = profileDAL;
        }

        public async Task<int> UploadImage(string fileName, int loggedInUserId)
        {
            return await profileDAL.UploadImage(fileName, loggedInUserId);
        }
    }
}
