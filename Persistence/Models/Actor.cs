using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Interfaces;

namespace Persistence.Models
{
    public class Actor: IEntityModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Birthday { get; set; }
        
        public ICollection<ActorShow> Shows { get; set; }
    }
}