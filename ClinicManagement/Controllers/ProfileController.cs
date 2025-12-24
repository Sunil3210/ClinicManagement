using BLL;
using ClinicManagement.Extension;
using ClinicManagement.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        public readonly IProfileBLL profileBLL;
        public readonly IUserClaimService userClaimService;

        public ProfileController(IWebHostEnvironment env, IProfileBLL profileBLL, IUserClaimService userClaimService)
        {
            _env = env;
            this.profileBLL = profileBLL;
            this.userClaimService = userClaimService;
        }

        #region UploadImage

        /// <summary>
        /// Upload Profile Image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>

        [Route("upload-image")]
        [HttpPost]
        public async Task<BLLResponse> UploadImage(IFormFile file)
        {
            BLLResponse bllResponse = null;

            if (file == null || file.Length == 0)
            {
                bllResponse = CreateFailResponse(null, HttpStatusCode.BadRequest, "Please Upload Image");
                return bllResponse;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                bllResponse = CreateFailResponse(null, HttpStatusCode.BadRequest, "Invalid image format");
                return bllResponse;
            }

            // Create folder if not exists
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "profiles");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // Unique file name
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadFolder, fileName);

            // Save image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Image URL
            var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/profiles/{fileName}";
            var response = await profileBLL.UploadImage(fileName, int.Parse(userClaimService.GetUserId()));
            bllResponse = CreateSuccessResponse(new
            {
                FileName = fileName,
                ImageUrl = imageUrl
            }, HttpStatusCode.OK, "Image Uploaded Successfully");
            return bllResponse;
        }

        #endregion
    }
}
