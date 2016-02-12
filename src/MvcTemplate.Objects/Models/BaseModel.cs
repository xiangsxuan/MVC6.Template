using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public abstract class BaseModel
    {
        [Key]
        [Required]
        [StringLength(36)]
        public virtual String Id
        {
            get
            {
                return InternalId ?? (InternalId = Guid.NewGuid().ToString());
            }
            set
            {
                InternalId = value;
            }
        }
        private String InternalId
        {
            get;
            set;
        }

        public virtual DateTime CreationDate
        {
            get
            {
                if (!IsCreationDateSet)
                    CreationDate = DateTime.Now;

                return InternalCreationDate;
            }
            protected set
            {
                IsCreationDateSet = true;
                InternalCreationDate = value;
            }
        }
        private Boolean IsCreationDateSet
        {
            get;
            set;
        }
        private DateTime InternalCreationDate
        {
            get;
            set;
        }
    }
}
