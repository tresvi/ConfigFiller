using ConfigFiller;
using System.Collections.Generic;
using System.Text;

namespace Test_ConfigFiller.UnitTests
{
    [TestFixture]
    internal class Utils_Test
    {
        private const string TEXTO_A_REEMPLAZAR1 = "Hola %1% tal %2% estas?";
        private const string TEXTO_REEMPLAZADO1 = "Hola que tal como estas?";

        private const string TEXTO_A_REEMPLAZAR2 = "Todo %estado%, aca %accion% un rato";
        private const string TEXTO_REEMPLAZADO2 = "Todo bien, aca caminando un rato";


        [Test]
        [TestCase(TEXTO_A_REEMPLAZAR1, TEXTO_REEMPLAZADO1, "1=que;2=como", false)]
        [TestCase(TEXTO_A_REEMPLAZAR2, TEXTO_REEMPLAZADO2, "estado=bien;accion=caminando", false)]
        public void ReplaceVariables_OK(string textToReplace, string textReplacedOK, string keyValuePairsString, bool failIfZeroReplace)
        {
            List<string> keyValuePairs = keyValuePairsString.Split(';').ToList();

            Utils utils = new Utils();
            string result = utils.ReplaceVariables(textToReplace, keyValuePairs, failIfZeroReplace);
            Assert.That(result, Is.EqualTo(textReplacedOK));
        }


        [Test]
        [TestCase(TEXTO_A_REEMPLAZAR1, TEXTO_REEMPLAZADO1, "1que;2=como", false)]
        [TestCase(TEXTO_A_REEMPLAZAR2, TEXTO_REEMPLAZADO2, "estado=bien; ", false)]
        public void ReplaceVariables_ThrowsException_WrongKeyValueStructure(string textToReplace, string textReplacedOK, string keyValuePairsString, bool failIfZeroReplace)
        {
            List<string> keyValuePairs = keyValuePairsString.Split(';').ToList();

            Utils utils = new Utils();
            Exception exceptionDetalle = Assert.Throws<Exception>(() => utils.ReplaceVariables(textToReplace, keyValuePairs, failIfZeroReplace));

            Assert.That(exceptionDetalle.Message, Does.Contain("requiere un par Clave=Valor").IgnoreCase);
        }


        [Test]
        [TestCase(TEXTO_A_REEMPLAZAR1, TEXTO_REEMPLAZADO1, "11=que;2=como", true)]
        [TestCase(TEXTO_A_REEMPLAZAR2, TEXTO_REEMPLAZADO2, "estadoXXX=bien;accion=caminando", true)]
        public void ReplaceVariables_ThrowsException_NotKeyPresentInText(string textToReplace, string textReplacedOK, string keyValuePairsString, bool failIfZeroReplace)
        {
            List<string> keyValuePairs = keyValuePairsString.Split(';').ToList();

            Utils utils = new Utils();
            Exception exceptionDetalle = Assert.Throws<Exception>(() => utils.ReplaceVariables(textToReplace, keyValuePairs, failIfZeroReplace));

            Assert.That(exceptionDetalle.Message, Does.Contain("no esta presente").IgnoreCase);
        }


        [Test]
        [TestCase(@".\..\..\..\TestFiles\Web.config")]
        public void GetFileEncoding_OK(string testFilePath)
        {
            Encoding expectedEncoding = Encoding.UTF8;

            Utils utils = new Utils();
            Encoding result = utils.GetFileEncoding(testFilePath);

            Assert.That(result, Is.EqualTo(expectedEncoding));
        }


        [Test]
        public void SaveFile_OK()
        {
            string tempFilePath = $@"{Guid.NewGuid()}.tmp";
            string fileContent = "XXXXXXXXXXXXXXXX";
            Encoding encoding = Encoding.UTF8;

            Utils utils = new Utils();
            utils.SaveFile(tempFilePath, fileContent, encoding);

            bool fileExists = File.Exists(tempFilePath);
            if (fileExists) File.Delete(tempFilePath);

            Assert.IsTrue(fileExists);
        }


        [Test]
        public void SaveFile_ThrowsExceptionProtectedFile()
        {
            string tempFilePath = $@"{Guid.NewGuid()}.tmp";
            string fileContent = "XXXXXXXXXXXXXXXX";
            Encoding encoding = Encoding.UTF8;

            //Creo un archivo y lo bloqueo
            Utils utils = new Utils();
            utils.SaveFile(tempFilePath, fileContent, encoding);
            FileAttributes originalFileAttributes = File.GetAttributes(tempFilePath);
            File.SetAttributes(tempFilePath, originalFileAttributes | FileAttributes.ReadOnly);

            //Assert
            Exception exceptionDetalle = Assert.Throws<Exception>(() => utils.SaveFile(tempFilePath, fileContent, encoding));

            //Lo desbloqueo y lo elimino
            File.SetAttributes(tempFilePath, originalFileAttributes);
            File.Delete(tempFilePath);

            Assert.That(exceptionDetalle.Message, Does.Contain("Error al guardar el archivo").IgnoreCase);
        }

    }
}
