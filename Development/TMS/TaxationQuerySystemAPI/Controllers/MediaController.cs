using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        public MediaManager _mediaManager { get; }
        public MediaController(MediaManager mediaManager)
        {
            _mediaManager = mediaManager;
        }

        #region "Media"

        // GET: api/Media
        [HttpGet("api/Media")]
        public ActionResult<IEnumerable<Media>> GetMedia()
        {
            try
            {
                _mediaManager.LoadXml();
                return Ok(_mediaManager.xmlObjects.OrderByDescending(o => o.Id));
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }

        }

        // GET: api/Media/5
        [HttpGet("api/Media/{id}")]
        public ActionResult<Media> GetMedia(long id)
        {

            try
            {
                _mediaManager.LoadXml();
                var media = _mediaManager.xmlObjects.FirstOrDefault(m => m.Id == id);

                if (media == null)
                {
                    return NotFound(new { Error = $"Media with id {id} not found" });
                }

                return Ok(media);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // PUT: api/Media/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("api/Media/{id}")]
        public IActionResult PutMedia(long id, Media media)
        {
            if (id != media.Id)
            {
                return BadRequest();
            }
            try
            {
                _mediaManager.LoadXml();
                _mediaManager.EditMedia(media);
                _mediaManager.SaveXml();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    return NotFound(new { Error = ex.Message });
                }
                if (!MediaExists(id))
                {
                    return NotFound(new { Error = $"Media with id {id} not found" });
                }
                else
                {
                    return NotFound(new { Error = ex.Message });
                }
            }

            return Ok();
        }

        // POST: api/Media
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("api/Media")]
        public ActionResult<Media> PostMedia(Media media)
        {
            try
            {
                _mediaManager.LoadXml();
                _mediaManager.AddMedia(media);
                _mediaManager.SaveXml();

                return Ok(media);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // DELETE: api/Media/5
        [HttpDelete("api/Media/{id}")]
        public ActionResult<Media> DeleteMedia(long id)
        {
            try
            {
                _mediaManager.LoadXml();
                var media = _mediaManager.xmlObjects.FirstOrDefault(m => m.Id == id);
                if (media == null)
                {
                    return NotFound(new { Error = $"Media with id {id} not found" });
                }

                _mediaManager.RemoveMedia(media);
                _mediaManager.SaveXml();

                return Ok(media);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        private bool MediaExists(long id)
        {
            return _mediaManager.xmlObjects.Any(e => e.Id == id);
        }

        // GET: api/files
        [HttpGet("api/files")]
        public ActionResult<IEnumerable<File>> Getfiles()
        {
            try
            {
                _mediaManager.LoadXml();
                return Ok(_mediaManager.xmlObjects.SelectMany(media => media.files).ToList());
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpGet("api/files/{id}")]
        public ActionResult<File> Getfile(string id)
        {
            try
            {
                _mediaManager.LoadXml();
                var file = _mediaManager.xmlObjects.SelectMany(media => media.files).FirstOrDefault(ofile => ofile.fileName == id);

                if (file == null)
                {
                    return NotFound(new { Error = $"File with Name {id} not found" });
                }

                return Ok(file);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("api/Media/{mediaId}/files/")]
        public IActionResult PostFile(long mediaId, File file)
        {
            try
            {
                _mediaManager.LoadXml();
                if (!MediaExists(mediaId))
                {
                    return NotFound(new { Error = $"Media with id {mediaId} not found" });
                }
                _mediaManager.AddFile(mediaId, file);
                _mediaManager.SaveXml();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // DELETE: api/mediaId/5
        [HttpDelete("api/Media/{mediaId}/files/{id}")]
        public ActionResult<Media> DeleteFile(string id, long mediaId)
        {
            try
            {
                _mediaManager.LoadXml();
                if (!MediaExists(mediaId))
                {
                    return NotFound(new { Error = $"Media with id {mediaId} not found" });
                }
                var file = _mediaManager.xmlObjects.SelectMany(media => media.files).FirstOrDefault(ofile => ofile.fileName == id);

                if (file == null)
                {
                    return NotFound(new { Error = $"File with Name {id} not found" });
                }

                _mediaManager.RemoveFile(mediaId, file);
                _mediaManager.SaveXml();

                return Ok(file);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        private bool FileExists(string id)
        {
            return _mediaManager.xmlObjects.SelectMany(media => media.files).Any(e => e.fileName == id);
        }
        #endregion
    }
}