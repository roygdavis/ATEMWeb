using System;

namespace ATEMWebApp.Dtos
{
    public class ResponseDto<T1>
    {
        public bool IsSuccess { get; set; }
        public T1 Data { get; set; }

        public static ResponseDto<T1> CreateOkResponse(T1 data)
        {
            return new ResponseDto<T1>
            {
                Data = data,
                IsSuccess = true,
            };
        }

        public static ResponseDto<ErrorDto> CreateErrorResponse<T2>(string message, T2 exception) where T2: Exception
        {
            return new ResponseDto<ErrorDto>
            {
                IsSuccess = false,
                Data = new ErrorDto
                {
                    Error = exception.Message,
                    Message = message
                }
            };
        }
    }
}
