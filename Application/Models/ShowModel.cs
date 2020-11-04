using System;
using System.Collections.Generic;
using Application.Interfaces.Models;
using Newtonsoft.Json;

namespace Application.Models
{
    public class ShowModel: IShowModel
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public IEnumerable<ActorModel> Cast { get; set; }
    }
}