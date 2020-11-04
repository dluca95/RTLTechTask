using System.Collections.Generic;
using Application.Models;

namespace Application.Mappings
{
    public class ActorEqualityComparer: IEqualityComparer<ActorModel>
    {
        public bool Equals(ActorModel x, ActorModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            
            return x.Id == y.Id;
        }

        public int GetHashCode(ActorModel obj)
        {
            throw new System.NotImplementedException();
        }
    }
}