using System.Collections.Generic;

namespace Theta_Bot.Database
{
    public class Level
    {
        public string Name { get; set; }
        public Dictionary<string, string> Requires { get; set; }
    }
}

//levels:
//    <id>:
//        name: <string>
//        requires:
//            [0]: <level-id>
//            ...