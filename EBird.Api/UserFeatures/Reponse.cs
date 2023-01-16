﻿namespace Response
{
    public class Response<T>
    {
        public object? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int? StatusCode { get; set; }
    }
}
