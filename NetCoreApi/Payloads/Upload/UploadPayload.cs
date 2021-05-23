using Microsoft.AspNetCore.Http;

namespace NetCoreApi.Payloads.Upload
{
    public class UploadPayload
    {
        public string DisplayName { get; set; }
        public string Remark { get; set; }
        public IFormFile File { get; set; }
    }
}
