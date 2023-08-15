﻿namespace Mango.Services.ProductAPI.Models.DTO
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; }=false;
        public string? Message { get; set; }
    }
}
