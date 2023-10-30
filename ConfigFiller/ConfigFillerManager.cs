using CommandLine;
using System.Text;

namespace ConfigFiller
{
    public class ConfigFillerManager
    {
        private readonly IUtils _utils;

        public ConfigFillerManager(IUtils utils)
        {
            _utils = utils;
        }


        public void Fill(string[] args)
        {
            try
            {
                Options options = Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(o => { }).Value;

                if (string.IsNullOrEmpty(options.Path))
                    throw new Exception("El parametro Path no puede ser nulo");

                if (!File.Exists(options.Path))
                    throw new Exception($"No se encuentra un archivo en el path pasado como parametro: {options.Path}");

                string fileContent = File.ReadAllText(options.Path);
                Console.WriteLine("Archivo localizado y abierto correctamente");

                int rangeToRemove = 2;  //nunca se contaran ni la palabra path, ni su valor
                if (options.FailIf0Replace) rangeToRemove++;
                //if (options.FailIfMore1Replace) rangeToRemove++;

                List<string> keyValuePairs = args.ToList();
                keyValuePairs.RemoveRange(0, rangeToRemove);

                fileContent = _utils.ReplaceVariables(fileContent, keyValuePairs, options.FailIf0Replace);

                Encoding encoding = _utils.GetFileEncoding(options.Path);
                _utils.SaveFile(options.Path, fileContent, encoding);
                Console.WriteLine($"Archivo {options.Path} guardado correctamente");
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
