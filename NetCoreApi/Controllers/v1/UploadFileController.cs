using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreApi.Payloads.Upload;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NetCoreApi.Controllers.v1
{
    public class UploadFileController : ApiController
    {
        public UploadFileController(ILoggerFactory factory) : base(factory) { }

        /// <summary>
        /// 檔案上傳
        /// </summary>
        /// <returns></returns>
        /// <response code="204">上傳成功</response>
        /// <response code="400">上傳失敗</response>
        [HttpPost, Route("{prefix}/{root}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(string prefix, string root, [FromForm] UploadPayload upload)
        {
            string _prefix = System.Net.WebUtility.UrlEncode(prefix);
            string _path = Path.Combine(_prefix, System.Net.WebUtility.UrlEncode(root));

            try
            {
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);

                using var stream = new FileStream(Path.Combine(_path, upload.File.FileName), FileMode.Create);
                await upload.File.CopyToAsync(stream);

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 檔案下載
        /// </summary>
        /// <returns></returns>
        /// <response code="204">下載成功</response>
        /// <response code="400">找不到檔案</response>
        [HttpGet, Route("{prefix}/{root}/{fileName}")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Download(string prefix, string root, string fileName)
        {
            string _prefix = System.Net.WebUtility.UrlEncode(prefix);
            string _path = Path.Combine(_prefix, System.Net.WebUtility.UrlEncode(root), fileName);

            try
            {
                var memoryStream = new MemoryStream();

                using (var stream = new FileStream(_path, FileMode.Open))
                    await stream.CopyToAsync(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);

                Response.Headers.Add("Content-Disposition", $"attachment;filename=\"{fileName}\"");
                return File(memoryStream, "application/octet-stream");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
