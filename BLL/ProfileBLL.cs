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
        Task<int> ChangePassword(string oldPassword, string newPassword, int loggedInUserId);
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
        
        public async Task<int> ChangePassword(string oldPassword, string newPassword, int loggedInUserId)
        {
            return await profileDAL.ChangePassword(oldPassword, newPassword, loggedInUserId);
        }
    }
}
