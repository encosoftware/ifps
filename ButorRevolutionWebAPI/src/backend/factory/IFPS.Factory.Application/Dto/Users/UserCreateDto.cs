﻿namespace IFPS.Factory.Application.Dto
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool GaveEmailConsent { get; set; }
        public string Password { get; set; }
    }
}