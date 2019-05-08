using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class CommandModes
    {
        public List<string> SqlCommands { get; set; }
        public string[] TablesToValidate { get; set; }
    }
    public class OtherMaps
    {
        private static List<dynamic> modelCreatingMaps = new List<dynamic>();
        private static List<CommandModes> _commandModes = new List<CommandModes>();
       

        public static void AddMaps(dynamic map)
        {
            modelCreatingMaps.Add(map);
        }

        public static List<dynamic> ModelCreatingMaps
        {
            get { return modelCreatingMaps; }
        }

        public static void AddSqlCommands(string[] tablesValidate, List<string> command)
        {
            _commandModes.Add(new CommandModes
            {
                TablesToValidate = tablesValidate,
                SqlCommands = command,
            });
        }

        public static List<CommandModes> SqlCommands
        {
            get { return _commandModes; }
        } 


    }
}
