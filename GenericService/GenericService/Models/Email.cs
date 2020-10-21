using GenericService.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericService.Models
{
    [Table("emails")]
    public class Email : ITimestampedEntity
    {

        #region private properties

        private string _address;
        private static readonly Regex ValidEmailRegex = CreateValidEmailRegex();

        #endregion

        #region keys

        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("Contact_id")]
        public long? ContactId { get; set; }


        #endregion

        #region defining properties

        [Required]
        [DataType(DataType.EmailAddress)]
        [Column("address")]
        public string Address
        {
            get => _address;
            set => _address = value.Trim().ToLowerInvariant();
        }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("is_primary")]
        public bool IsPrimary { get; set; }

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

        #region constructors 

        public Email()
        {
            Modified = Created = DateTime.UtcNow;
            ModifiedBy = "System";
        }

        public Email(string address) : this()
        {
            Address = address.Trim().ToLowerInvariant();
            Modified = Created = DateTime.UtcNow;
            ModifiedBy = "System";
        }

        #endregion

        #region functions

        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        /// <returns>Regex</returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Validates an email address via a regex pattern.
        /// See Email.CreateValidEmailRegex() for more details.
        /// </summary>
        /// <param name="emailAddress">The email address to be validated.</param>
        /// <returns></returns>
        public static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);
            return isValid;
        }

        /// <summary>
        /// For logging purposes, displays the most identifyable data of an email.
        /// </summary>
        /// <returns>a formatted string of important email properties</returns>
        public override string ToString()
        {
            return $"id:{Id}|Contact_id:{ContactId}|address:{Address}|is_deleted:{IsDeleted}|is_primary:{IsPrimary}";
        }

        public bool Equals(Email other)
        {
            if (other is null) return false;
            if (Created != other.Created) return false;
            if (Address != other.Address) return false;
            if (IsPrimary != other.IsPrimary) return false;
            if (IsDeleted != other.IsDeleted) return false;
            if (Modified != other.Modified) return false;
            if (ModifiedBy != other.ModifiedBy) return false;
            return Id == other.Id;
        }

        #endregion
    }
}
