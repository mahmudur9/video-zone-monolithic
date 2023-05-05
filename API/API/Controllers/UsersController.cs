using API.Domain.Interfaces;
using API.Domain.Models;
using API.Domain.ViewModels;
using API.DTOs;
using API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IAuthService _auth;

        public UsersController(IUnitOfWork unit, IAuthService auth)
        {
            _unit = unit;
            _auth = auth;
        }

        [HttpGet("get/{id}"), AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _unit.User.GetUserByIdAsync(id);

            if (user == null)
            {
                return BadRequest(new { error = "The user was not found!" });
            }

            var videoList = new List<VideoView>();
            foreach (var video in user.Videos!)
            {
                videoList.Add(new VideoView()
                {
                    Id = video.Id,
                    Title = video.Title,
                    Description = video.Description,
                    Url = video.Url,
                    ThumbnailUrl = video.ThumbnailUrl,
                    PostedById = video.PostedById,
                    PostedBy = video.PostedBy,
                    VideoCategoryId = video.VideoCategoryId,
                    VideoCategory = video.VideoCategory,
                    Likes = video.Likes,
                    Dislikes = video.Dislikes,
                    Comments = video.Comments,
                    ViewCounts = video.ViewCounts,
                    CreatedAt = DateProcess.GetDate(video.CreatedAt),
                    UpdatedAt = DateProcess.GetDate(video.CreatedAt)
                });
            }
            var userView = new UserView()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserImage = user.UserImage,
                Type = user.Type,
                Videos = videoList,
                Likes = user.Likes,
                Dislikes = user.Dislikes,
                Comments = user.Comments,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(new { user = userView });
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register(User user)
        {
            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { error = "The password field can not be empty!" });
            }

            if (await _unit.User.GetUserByEmailAsync(user.Email!) != null)
            {
                return BadRequest(new { error = "A user already exists by this email!" });
            }

            user.Password = Encryption.CreateHash(user.Password!);
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            user.Type = Constants.RoleUser;
            if (!await _unit.User.CreateAsync(user))
            {
                return BadRequest(new { error = "Getting trouble creating user!" });
            }

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(UserCredentialDTO userCredential)
        {
            var user = await _unit.User.GetUserByEmailAsync(userCredential.Email!);

            if (user == null)
            {
                return BadRequest(new { error = "Wrong email or password!" });
            }

            if (!Encryption.VerifyHash(SHA256.Create(), userCredential.Password!, user.Password!))
            {
                return BadRequest(new { error = "Wrong email or password!" });
            }

            var token = _auth.GenerateToken(user.Id, user.Email!, user.Type!);

            return Ok(new { token });
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.Claims.FirstOrDefault(x => x.Type.Equals("userId"))!.Value;

            var user = await _unit.User.GetUserByIdAsync(Guid.Parse(userId));

            if (user == null)
            {
                return BadRequest(new { error = "The user was not found!" });
            }

            var videoList = new List<VideoView>();
            foreach (var video in user.Videos!)
            {
                videoList.Add(new VideoView()
                {
                    Id = video.Id,
                    Title = video.Title,
                    Description = video.Description,
                    Url = video.Url,
                    ThumbnailUrl = video.ThumbnailUrl,
                    PostedById = video.PostedById,
                    PostedBy = video.PostedBy,
                    VideoCategoryId = video.VideoCategoryId,
                    VideoCategory = video.VideoCategory,
                    Likes = video.Likes,
                    Dislikes = video.Dislikes,
                    Comments = video.Comments,
                    ViewCounts = video.ViewCounts,
                    CreatedAt = DateProcess.GetDate(video.CreatedAt),
                    UpdatedAt = DateProcess.GetDate(video.CreatedAt)
                });
            }
            var userView = new UserView()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserImage = user.UserImage,
                Type = user.Type,
                Videos = videoList,
                Likes = user.Likes,
                Dislikes = user.Dislikes,
                Comments = user.Comments,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(new { user = userView });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] User user)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity!.FindFirst(ClaimTypes.Role)?.Value;
            var userId = claimsIdentity!.Claims.FirstOrDefault(x => x.Type.Equals("userId"))!.Value;

            var getUser = await _unit.User.GetByIdAsync(Guid.Parse(userId));

            if (getUser == null)
            {
                return BadRequest(new { error = "The user was not found!" });
            }

            getUser.Name = user.Name;

            // Validate email
            var getUserByEmail = await _unit.User.GetUserByEmailAsync(user.Email!);

            if (getUserByEmail != null)
            {
                if (!getUserByEmail.Id.Equals(Guid.Parse(userId)))
                {
                    return BadRequest(new { error = "A user already exists by this email!" });
                }
            }

            getUser.Email = user.Email;

            if (user.Password != null)
            {
                getUser.Password = Encryption.CreateHash(user.Password);
            }

            // Upload image
            if (user.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(getUser.UserImage))
                {
                    // delete start
                    string PreviousImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", getUser.UserImage);
                    var fileInfo = new FileInfo(PreviousImage);
                    fileInfo.Delete();
                    // delete end
                }
                // upload image
                var file = user.ImageFile;
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ImageName);

                FileProcess.ResizeAndUpload(SavePath, file);

                getUser.UserImage = ImageName;
                // End image upload
            }

            getUser.UpdatedAt = DateTime.Now;

            if (!await _unit.User.UpdateAsync(getUser))
            {
                return BadRequest(new { error = "Getting trouble creating user!" });
            }

            return Ok(new { message = "User updated successfully!" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _unit.User.GetByIdAsync(id);

            if (user == null)
            {
                return BadRequest(new { error = "The user was not found!" });
            }

            if (!string.IsNullOrEmpty(user.UserImage))
            {
                // delete start
                string PreviousImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", user.UserImage);
                var fileInfo = new FileInfo(PreviousImage);
                fileInfo.Delete();
                // delete end
            }

            if (!await _unit.User.DeleteAsync(user))
            {
                return BadRequest(new { error = "Getting trouble deleting the user!" });
            }

            return Ok(new { message = "User deleted successfully!" });
        }
    }
}
