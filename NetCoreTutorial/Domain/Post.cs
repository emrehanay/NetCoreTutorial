using System;
using System.Collections.Generic;

namespace NetCoreTutorial.Domain
{
    public class Post : IBaseEntity
    {
        public long Id { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public long UserId { get; set; }

        public User User { get; set; }
        public IEnumerable<PostTag> PostTags { get; set; }
    }
}