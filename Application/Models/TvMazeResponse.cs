using System.Text.Json.Serialization;
using Common;
using Common.Interfaces;

namespace Application.Models
{
    public class TvMazeShowResponse:  IAppModel
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public ShowModel Show { get; set; }
    }
    
    public sealed class TvMazeCastResponse: IAppModel
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public ActorModel Person { get; set; }
    }
}