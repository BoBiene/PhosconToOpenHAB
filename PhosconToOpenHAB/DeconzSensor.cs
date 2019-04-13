using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhosconToOpenHAB
{
    public class DeconzSensor
    {
        public Dictionary<string, object> Config { get; set; } = new Dictionary<string, object>();
        public string Etag { get; set; }
        public string Manufacturername { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uniqueid { get; set; }

        public string Modelid { get; set; }

        public int Id { get; set; }

        public string DeviceId => Uniqueid?.Substring(0, Uniqueid?.LastIndexOf('-') ?? 0);
    }
}
