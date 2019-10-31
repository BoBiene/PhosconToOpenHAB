using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhosconToOpenHAB
{
    public class Thing
    {
        public static readonly Thing presencesensor = new Thing
                    ("presencesensor", new Channel[] { new Channel { ChannelType = "presence", ItemType = "Switch",AlexaTpye= "MotionSensor", Description = "Status of presence: ON = presence; OFF = no-presence" } });
        public static readonly Thing powersensor = new Thing
                    ("powersensor", new Channel[] {
                                new Channel { ChannelType = "power", ItemType = "Number:Power" },
                                new Channel { ChannelType = "voltage", ItemType = "Number:ElectricPotential" },
                                new Channel { ChannelType = "current", ItemType = "Number:ElectricCurrent" } });
        public static readonly Thing consumptionsensor = new Thing
                    ("consumptionsensor", new Channel[] {
                                new Channel { ChannelType = "power", ItemType = "Number:Power" },
                                new Channel { ChannelType = "consumption", ItemType = "Number:Energy" } });
        public static readonly Thing Switch = new Thing
                   ("switch", new Channel[] {
                               new Channel { ChannelType = "button", ItemType = "Number", Description="Last pressed button id on a switch" } });
        public static readonly Thing lightsensor = new Thing
                   ("lightsensor", new Channel[] {
                            new Channel { ChannelType = "lightlux", ItemType = "Number:Illuminance", Description = "Current light illuminance in Lux" },
                            new Channel { ChannelType = "light_level", ItemType = "Number",Description="Current light level" },
                            new Channel { ChannelType = "dark", ItemType = "Switch",Description="Light level is below the darkness threshold." },
                            new Channel { ChannelType = "daylight", ItemType = "Switch",Description="Light level is above the daylight threshold." },
                   });
        public static readonly Thing temperaturesensor = new Thing
                   ("temperaturesensor", new Channel[] { Channel.temperature });

        public static readonly Thing humiditysensor = new Thing
                   ("humiditysensor", new Channel[] {
                       new Channel {ChannelType = "humidity", ItemType = "Number:Dimensionless",AlexaTpye="CurrentHumidity" } });

        public static readonly Thing pressuresensor = new Thing
                   ("pressuresensor", new Channel[] {
                       new Channel { ChannelType = "pressure", ItemType = "Number:Pressure" } });

        public static readonly Thing openclosesensor = new Thing
                   ("openclosesensor", new Channel[] {
                       new Channel { ChannelType = "open", ItemType = "Contact", AlexaTpye="ContactSensor" } });

        public static readonly Thing waterleakagesensor = new Thing
                   ("waterleakagesensor", new Channel[] {
                       new Channel { ChannelType = "waterleakage", ItemType = "Switch" ,Description="Status of water leakage: ON = water leakage detected; OFF = no water leakage detected" } });

        public static readonly Thing alarmsensor = new Thing
                   ("alarmsensor", new Channel[] {
                       new Channel { ChannelType = "alarm", ItemType = "Switch" ,Description="Status of an alarm: ON = alarm was triggered; OFF = no alarm"} });
        public static readonly Thing vibrationsensor = new Thing
            ("vibrationsensor", new Channel[] { new Channel { ChannelType = "vibration", ItemType = "Switch", Description = "Status of vibration: ON = vibration was detected; OFF = no vibration" } });

        public static readonly Thing OnOffLight = new Thing
            ("0000", new Channel[] { Channel .Switch});

        public static readonly Thing OnOffPlugIn = new Thing
         ("0010", new Channel[] { Channel.Switch });


        private static readonly Dictionary<string, Thing> typeMap =
             new Dictionary<string, Thing>(StringComparer.OrdinalIgnoreCase)
         {
                {"ZHAPresence",Thing.presencesensor }, {"CLIPPrensence", Thing.presencesensor },
                {"ZHAPower",Thing.powersensor }, {"CLIPPower",Thing.powersensor },
                {"ZHAConsumption",Thing.consumptionsensor},
                {"ZHASwitch",Thing.Switch},
                {"ZHALightLevel",Thing.lightsensor},
                {"ZHATemperature",Thing.temperaturesensor},
                {"ZHAHumidity",Thing.humiditysensor},
                {"ZHAPressure",Thing.pressuresensor},
                {"ZHAOpenClose",Thing.openclosesensor},
                {"ZHAWater",Thing.waterleakagesensor},
                {"ZHAAlarm",Thing.alarmsensor},
                {"ZHAVibration",Thing.vibrationsensor},
                 {"On/Off Light",Thing.OnOffLight},
                 {"On/Off Plug-in Unit",Thing.OnOffPlugIn},
             };


        public Thing(string strName, IEnumerable<Channel> channels)
        {
            Channels = new List<Channel>();
             Channels.AddRange(channels);
            Name = strName;
        }


        public static bool TryGetThing(string strType, out Thing thing)
        {
            return typeMap.TryGetValue(strType, out thing);
        }


        public string Name { get; }
        public List<Channel> Channels { get; }


        public class Channel
        {
            public static readonly Channel last_updated = new Channel { ChannelType = "last_updated", ItemType = "DateTime", Description = "Timestamp when the sensor was last updated" };
            public static readonly Channel battery_level = new Channel { ChannelType = "battery_level", ItemType = "Number", Description = "" };
            public static readonly Channel battery_low = new Channel { ChannelType = "battery_low", ItemType = "Switch", Description = "Battery level low: ON; OFF" };
            public static readonly Channel temperature = new Channel { ChannelType = "temperature", AlexaTpye= "CurrentTemperature", ItemType = "Number:Temperature" };

            public static readonly Channel Switch = new Channel { ChannelType = "switch", ItemType = "Switch", ItemTag= "Switchable" };

            public string AlexaTpye { get; set; }
            public string ItemType { get; set; }
            public string ChannelType { get; set; }
            public string Description { get; set; }

            public string ItemTag { get; set; }
        }
    }
}
