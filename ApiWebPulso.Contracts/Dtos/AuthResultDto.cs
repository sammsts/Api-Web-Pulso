﻿namespace ApiWebPulso.Contracts.Dtos
{
    public class AuthResultDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }
    }
}
