using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhosconToOpenHAB
{
    public class DeconzSensor
    {
        private static Regex regex = new Regex("^(?<Name>.+?)(\\s?\\[(?<Group>.+)\\])?$",RegexOptions.CultureInvariant| RegexOptions.Compiled);


        private string m_strName = string.Empty;
        public Dictionary<string, object> Config { get; set; } = new Dictionary<string, object>();
        public string Etag { get; set; }
        public string Manufacturername { get; set; }
        public string Name { 
            get {   return m_strName; } 
            set {
                var m= regex.Match(value);
                m_strName = m.Groups["Name"].Value;
                openHABGroupTag = m.Groups["Group"].Value;
            } 
        }
        public string openHABGroupTag { get; set; }
        public string Type { get; set; }
        public string Uniqueid { get; set; }

        public string Modelid { get; set; }

        public int Id { get; set; }

        public string DeviceId => Uniqueid?.Substring(0, Uniqueid?.LastIndexOf('-') ?? 0);
    }
}
