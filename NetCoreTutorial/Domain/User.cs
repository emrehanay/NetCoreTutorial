using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetCoreTutorial.Domain
{
    public class User : IBaseEntity
    {
        public long Id { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string PasswordHash { get; set; }
        [JsonIgnore] public IEnumerable<Post> Posts { get; set; }
    }
}