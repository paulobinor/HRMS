﻿namespace hrms_be_backend_common.DTO
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UpdateUserDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class DeactivateUserDto
    {
        public long UserId { get; set; }
        public string DeactivatedComment { get; set; }       
    }
    public class DisapproveUserDto
    {
        public long UserId { get; set; }
        public string DisapproveComment { get; set; }
    }
}
