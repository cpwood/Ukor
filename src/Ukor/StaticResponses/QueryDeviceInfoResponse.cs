using System.Xml.Serialization;

namespace Ukor.StaticResponses
{
    [XmlType("device-info")]
    public class QueryDeviceInfoResponse
    {
        [XmlElement("udn")] public string Udn { get; set; } = "29600009-5406-1005-8080-1234567890ab";
        [XmlElement("serial-number")] public string SerialNumber { get; set; } = "YH009E000001";
        [XmlElement("device-id")] public string DeviceId { get; set; } = "E43979390000";

        [XmlElement("advertising-id")]
        public string AdvertisingId { get; set; } = "a5f89592-c53c-5ea5-9f84-dc663195d91d";

        [XmlElement("vendor-name")] public string VendorName { get; set; } = "Roku";
        [XmlElement("model-name")] public string ModelName { get; set; } = "Ukor Server";
        [XmlElement("model-number")] public string ModelNumber { get; set; } = "3810EU";
        [XmlElement("model-region")] public string ModelRegion { get; set; } = "US";
        [XmlElement("is-tv")] public bool IsTv { get; set; }
        [XmlElement("is-stick")] public bool IsStick { get; set; } = true;
        [XmlElement("supports-ethernet")] public bool SupportsEthernet { get; set; }
        [XmlElement("wifi-mac")] public string WifiMac { get; set; } = "ac:ae:01:02:03:04";
        [XmlElement("wifi-driver")] public string WifiDriver { get; set; } = "realtek";
        [XmlElement("network-type")] public string NetworkType { get; set; } = "wifi";
        [XmlElement("network-name")] public string NetworkName { get; set; } = "not-real";
        [XmlElement("friendly-device-name")] public string FriendlyDeviceName { get; set; } = "Ukor Server";
        [XmlElement("friendly-model-name")] public string FriendlyModelName { get; set; } = "Ukor Server";

        [XmlElement("default-device-name")]
        public string DefaultDeviceName { get; set; } = "Ukor Server - YH009E000001";

        [XmlElement("user-device-name")] public string UserDeviceName { get; set; } = "Ukor Server";
        [XmlElement("user-device-location")] public string UserDeviceLocation { get; set; } = "The moon.";
        [XmlElement("build-number")] public string BuildNumber { get; set; } = "509.20E04807A";
        [XmlElement("software-version")] public string SoftwareVersion { get; set; } = "9.2.0";
        [XmlElement("software-build")] public string SoftwareBuild { get; set; } = "4807";
        [XmlElement("secure-device")] public bool SecureDevice { get; set; } = true;
        [XmlElement("language")] public string Language { get; set; } = "en";
        [XmlElement("country")] public string Country { get; set; } = "GB";
        [XmlElement("locale")] public string Locale { get; set; } = "en_GB";
        [XmlElement("time-zone-auto")] public bool TimeZoneAuto { get; set; } = true;
        [XmlElement("time-zone")] public string TimeZone { get; set; } = "Europe/United Kingdom";
        [XmlElement("time-zone-name")] public string TimeZoneName { get; set; } = "Europe/United Kingdom";
        [XmlElement("time-zone-tz")] public string TimeZoneTz { get; set; } = "Europe/London";
        [XmlElement("time-zone-offset")] public int TimeZoneOffset { get; set; } = 60;
        [XmlElement("clock-format")] public string ClockFormat { get; set; } = "24-hour";
        [XmlElement("uptime")] public int Uptime { get; set; } = 100;
        [XmlElement("power-mode")] public string PowerMode { get; set; } = "PowerOn";
        [XmlElement("supports-suspend")] public bool SupportsSuspend { get; set; }
        [XmlElement("supports-find-remote")] public bool SupportsFindRemote { get; set; } = true;
        [XmlElement("find-remote-is-possible")] public bool FindRemoteIsPossible { get; set; }
        [XmlElement("supports-audio-guide")] public bool SupportsAudioGuide { get; set; }
        [XmlElement("developer-enabled")] public bool DeveloperEnabled { get; set; }
        [XmlElement("keyed-developer-id")] public string KeyedDeveloperId { get; set; } = string.Empty;
        [XmlElement("search-enabled")] public bool SearchEnabled { get; set; } = true;

        [XmlElement("search-channels-enabled")]
        public bool SearchChannelsEnabled { get; set; } = true;

        [XmlElement("voice-search-enabled")] public bool VoiceSearchEnabled { get; set; } = true;
        [XmlElement("notifications-enabled")] public bool NotificationsEnabled { get; set; } = true;
        [XmlElement("notifications-first-use")] public bool NotificationsFirstUse { get; set; }

        [XmlElement("supports-private-listening")]
        public bool SupportsPrivateListening { get; set; } = true;

        [XmlElement("headphones-connected")] public bool HeadphonesConnected { get; set; }
        [XmlElement("supports-ecs-textedit")] public bool SupportsEcsTextEdit { get; set; } = true;
        [XmlElement("supports-ecs-microphone")] public bool SupportsEcsMicrophone { get; set; } = true;
        [XmlElement("supports-wake-on-wlan")] public bool SupportsWakeOnWlan { get; set; }
        [XmlElement("has-play-on-roku")] public bool HasPlayOnRoku { get; set; } = true;
        [XmlElement("has-mobile-screensaver")] public bool HasMobileScreensaver { get; set; }
        [XmlElement("support-url")] public string SupportUrl { get; set; } = "github.com/cpwood/ukor";
        [XmlElement("grandcentral-version")] public string GrandCentralVersion { get; set; } = "2.9.57";
        [XmlElement("has-wifi-extender")] public bool HasWifiExtender { get; set; }
        [XmlElement("has-wifi-5G-support")] public bool HasWifi5gSupport { get; set; } = true;
        [XmlElement("can-use-wifi-extender")] public bool CanUseWifiExtender { get; set; } = true;

    }
}
