namespace BrokerAlgo.Interfaces
{
    interface IPriceToFileLoader
    {
        void Save();
        void Save(string filename);
    }
}