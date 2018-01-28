using JetBrains.Annotations;
using System.Collections.Generic;

namespace BrokerAlgo.Interfaces
{
    public interface IStrategy
    {
        [CanBeNull]
        IList<IDeal> GetDeals(ITool tool);
    }
}