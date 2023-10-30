using CommandLine;

namespace ConfigFiller
{
    internal class Options
    {
        [Option('p', "path", Required = true, HelpText = "Path del archvo de configuracion a completar")]
        public string Path { get; set; }

        [Option('z', "fail0Replace", Required = false, HelpText = "Falla si no se encontro ninguna lcave para reemplazar")]
        public bool FailIf0Replace { get; set; }

//        [Option('m', "fail1+Replace", Required = false, HelpText = "Falla si alguna clave fue encontrada mas de 1 vez en el archivo")]
//        public bool FailIfMore1Replace { get; set; }
    }
}
