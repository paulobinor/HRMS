﻿namespace hrms_be_backend_common.DTO
{
    public class CompanyCreateDto
    {       
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyLogo { get; set; }
        public string Website { get; set; }
        public string FullAddress { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public bool IsPublicSector { get; set; }
    }
    public class CompanyUpdateDto
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyLogo { get; set; }
        public string Website { get; set; }
        public string FullAddress { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public bool IsPublicSector { get; set; }
    }
}