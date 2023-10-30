using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigFiller
{
    public interface IUtils
    {
        Encoding GetFileEncoding(string filePath);
        string ReplaceVariables(string originalText, List<string> keyValuePairs, bool failIfZeroReplace);
        void SaveFile(string filePath, string content, Encoding encoding);
    }


    public class Utils : IUtils
    {
        public string ReplaceVariables(string textToReplace, List<string> keyValuePairs, bool failIfZeroReplace)
        {
            try
            {
                foreach (string keyValuePair in keyValuePairs)
                {
                    string[] keyValue = keyValuePair.Split('=');

                    if (keyValue.Count() != 2)
                        throw new Exception($"Cada juego de parametros requiere un par Clave=Valor, el juego '{keyValuePair}' no cumple con esta estrucutra");

                    string key = keyValue[0].Trim();
                    string value = keyValue[1];

                    if (failIfZeroReplace)
                    {
                        bool TextIsPresent = textToReplace.Contains($"%{key}%", StringComparison.OrdinalIgnoreCase);
                        if (!TextIsPresent)
                        {
                            throw new Exception($"La clave '{key}' no esta presente en el archivo analizado");
                        }
                    }

                    textToReplace = textToReplace.Replace($"%{key}%", value, StringComparison.OrdinalIgnoreCase);
                }
                return textToReplace;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al reemplazar los pares Clave=Valor. Detalles: {ex.Message}");
            }
        }


        public Encoding GetFileEncoding(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath, detectEncodingFromByteOrderMarks: true))
                {
                    return reader.CurrentEncoding;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error determinar el encoding del archivo {filePath}. Detalles: {ex.Message}");
            }
        }


        public void SaveFile(string filePath, string content, Encoding encoding)
        {
            try
            {
                using (var writer = new StreamWriter(filePath, false, encoding))
                {
                    writer.Write(content);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el archivo modificado '{filePath}'. Detalles: {ex.Message}");
            }
        }


    }
}
