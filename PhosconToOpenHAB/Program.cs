using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhosconToOpenHAB
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Options>(args)
                 .WithParsed<Options>(Run);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: " + ex.ToString());
            }
        }

        static void Run(Options options)
        {
            try
            {
                using (StreamWriter thingsFile = CreateConfigFile(options, Path.Combine("things","phoscon.things")))
                {
                    using (StreamWriter itemsFile = CreateConfigFile(options, Path.Combine("items", "phoscon.items")))
                    {
                        using (var sitemapFile = CreateSitemap(options))
                        {


                            Uri phosconBaseURL = new Uri($"http://{options.PhosconURL}:{options.HTTPPort}/");

                            RestRequest request = new RestRequest("api/{APIKEY}/{Method}");
                            request.AddUrlSegment("APIKEY", options.APIKey.ToString());
                            request.AddUrlSegment("Method", "sensors");

                            RestClient client = new RestClient(phosconBaseURL);

                            var responseSonsors = client.Execute(request);
                            //JObject jsonSensors = JObject.Parse(responseSonsors.Content);

                            var sensors = JsonConvert.DeserializeObject<Dictionary<string, DeconzSensor>>(responseSonsors.Content);

                            using (var deconzBridge = CreateDconzBridge(options, thingsFile))
                            {
                                CreateItems(itemsFile, deconzBridge, sensors);
                            }

                            request = new RestRequest("api/{APIKEY}/{Method}");
                            request.AddUrlSegment("APIKEY", options.APIKey.ToString());
                            request.AddUrlSegment("Method", "lights");

                            var responseLights = client.Execute(request);
                            JObject jsonLights = JObject.Parse(responseLights.Content);

                            var lights = JsonConvert.DeserializeObject<Dictionary<string, DeconzSensor>>(responseLights.Content);

                            using (var deconzBridge = CreateHueBridge(options, thingsFile))
                            {
                                CreateItems(itemsFile, deconzBridge, lights);
                            }
                        }
                    }
                }
                DirectoryInfo confDir = new DirectoryInfo(Path.Combine(options.OutputDir, "conf"));
                Console.WriteLine("Created config files at " + confDir.FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: " + ex.ToString());
            }
        }

        private static void CreateItems(StreamWriter itemsFile, PhosconBridge brige, Dictionary<string, DeconzSensor> sensors)
        {
            foreach (var keyValue in sensors)
                keyValue.Value.Id = Convert.ToInt32(keyValue.Key);

            foreach (var device in sensors.Values.GroupBy((sensor) => sensor.DeviceId))
            {
                var deviceid = device.Key;
                var devices = device.OrderBy((sensor) => sensor.Id);
                var primaryDevice = device.First();

                if (Thing.TryGetThing(primaryDevice.Type, out Thing primaryThing))
                {
                    itemsFile.WriteLine($"//{primaryDevice.Name}");
                    itemsFile.WriteLine($"Group g{primaryDevice.Name.Escape()} \"{primaryDevice.Name}\"");

                    var sensorName = primaryDevice.Name.Escape();

                    bool blnHasTempChannel = false;
                    foreach (var sensor in device)
                    {
                        if (brige.TryAddSensor(primaryDevice.Name, sensor, out Thing thing))
                        {
                            foreach (var channel in thing.Channels)
                            {
                                AddItem(itemsFile, brige, primaryDevice, sensorName, sensor, thing, channel);
                            }

                            blnHasTempChannel |= thing.Channels.Contains(Thing.Channel.temperature);
                        }
                        else { }
                    }


                    if (primaryDevice.Config.ContainsKey("temperature") && !blnHasTempChannel)
                    {
                        AddItem(itemsFile, brige, primaryDevice, sensorName, primaryDevice, primaryThing, Thing.Channel.temperature);
                    }
                    else { }

                    AddItem(itemsFile, brige, primaryDevice, sensorName, primaryDevice, primaryThing, Thing.Channel.last_updated);

                    if (primaryDevice.Config.ContainsKey("battery"))
                    {
                        AddItem(itemsFile, brige, primaryDevice, sensorName, primaryDevice, primaryThing, Thing.Channel.battery_level);
                        AddItem(itemsFile, brige, primaryDevice, sensorName, primaryDevice, primaryThing, Thing.Channel.battery_low);
                    }
                    else { }
                    itemsFile.WriteLine();
                }
                else
                {

                    Console.WriteLine("Type is not supported, ignoring " + primaryDevice.Type);
                }

            }
        }

        private static void AddItem(StreamWriter itemsFile, PhosconBridge brige, DeconzSensor primaryDevice, string sensorName, DeconzSensor sensor, Thing thing, Thing.Channel channel)
        {
            string itemTag = string.IsNullOrEmpty(channel.ItemTag) ? string.Empty : $"[{channel.ItemTag}]";
            string alexa = string.IsNullOrEmpty(channel.AlexaTpye) ? string.Empty : "alexa=\"" + channel.AlexaTpye + "\",";
            itemsFile.WriteLine($"{channel.ItemType} phoscon_{sensorName}_{channel.ChannelType} \"{primaryDevice.Name} {channel.ChannelType}\" (g{primaryDevice.Name.Escape()}) {itemTag} {{{alexa}channel=\"{brige.GetChannel(channel,thing,sensor)}\"}}");
        }

        private static DconzBridge CreateDconzBridge(Options options, StreamWriter writer)
        {
            writer.WriteLine($"Bridge deconz:deconz:phoscon \"Phoscon Deconz-Bridge\" [ host=\"{options.PhosconURL}\", httpPort=\"{options.HTTPPort}\", apikey=\"{options.APIKey}\" ] {{");

            return new DconzBridge(writer);
        }

        private static HueBridge CreateHueBridge(Options options, StreamWriter writer)
        {
            writer.WriteLine($"Bridge hue:bridge:phoscon \"Phoscon Hue-Bridge\" [ ipAddress=\"{options.PhosconURL}\", port=\"{options.HTTPPort}\", userName=\"{options.APIKey}\" ] {{");

            return new HueBridge(writer);
        }
        private class ConfigSection : IDisposable
        {
            public StreamWriter Writer { get;  }
            public ConfigSection(StreamWriter writer)
            {
                Writer = writer;
            }

            public void Dispose()
            {
                Writer.WriteLine("}");
                Writer.WriteLine();
            }
        }
        private abstract class PhosconBridge :ConfigSection
        {
            public PhosconBridge(StreamWriter writer)
               : base(writer)
            {

            }

            internal abstract string GetChannel(Thing.Channel channel, Thing thing, DeconzSensor sensor);

            internal abstract bool TryAddSensor(string strSensorName, DeconzSensor sensor, out Thing thing);
        }

        private class HueBridge : PhosconBridge
        {
            public HueBridge(StreamWriter writer)
               : base(writer)
            {

            }

            internal override bool TryAddSensor(string strSensorName, DeconzSensor sensor, out Thing thing)
            {
                var sensorName = strSensorName == sensor.Name ? strSensorName : $"{strSensorName} ({sensor.Name})";
                var sensorType = sensor.Type;
                var uniqueID = sensor.Uniqueid;

                if (Thing.TryGetThing(sensorType, out thing))
                {
                    var itemId = uniqueID.Escape();
                    Writer.WriteLine($"{thing.Name} {itemId} \"Phoscon {sensorName}\" [lightId=\"{sensor.Id}\"]");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Unknown sensor type {sensorType} for sensor {sensor.Id}");
                    return false;
                }
            }

            internal override string GetChannel(Thing.Channel channel, Thing thing, DeconzSensor sensor)
            {
                return $"hue:{thing.Name}:phoscon:{sensor.Uniqueid.Escape()}:{channel.ChannelType}";
            }
        }
        private class DconzBridge : PhosconBridge
        {

            public DconzBridge(StreamWriter writer)
                : base(writer)
            {

            }

            internal override string GetChannel(Thing.Channel channel, Thing thing, DeconzSensor sensor)
            {
                return $"deconz:{thing.Name}:phoscon:{sensor.Uniqueid.Escape()}:{channel.ChannelType}";
            }

            internal override bool TryAddSensor(string strSensorName, DeconzSensor sensor, out Thing thing)
            {
                var sensorName = strSensorName == sensor.Name ? $"{strSensorName} ({sensor.Id})" : $"{strSensorName} ({sensor.Name})";
                var sensorType = sensor.Type;
                var uniqueID = sensor.Uniqueid;

                if (Thing.TryGetThing(sensorType, out thing))
                {
                    var itemId = uniqueID.Escape();
                    Writer.WriteLine($"{thing.Name} {itemId} \"Phoscon {sensorName}\" [id=\"{sensor.Id}\"]");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Unknown sensor type {sensorType} for sensor {sensor.Id}");
                    return false;
                }
            }
        }

 
        private static StreamWriter CreateConfigFile(Options options, string strName)
        {
            string strPath = Path.Combine(options.OutputDir, Path.Combine("conf" , strName));
            FileInfo f = new FileInfo(strPath);
            if (!f.Directory.Exists)
                f.Directory.Create();

            Stream s = File.Open(strPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            return new StreamWriter(s);
        }


        private static Sitemap CreateSitemap(Options options)
        {
            var writer = CreateConfigFile(options, Path.Combine("sitemaps","phoscon.sitemap"));

            writer.WriteLine("sitemap phoscon label=\"Phoscon\" {");

            return new Sitemap(writer);
        }

        private class Sitemap : ConfigSection
        {
            public Sitemap(StreamWriter writer)
               : base(writer)
            {

            }

        }
    }
}
