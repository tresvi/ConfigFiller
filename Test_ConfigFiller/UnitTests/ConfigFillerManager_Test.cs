using ConfigFiller;
using Moq;
using System.Text;

namespace Test_ConfigFiller.UnitTests
{
    [TestFixture]
    internal class ConfigFillerManager_Test
    {
        [Test]
        [TestCase(@"--path | .\..\..\..\TestFiles\Web.config | --fail0Replace | ConnectionString= Es_una_ConnectionSting | LogDir=C:\Temp\Log | EnableLog=True")]
        public void Fill_OK(string commandLine)
        {
            string[] args = commandLine.Split('|');
            for (int i = 0; i < args.Count() -1; i++)
            {
                args[i] = args[i].Trim();
            }
            string filePath = args[1];
            var mockUtils = new Mock<IUtils>();
            mockUtils.Setup(u => u.SaveFile(filePath, "XXX", Encoding.UTF8));
            mockUtils.Setup(u => u.ReplaceVariables("XXX", new List<string>(), true)).Returns("ZZZ");
            mockUtils.Setup(u => u.GetFileEncoding(filePath)).Returns(Encoding.UTF8);

            ConfigFillerManager configFiller = new ConfigFillerManager(mockUtils.Object);
            configFiller.Fill(args);

            Assert.IsTrue(true);
        }


        [Test]
        [TestCase(@"--path |   | --fail0Replace | ConnectionString= Es_una_ConnectionSting | LogDir=C:\Temp\Log | EnableLog=True")]
        public void Fill_ThrowsException_NoPathDefined(string commandLine)
        {
            string[] args = commandLine.Split('|');
            for (int i = 0; i < args.Count() - 1; i++)
            {
                args[i] = args[i].Trim();
            }
            string filePath = args[1];
            var mockUtils = new Mock<IUtils>();
            mockUtils.Setup(u => u.SaveFile(filePath, "XXX", Encoding.UTF8));
            mockUtils.Setup(u => u.ReplaceVariables("XXX", new List<string>(), true)).Returns("YYY");
            mockUtils.Setup(u => u.GetFileEncoding(filePath)).Returns(Encoding.UTF8);

            ConfigFillerManager configFiller = new ConfigFillerManager(mockUtils.Object);
           
            Exception exceptionDetalle = Assert.Throws<Exception>(() => configFiller.Fill(args));
            Assert.That(exceptionDetalle.Message, Does.Contain("El parametro Path no puede ser nulo").IgnoreCase);
        }


        [Test]
        public void Fill_ThrowsException_FileNotExists()
        {
            string[] args = { 
                "--path"
                , Path.GetRandomFileName()
                ,"--fail0Replace"
                , "ConnectionString=Es_una_ConnectionSting"
                , @"LogDir=C:\Temp\Log"
                , "EnableLog=True"
            };
            
            string filePath = args[1];
            var mockUtils = new Mock<IUtils>();
            mockUtils.Setup(u => u.SaveFile(filePath, "XXX", Encoding.UTF8));
            mockUtils.Setup(u => u.ReplaceVariables("XXX", new List<string>(), true)).Returns("YYY");
            mockUtils.Setup(u => u.GetFileEncoding(filePath)).Returns(Encoding.UTF8);

            ConfigFillerManager configFiller = new ConfigFillerManager(mockUtils.Object);

            Exception exceptionDetalle = Assert.Throws<Exception>(() => configFiller.Fill(args));
            Assert.That(exceptionDetalle.Message, Does.Contain("No se encuentra un archivo").IgnoreCase);
        }

    }
}
