using API.Domain.Interfaces;
using API.Domain.Models;
using API.Domain.ViewModels;
using API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly ILogger<VideosController> _logger;
        private readonly IUnitOfWork _unit;

        public VideosController(IUnitOfWork unit, ILogger<VideosController> logger)
        {
            _unit = unit;
            _logger = logger;

        }

        [HttpGet("")]
        public async Task<IActionResult> VideoList()
        {
            var videos = await _unit.Video.VideoListAsync();

            var videoList = new List<VideoView>();
            foreach (var video in videos)
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

            return Ok(new { videos = videoList });
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var video = await _unit.Video.GetVideoByIdAsync(id);

            if (video == null)
            {
                return BadRequest(new { error = "The video was not found!" });
            }

            return Ok(new { video });
        }

        [HttpGet("watch")]
        public IActionResult Watch([FromQuery] string url)
        {
            try
            {
                var fileName = url;
                string videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", fileName);

                var filestream = System.IO.File.OpenRead(videoPath);
                return File(filestream, contentType: "video/mp4", fileDownloadName: fileName, enableRangeProcessing: true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something Went Wrong in the {nameof(Watch)}");

                return BadRequest();
            }
        }

        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] Video video)
        {
            if (video.VideoFile != null && video.ThumbnailFile != null && video.Title != null && video.Description != null)
            {
                //var extensionSplitArray = Path.GetExtension(video.VideoFile.FileName).Split(".");
                if (!Path.GetExtension(video.VideoFile.FileName).Split(".")[1].ToString().ToLower().Equals("mp4") && !Path.GetExtension(video.VideoFile.FileName).Split(".")[1].ToString().ToLower().Equals("avi") && !Path.GetExtension(video.VideoFile.FileName).Split(".")[1].ToString().ToLower().Equals("mkv"))
                {
                    return BadRequest(new { error = "Unsupported file!" });
                }

                if (!Path.GetExtension(video.ThumbnailFile.FileName).Split(".")[1].ToString().ToLower().Equals("jpg") && !Path.GetExtension(video.ThumbnailFile.FileName).Split(".")[1].ToString().ToLower().Equals("png"))
                {
                    return BadRequest(new { error = "Unsupported thumbnail file!" });
                }

                // Video upload start
                var file = video.VideoFile;
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", ImageName);

                await FileProcess.UploadVideoAsync(SavePath, file);

                video.Url = ImageName;
                // Video upload end

                // Thumbnail upload start
                var thumbnailFile = video.ThumbnailFile;
                string thumbnailName = Guid.NewGuid().ToString() + Path.GetExtension(thumbnailFile.FileName);
                string ThumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumbnails", thumbnailName);

                FileProcess.ResizeAndUpload(ThumbnailPath, thumbnailFile);

                video.ThumbnailUrl = thumbnailName;
                // Thumbnail upload end

                video.CreatedAt = DateTime.Now;
                video.UpdatedAt = DateTime.Now;
            }
            else
            {
                return BadRequest(new { error = "The fields can not be empty!" });
            }

            if (!await _unit.Video.CreateAsync(video))
            {
                return BadRequest(new { error = "Getting trouble creating video!" });
            }

            return Ok(new { message = "Video added successfully!" });
        }

        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] Video video)
        {
            var getVideo = await _unit.Video.GetByIdAsync(video.Id);
            if (getVideo == null)
            {
                return BadRequest(new { error = "The video was not found!" });
            }

            if (video.VideoFile != null)
            {
                if (!Path.GetExtension(video.VideoFile.FileName).Split(".")[1].ToString().ToLower().Equals("mp4") && !Path.GetExtension(video.VideoFile.FileName).Split(".")[1].ToString().ToLower().Equals("avi") && !Path.GetExtension(video.VideoFile.FileName).Split(".")[1].ToString().ToLower().Equals("mkv"))
                {
                    return BadRequest(new { error = "Unsupported file!" });
                }

                // Delete start
                string previousVideo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", video.Url!);
                var fileInfo = new FileInfo(previousVideo);
                fileInfo.Delete();
                // Delete end

                // Video upload
                var file = video.VideoFile;
                string VideoName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", VideoName);

                await FileProcess.UploadVideoAsync(SavePath, file);

                getVideo.Url = VideoName;
                // End video upload
            }

            if (video.ThumbnailFile != null)
            {
                if (!Path.GetExtension(video.ThumbnailFile.FileName).Split(".")[1].ToString().ToLower().Equals("jpg") && !Path.GetExtension(video.ThumbnailFile.FileName).Split(".")[1].ToString().ToLower().Equals("png"))
                {
                    return BadRequest(new { error = "Unsupported thumbnail file!" });
                }

                // Delete thumbnail start
                string previousThumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumbnails", video.ThumbnailUrl!);
                var thumbnailInfo = new FileInfo(previousThumbnail);
                thumbnailInfo.Delete();
                // Delete thumbnail end

                // Thumbnail upload start
                var thumbnailFile = video.ThumbnailFile;
                string thumbnailName = Guid.NewGuid().ToString() + Path.GetExtension(thumbnailFile.FileName);
                string ThumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumbnails", thumbnailName);

                FileProcess.ResizeAndUpload(ThumbnailPath, thumbnailFile);

                getVideo.ThumbnailUrl = thumbnailName;
                // Thumbnail upload end
            }

            getVideo.UpdatedAt = DateTime.Now;
            getVideo.Title = video.Title;
            getVideo.Description = video.Description;
            getVideo.VideoCategoryId = video.VideoCategoryId;

            if (!await _unit.Video.UpdateAsync(getVideo))
            {
                return BadRequest(new { error = "Getting trouble updating video!" });
            }

            return Ok(new { message = "Video updated successfully!" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var video = await _unit.Video.GetByIdAsync(id);
            if (video == null)
            {
                return BadRequest(new { error = "The video was not found!" });
            }
            if (!await _unit.Video.DeleteAsync(video))
            {
                return BadRequest(new { error = "Getting trouble deleting the video!" });
            }

            // Delete start
            string previousVideo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", video.Url!);
            var fileInfo = new FileInfo(previousVideo);
            fileInfo.Delete();
            // Delete end

            // Delete thumbnail start
            if (video.ThumbnailUrl != null)
            {
                string previousThumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumbnails", video.ThumbnailUrl);
                var thumbnailInfo = new FileInfo(previousThumbnail);
                thumbnailInfo.Delete();
            }
            // Delete thumbnail end

            return Ok(new { message = "Video deleted successfully!" });
        }

        [HttpPost("view/{id}")]
        public async Task<IActionResult> AddViewCount(Guid id)
        {
            var video = await _unit.Video.GetByIdAsync(id);
            if (video == null)
            {
                return BadRequest(new { error = "The video was not found!" });
            }

            video.ViewCounts += 1;

            if (!await _unit.Video.UpdateAsync(video))
            {
                return BadRequest(new { error = "Getting trouble updating video!" });
            }

            return Ok(new { message = "Video updated successfully!" });
        }

        [HttpPost("comment")]
        public async Task<IActionResult> Comment([FromBody] Comment comment)
        {
            if (comment.Message == null)
            {
                return BadRequest(new { error = "Comment field can not be empty!" });
            }

            comment.CreatedAt = DateTime.Now;
            comment.UpdatedAt = DateTime.Now;
            if (!await _unit.Comment.CreateAsync(comment))
            {
                return BadRequest(new { error = "Getting trouble!" });
            }

            return Ok(new { message = "Your comment has successfully been submitted!" });
        }

        [HttpPut("update-comment")]
        public async Task<IActionResult> UpdateComment([FromBody] Comment comment)
        {
            if (comment.Message == null)
            {
                return BadRequest(new { error = "Comment field can not be empty!" });
            }

            var getComment = await _unit.Comment.GetByIdAsync(comment.Id);

            if (comment == null)
            {
                return BadRequest(new { error = "The comment was not found!" });
            }

            getComment!.Message = comment.Message;
            getComment.UpdatedAt = DateTime.Now;

            if (!await _unit.Comment.UpdateAsync(getComment))
            {
                return BadRequest(new { error = "Getting trouble!" });
            }

            return Ok(new { message = "Your comment has successfully been updated!" });
        }

        [HttpDelete("delete-comment/{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _unit.Comment.GetByIdAsync(id);

            if (comment == null)
            {
                return BadRequest(new { error = "The comment was not found!" });
            }
            if (!await _unit.Comment.DeleteAsync(comment))
            {
                return BadRequest(new { error = "Getting trouble deleting the comment!" });
            }

            return Ok(new { message = "Your comment has successfully been deleted!" });
        }

        [HttpPost("like")]
        public async Task<IActionResult> Like([FromBody] Like like)
        {
            var previousLike = await _unit.Like.GetPreviousLike((Guid)like.LikerId!, like.VideoId);

            if (previousLike != null)
            {
                await _unit.Like.DeleteAsync(previousLike);
                return Ok(new { message = "Your like has been removed!" });
            }

            var previousDislike = await _unit.Dislike.GetPreviousDislike((Guid)like.LikerId, like.VideoId);
            if (previousDislike != null)
            {
                await _unit.Dislike.DeleteAsync(previousDislike);
            }

            if (!await _unit.Like.CreateAsync(like))
            {
                return BadRequest(new { error = "Getting trouble!" });
            }

            return Ok(new { message = "You have liked this video!" });
        }

        [HttpPost("dislike")]
        public async Task<IActionResult> Dislike([FromBody] Dislike dislike)
        {
            var previousDislike = await _unit.Dislike.GetPreviousDislike((Guid)dislike.DislikerId!, dislike.VideoId);

            if (previousDislike != null)
            {
                await _unit.Dislike.DeleteAsync(previousDislike);
                return Ok(new { message = "Your dislike has been removed!" });
            }

            var previousLike = await _unit.Like.GetPreviousLike((Guid)dislike.DislikerId, dislike.VideoId);
            if (previousLike != null)
            {
                await _unit.Like.DeleteAsync(previousLike);
            }

            if (!await _unit.Dislike.CreateAsync(dislike))
            {
                return BadRequest(new { error = "Getting trouble!" });
            }

            return Ok(new { message = "You have disliked this video!" });
        }
    }
}
