using ConfigFiller;

namespace Test_ConfigFiller.IntegrationTests
{
    [TestFixture]
    internal class ConfigFillerManager_Test
    {
        [Test]
        [TestCase(@"--path | .\..\..\..\TestFiles\Temp.config | --fail0Replace | ConnectionString=Es una ConnectionString | LogDir=C:\Temp\Log | EnableLog=True"
                , @".\..\..\..\TestFiles\Web.config"
                , @".\..\..\..\TestFiles\WebReplacedOK.config")]
        public void Fill_OK(string commandLine, string referenceOriginalFilePath, string referenceCompletedFilePath)
        {
            string[] args = commandLine.Split('|');
            for (int i = 0; i < args.Count() - 1; i++)
            {
                args[i] = args[i].Trim();
            }

            string tempConfigFile = $@"{Guid.NewGuid()}.tmp";
            args[1] = tempConfigFile;

            //Creo el archivo temporal como copia del archivo de referencia sin completar
            //y le saco el bloqueo de escritura
            File.Copy(referenceOriginalFilePath, tempConfigFile);
            FileAttributes fileAttributes = File.GetAttributes(tempConfigFile);
            fileAttributes &= ~FileAttributes.ReadOnly; // Usamos una operación de bits para quitar el atributo
            File.SetAttributes(tempConfigFile, fileAttributes);

            //Invoco al programa para completar el archivo temporal
            int returnCode = Program.Main(args);

            //Cargo el archivo temporal recien completado y lo comparo con el de referencia completado
            string completedFile = File.ReadAllText(tempConfigFile);
            string referenceCompleteFilePath = File.ReadAllText(referenceCompletedFilePath);
            if (File.Exists(tempConfigFile)) File.Delete(tempConfigFile);

            Assert.Multiple(() =>
            {
                Assert.That(completedFile, Is.EqualTo(referenceCompleteFilePath));
                Assert.That(returnCode, Is.EqualTo(0));
            });   
        }


        [Test]
        [TestCase(@"--path | .\..\..\..\TestFiles\Temp.config | --fail0Replace | ConnectionString=Es una ConnectionString | LogDir=C:\Temp\Log | EnableLog=True | ParamNoExsite=True"
        , @".\..\..\..\TestFiles\Web.config"
        , @".\..\..\..\TestFiles\WebReplacedOK.config")]
        public void Fill_ThrowsException_ParameterNotFound(string commandLine, string referenceOriginalFilePath, string referenceCompletedFilePath)
        {
            string[] args = commandLine.Split('|');
            for (int i = 0; i < args.Count() - 1; i++)
            {
                args[i] = args[i].Trim();
            }

            string tempConfigFile = $@"{Guid.NewGuid()}.tmp";
            args[1] = tempConfigFile;

            //Creo el archivo temporal como copia del archivo de referencia sin completar
            //y le saco el bloqueo de escritura
            File.Copy(referenceOriginalFilePath, tempConfigFile);
            FileAttributes fileAttributes = File.GetAttributes(tempConfigFile);
            fileAttributes &= ~FileAttributes.ReadOnly; // Usamos una operación de bits para quitar el atributo
            File.SetAttributes(tempConfigFile, fileAttributes);

            //Invoco al programa
            int returnCode = Program.Main(args);

            Assert.That(returnCode, Is.EqualTo(1));
        }
    }

}
