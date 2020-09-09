using System;
using System.Collections.Generic;

namespace NetCoreTutorial.Domain
{
    public class Tag : IBaseEntity
    {
        public long Id { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public IEnumerable<PostTag> PostTags { get; set; }
    }
}