﻿namespace Core.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Apartment { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode {  get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}
