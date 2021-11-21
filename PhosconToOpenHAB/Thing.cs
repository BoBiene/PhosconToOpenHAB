using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhosconToOpenHAB
{
    public class Thing
    {
        public static readonly Thing presencesensor = new Thing("presencesensor", "MotionSensor", "MotionDetector", new Channel[] { Channel.presence, Channel.enabled, Channel.dark });
        public static readonly Thing powersensor = new Thing("powersensor", "Other", "PowerOutlet", new Channel[] { Channel.power, Channel.voltage, Channel.current });
        public static readonly Thing consumptionsensor = new Thing("consumptionsensor","Other", "Sensor", new Channel[] { Channel.power, Channel.consumption });
        public static readonly Thing Switch = new Thing("switch", "Other","Sensor", new Channel[] { Channel.button, Channel.gesture });
        public static readonly Thing lightsensor = new Thing("lightsensor", "Other", "Sensor", new Channel[] { Channel.lightlux, Channel.light_level, Channel.dark, Channel.daylight, });
        public static readonly Thing temperaturesensor = new Thing("temperaturesensor", "Other", "Sensor", new Channel[] { Channel.temperature });
        public static readonly Thing humiditysensor = new Thing("humiditysensor", "Other", "Sensor", new Channel[] { Channel.humidity });
        public static readonly Thing pressuresensor = new Thing("pressuresensor", "Other", "Sensor", new Channel[] { Channel.pressure });
        public static readonly Thing openclosesensor = new Thing("openclosesensor", "ContactSensor", "Sensor", new Channel[] { Channel.open });
        public static readonly Thing waterleakagesensor = new Thing("waterleakagesensor", "SecurityPanel", "Sensor", new Channel[] { Channel.waterleakage });
        public static readonly Thing alarmsensor = new Thing("alarmsensor", "SecurityPanel", "Sensor", new Channel[] { Channel.alarm, Channel.vibration });
        public static readonly Thing firesensor = new Thing("firesensor", "SecurityPanel", "SmokeDetector", new Channel[] { Channel.fire });
        public static readonly Thing vibrationsensor = new Thing("vibrationsensor", "Other", "Sensor", new Channel[] { Channel.vibration });
        public static readonly Thing daylightsensor = new Thing("daylightsensor", "Other", "Sensor", new Channel[] { Channel.daylight, Channel.light, Channel.value });
        public static readonly Thing carbonmonoxide = new Thing("carbonmonoxide", "Other", "Sensor", new Channel[] { Channel.carbonmonoxide });
        public static readonly Thing colorcontrol = new Thing("colorcontrol", "Other", "RemoteControl", new Channel[] { Channel.colorState, Channel.button });


        public static readonly Thing LightDimmable = new Thing ("dimmablelight","Light", "Lightbulb", new Channel[] { Channel.brightness, Channel.alert, Channel.ontime });
        public static readonly Thing LightOnOff = new Thing ("onofflight", "Light", "Lightbulb", new Channel[] { Channel.Switch, Channel.ontime });
        public static readonly Thing LightColorTemperature = new Thing("colortemperaturelight", "Light", "Lightbulb", new Channel[] { Channel.brightness, Channel.alert,Channel.color_temperature, Channel.ontime });
        public static readonly Thing LightColor = new Thing("colorlight", "Light", "Lightbulb", new Channel[] { Channel.color, Channel.effect,Channel.effectSpeed,Channel.alert, Channel.ontime });
        public static readonly Thing LightExtendedColor = new Thing("extendedcolorlight", "Light", "Lightbulb", new Channel[] { Channel.color,Channel.color_temperature, Channel.alert, Channel.effect, Channel.effectSpeed, Channel.ontime });
        public static readonly Thing WindowCovering = new Thing("windowcovering", "Light", "Window", new Channel[] { Channel.position });
        public static readonly Thing Thermostat = new Thing("thermostat", "Thermostat", "Sensor", new Channel[] { Channel.heatsetpoint,Channel.valve,Channel.mode, Channel.offset });
        public static readonly Thing Warningdevice = new Thing("warningdevice", "SecurityPanel", "Siren", new Channel[] { Channel.alert });
        public static readonly Thing Doorlock = new Thing("doorlock", "Lock", "Lock", new Channel[] { Channel.Lock });
        public static readonly Thing SwitchOnOff = new Thing("onoffswitch", "Switch", "WallSwitch", new Channel[] { Channel.Switch, Channel.ontime });

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
                {"ZHAFire",Thing.firesensor},
                {"ZHAVibration",Thing.vibrationsensor},
                {"deCONZ specific: simulated sensor",Thing.daylightsensor},
                {"ZHACarbonmonoxide",Thing.carbonmonoxide},
                {"ZBT-Remote-ALL-RGBW",Thing.colorcontrol},
                
                 {"Dimmable light",Thing.LightDimmable},{"Dimmable plug-in unit",Thing.LightDimmable},
                 {"On/Off light",Thing.LightOnOff},
                 {"On/Off plug-in unit",Thing.SwitchOnOff},{"Smart plug",Thing.SwitchOnOff},
                 {"Color temperature light",Thing.LightColorTemperature},
                 {"Color dimmable light",Thing.LightColor},
                 {"Extended color light",Thing.LightExtendedColor},
                 {"Window covering device",Thing.WindowCovering},
                 {"ZHAThermostat",Thing.Thermostat},
                 {"Warning device",Thing.Warningdevice},
                 {"Door Lock",Thing.Doorlock},
             };


        public Thing(string strName, string alexaType, string equipmentType, IEnumerable<Channel> channels)
        {
            Channels = new List<Channel>();
            Channels.AddRange(channels);
            Name = strName;
            AlexaType = alexaType;
            EquipmentType = equipmentType;
        }


        public static bool TryGetThing(string strType, out Thing thing)
        {
            return typeMap.TryGetValue(strType, out thing);
        }
        public static bool TryGetThing(IEnumerable<DeconzSensor> devices, out DeconzSensor primaryDevice, out Thing primaryThing)
        {
            Dictionary<DeconzSensor, Thing> possibleThings = new();
            foreach(var device in devices)
            {
                if (typeMap.TryGetValue(device.Type, out var possibleThing))
                    possibleThings.Add(device,possibleThing);
            }

            var thing = primaryThing = possibleThings.Values.Where(t => t.AlexaType != "Other").FirstOrDefault() ?? possibleThings.Values.FirstOrDefault();
            primaryDevice = possibleThings.Where(kv => kv.Value == thing).FirstOrDefault().Key ?? devices.FirstOrDefault();
            return thing != null;
        }

        public string Name { get; }
        public List<Channel> Channels { get; }
        public string AlexaType { get; set; }
        public string EquipmentType { get; set; }

        public class Channel
        {
            public static readonly Channel presence = new Channel { ChannelType = "presence", ItemType = "Switch", AlexaTpye = "MotionDetectionState", Description = "Status of presence: ON = presence; OFF = no-presence", SemanticModelTag = { "Measurement", "Presence" } };
            public static readonly Channel enabled = new Channel { ChannelType = "enabled", ItemType = "Switch", Description = "This channel activates or deactivates the sensor", SemanticModelTag = { "Status"} };
            public static readonly Channel last_updated = new Channel { ChannelType = "last_updated", ItemType = "DateTime", Description = "Timestamp when the sensor was last updated", SemanticModelTag = { "Status","Timestamp" } };
            public static readonly Channel last_seen = new Channel { ChannelType = "last_seen", ItemType = "DateTime", Description = "Timestamp when the sensor was last seen", SemanticModelTag = { "Status", "Timestamp" } };
            public static readonly Channel power = new Channel { ChannelType = "power", ItemType = "Number:Power", Description = "Current power usage in Watts", SemanticModelTag = { "Measurement", "Power" } };
            public static readonly Channel consumption = new Channel { ChannelType = "consumption", ItemType = "Number:Energy", Description = "Current power usage in Watts/Hour", SemanticModelTag = { "Measurement", "Energy" } };
            public static readonly Channel voltage = new Channel { ChannelType = "voltage", ItemType = "Number:ElectricPotential", Description = "Current voltage in V", SemanticModelTag = { "Measurement", "Voltage" } };
            public static readonly Channel current = new Channel { ChannelType = "current", ItemType = "Number:ElectricCurrent", Description = "Current current in mA", SemanticModelTag = { "Measurement", "Presence" } };
            public static readonly Channel button = new Channel { ChannelType = "button", ItemType = "Number", Description = "Last pressed button id on a switch", SemanticModelTag = { "Measurement", "Current" } };
            public static readonly Channel gesture = new Channel { ChannelType = "gesture", ItemType = "Number", Description = "A gesture that was performed with the", SemanticModelTag = { "Measurement" } };
            public static readonly Channel lightlux = new Channel { ChannelType = "lightlux", ItemType = "Number:Illuminance", Description = "Current light illuminance in Lux", SemanticModelTag = { "Measurement", "Light" } };
            public static readonly Channel light_level = new Channel { ChannelType = "light_level", ItemType = "Number", Description = "Current light level", SemanticModelTag = { "Measurement", "Light" } };
            public static readonly Channel dark = new Channel { ChannelType = "dark", ItemType = "Switch", Description = "Light level is below the darkness threshold" };
            public static readonly Channel daylight = new Channel { ChannelType = "daylight", ItemType = "Switch", Description = "Light level is above the daylight threshold" };
            public static readonly Channel temperature = new Channel { ChannelType = "temperature", ItemType = "Number:Temperature", AlexaTpye = "CurrentTemperature", Description = "Current temperature in ˚C", SemanticModelTag = { "Measurement", "Temperature" } };
            public static readonly Channel humidity = new Channel { ChannelType = "humidity", ItemType = "Number:Dimensionless", AlexaTpye = "CurrentHumidity", Description = "Current humidity in %", SemanticModelTag = { "Measurement", "Humidity" } };
            public static readonly Channel pressure = new Channel { ChannelType = "pressure", ItemType = "Number:Pressure", Description = "Current pressure in hPa", SemanticModelTag = { "Measurement", "Pressure" } };
            public static readonly Channel open = new Channel { ChannelType = "open", ItemType = "Contact", AlexaTpye = "ContactDetectionState", Description = "Status of contacts: OPEN; CLOSED", SemanticModelTag = { "OpenState" } };
            public static readonly Channel waterleakage = new Channel { ChannelType = "waterleakage", ItemType = "Switch",AlexaTpye= "WaterAlarm", Description = "Status of water leakage: ON = water leakage detected; OFF = no water leakage detected" };
            public static readonly Channel fire = new Channel { ChannelType = "fire", ItemType = "Switch",AlexaTpye= "FireAlarm", Description = "Status of a fire: ON = fire was detected; OFF = no fire detected" };
            public static readonly Channel alarm = new Channel { ChannelType = "alarm", ItemType = "Switch",AlexaTpye= "AlarmAlert", Description = "Status of an alarm: ON = alarm was triggered; OFF = no alarm" };
            public static readonly Channel tampered = new Channel { ChannelType = "tampered", ItemType = "Switch", Description = "Status of a zone: ON = zone is being tampered; OFF = zone is not tampered" };
            public static readonly Channel vibration = new Channel { ChannelType = "vibration", ItemType = "Switch", Description = "Status of vibration: ON = vibration was detected; OFF = no vibration" };
            public static readonly Channel light = new Channel { ChannelType = "light", ItemType = "String", Description = "Light level: Daylight; Sunset; Dark" };
            public static readonly Channel value = new Channel { ChannelType = "value", ItemType = "Number", Description = "Sun position: 130 = dawn; 140 = sunrise; 190 = sunset; 210 = dusk" };
            public static readonly Channel battery_level = new Channel { ChannelType = "battery_level", ItemType = "Number",AlexaTpye= "BatteryLevel", Description = "Battery level (in %)", SemanticModelTag = { "Measurement" } };
            public static readonly Channel battery_low = new Channel { ChannelType = "battery_low", ItemType = "Switch", Description = "Battery level low: ON; OFF", SemanticModelTag = { "LowBattery" } };
            public static readonly Channel carbonmonoxide = new Channel { ChannelType = "carbonmonoxide", ItemType = "Switch", Description = "ON = carbon monoxide detected" };
            public static readonly Channel colorState = new Channel { ChannelType = "color", ItemType = "Color", Description = "Color set by remote" };
            public static readonly Channel windowopen = new Channel { ChannelType = "windowopen", ItemType = "Contact", Description = "windowopen status is reported by some thermostats" };

            public static readonly Channel brightness = new Channel { ChannelType = "brightness", ItemType = "Dimmer", AlexaTpye= "PowerState,Brightness", Description = "Brightness of the light", SemanticModelTag = { "Light", "Setpoint" }  };
            public static readonly Channel Switch = new Channel { ChannelType = "switch", ItemType = "Switch",AlexaTpye= "PowerState", Description = "State of a ON/OFF device", SemanticModelTag = { "Switch" } };
            public static readonly Channel color = new Channel { ChannelType = "color", ItemType = "Color", AlexaTpye = "PowerState,Brightness,Color", Description = "Color of an multi-color light", SemanticModelTag = { "Setpoint" } };
            public static readonly Channel color_temperature = new Channel { ChannelType = "color_temperature", AlexaTpye= "ColorTemperature", ItemType = "Number", Description = "Color temperature in Kelvin. The value range is determined by each individual light", SemanticModelTag = { "ColorTemperature", "Control" } };
            public static readonly Channel effect = new Channel { ChannelType = "effect", ItemType = "String", Description = "Effect selection. Allowed commands are set dynamically" };
            public static readonly Channel effectSpeed = new Channel { ChannelType = "effectSpeed", ItemType = "Number", Description = "Effect Speed" };
            public static readonly Channel Lock = new Channel { ChannelType = "lock", ItemType = "Switch", Description = "Lock (ON) or unlock (OFF) the doorlock" };
            public static readonly Channel ontime = new Channel { ChannelType = "ontime", ItemType = "Number:Time", Description = "Timespan for which the light is turned on" };
            public static readonly Channel position = new Channel { ChannelType = "position", ItemType = "Rollershutter", Description = "Position of the blind" };
            public static readonly Channel heatsetpoint = new Channel { ChannelType = "heatsetpoint", ItemType = "Number:Temperature", Description = "Target Temperature in °C" };
            public static readonly Channel valve = new Channel { ChannelType = "valve", ItemType = "Number:Dimensionless", Description = "Valve position in %" };
            public static readonly Channel mode = new Channel { ChannelType = "mode", ItemType = "String", Description = "Mode: \"auto\", \"heat\" and \"off\"" };
            public static readonly Channel offset = new Channel { ChannelType = "offset", ItemType = "Number", Description = "Temperature offset for sensor" };
            public static readonly Channel alert = new Channel { ChannelType = "alert", ItemType = "String", Description = "Turn alerts on. Allowed commands are none, select (short blinking), lselect (long blinking)", SemanticModelTag = { "Control" } };
            public static readonly Channel all_on = new Channel { ChannelType = "all_on", ItemType = "Switch", Description = "All lights in group are on" };
            public static readonly Channel any_on = new Channel { ChannelType = "any_on", ItemType = "Switch", Description = "Any light in group is on" };
            public static readonly Channel scene = new Channel { ChannelType = "scene", ItemType = "String", Description = "Recall a scene. Allowed commands are set dynamically" };

            public string AlexaTpye { get; set; }
            public string ItemType { get; set; }
            public string ChannelType { get; set; }
            public string Description { get; set; }
            public IList<string> SemanticModelTag { get; set; } = new List<string>();
        }
    }
}
