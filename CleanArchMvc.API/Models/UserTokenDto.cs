﻿namespace CleanArchMvc.API.Models
{
    public class UserTokenDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
