/*
 *Ejemplos de invocacion:

    \>ConfigFiller --path "C:\Temp\prueba.txt" --Fail0Replace "ConnectionString=Es una ConnectionString" "LogEnable=True"
    \>ConfigFiller --path "C:\Temp\prueba.txt" "ConnectionString=Es una ConnectionString" "LogEnable=True"
*/

namespace ConfigFiller
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                IUtils utils = new Utils();
                ConfigFillerManager configFiller = new ConfigFillerManager(utils);
                configFiller.Fill(args);
                return 0;
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine( ex.Message );
                return 1;
            }
        }


    }
}