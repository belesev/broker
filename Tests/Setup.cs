using System.IO;
using BrokerAlgo;
using NUnit.Framework;

[SetUpFixture]
internal class Setup
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var configFile = new FileInfo($"{TestContext.CurrentContext.TestDirectory}\\Config\\log4net.config");
        Logger.InitLogger(configFile);
    }
}