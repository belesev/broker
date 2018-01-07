using BrokerAlgo.Entities;
using JetBrains.Annotations;
using QuikSharp;

namespace BrokerAlgo.Interfaces
{
    public interface IStrategy
    {
        [CanBeNull]
        IDeal GetDeal(Quik quik, ToolCode toolCode);
    }
}