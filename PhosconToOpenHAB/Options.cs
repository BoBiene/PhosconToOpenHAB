using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhosconToOpenHAB
{
    internal class Options
    {
        [Option('a', "addr", Required = true, HelpText = "The IP-Address oder DNS name of the Phoscon-Mini-Server (e.g 192.168.1.100 or phosconServer)")]
        public string PhosconURL { get; set; }

        [Option('k', "apikey", Required = true, HelpText = "The API Key")]
        public string APIKey { get; set; }

        [Option('p', "httpPort", Required = false, HelpText = "The httpPort", Default = "8080")]
        public string HTTPPort { get; set; } = "8080";



        [Option('o', "output", Required = false, Default = "", HelpText = "Target directory to create the openHAB files in.")]
        public string OutputDir { get; set; }

    }
}