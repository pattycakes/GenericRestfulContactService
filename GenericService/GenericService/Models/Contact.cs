using GenericService.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.Models
{
    [Table("contacts")]
    public sealed class Contact : ITimestampedEntity
    {
        #region Keys

        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }


        #endregion

        #region  name fields

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        #endregion

        #region collection fields

        [Column("emails")]
        public List<Email> Emails { get; set; }

        #endregion

        #region contacts work info

        [Column("job_title")]
        public string JobTitle { get; set; }

        #endregion

        #region audit properties 

        [DataType(DataType.DateTime)]
        [Column("created")]
        [Required]
        public DateTime? Created { get; set; }

        [DataType(DataType.DateTime)]
        [Column("modified")]
        [Required]
        public DateTime? Modified { get; set; }

        [Column("modified_by")]
        public string ModifiedBy { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        #endregion

        #region data administration

        /// <summary>
        /// flag for soft-deleting contacts.  if a contact is deleted this way, its nested emails and phones are 
        /// implicitly deleted, thoough they will not be marked that way in the DB.
        /// </summary>
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }


        #endregion

        public Contact()
        {
            Modified = Created = DateTime.UtcNow;
            Emails = new List<Email>();
        }

        public bool Equals(Contact other)
        {
            if (other is null) return false;
            if (Emails != other.Emails) return false;
            if (Modified != other.Modified) return false;
            if (Created != other.Created) return false;
            if (FirstName != other.FirstName) return false;
            if (LastName != other.LastName) return false;
            if (JobTitle != other.JobTitle) return false;
            if (IsDeleted != other.IsDeleted) return false;
            return Id == other.Id;
        }
    }
}
