using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Common.Interfaces;

namespace Persistence.Models
{
    public class Show: IEntityModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public ICollection<ActorShow> Cast { get; set; }
    }
}