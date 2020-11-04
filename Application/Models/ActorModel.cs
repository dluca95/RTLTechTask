using System;
using Application.Interfaces.Models;
using Newtonsoft.Json;

namespace Application.Models
{
    public class ActorModel: IActorModel, IEquatable<ActorModel>
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public bool Equals(ActorModel other)
        {
            return Id.Equals(other?.Id);
        }
    }
}