using System;

namespace NetCoreTutorial.Domain
{
    public interface IBaseEntity
    {
        public static int STATUS_INITIAL = 0;

        public long Id { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}