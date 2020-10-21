using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.Models.Interfaces
{
    /// <summary>
    /// base class for entities that have a create and last modified timestamp
    /// </summary>
    public interface ITimestampedEntity
    {
        /// <summary>
        /// Unique DB incremented Identification number.
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Datetime without a timezone saved as UTC in the DB and converted / displayed to local timezone.
        /// represents base entity types creation date time
        /// </summary>
        DateTime? Created { get; set; }

        /// <summary>
        /// Datetime without a timezone saved as UTC in the DB and converted / displayed to local timezone.
        /// represents base entity types Modified date time
        /// </summary>
        DateTime? Modified { get; set; }

        /// <summary>
        /// represents the entity to create the base entity
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// represents the last entity to update the base entity
        /// </summary>
        string ModifiedBy { get; set; }

    }
}
