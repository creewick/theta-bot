using System;
using CommandLine;

namespace theta_bot
{
    public class Options
    {
        [Option('t', Required = true, HelpText = "Telegram bot API token")]
        public string TelegramApiToken { get; set; }   
        
        [Option('p', Default = null, HelpText = "Address and port of proxy server")]
        public string Proxy { get; set; }   
        
        [Option("da", Required = true, HelpText = "Address of database")]
        public string DatabaseAddress { get; set; }   
        
        [Option("dt", HelpText = "Database token")]
        public string DatabaseToken { get; set; }   
    }
}