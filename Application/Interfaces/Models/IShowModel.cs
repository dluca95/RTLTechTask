using System.Collections.Generic;
using Application.Models;
using Common;
using Common.Interfaces;

namespace Application.Interfaces.Models
{
    public interface IShowModel: IAppModel
    {
      public string Name { get; set; }   
      
      public IEnumerable<ActorModel> Cast { get; set; }
    }
}