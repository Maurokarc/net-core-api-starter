using NetCoreApi.Toolkit.Enums;
using System.Collections.Generic;

namespace NetCoreApi.Toolkit.DTO
{
    public class ResultDTO<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucceed { get; set; } = true;

        /// <summary>
        /// 回傳狀態碼
        /// </summary>
        public ResultCode ResultCode { get; set; } = ResultCode.Empty;

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 自定義資料
        /// </summary>
        public T Meta { get; set; }

        /// <summary>
        /// 擴充資料
        /// </summary>
        public Dictionary<string, object> DataSet { get; set; }
    }

    public class ResultDTO : ResultDTO<object> { }
}
