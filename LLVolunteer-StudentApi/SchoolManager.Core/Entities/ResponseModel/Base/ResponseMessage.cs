using System;
using System.Collections.Generic;
using System.Text;
using SchoolManager.Core.Entities.Enum;

namespace SchoolManager.Core.Entities.ResponseModel.Base
{
    /// <summary>
    /// 响应数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseMessage<T>
    {
        public ResponseCodeType ResponseCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }

        public void Ok(T data)
        {
            Data = data;
            ResponseCode = ResponseCodeType.Ok;
            Message = string.Empty;
            StackTrace = string.Empty;
            Success = true;
        }

        public void Error(Exception e)
        {
            ResponseCode = ResponseCodeType.Error;
            Message = e.Message;
            StackTrace = e.StackTrace;
            Success = false;
        }
    }
}
