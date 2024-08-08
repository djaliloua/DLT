namespace ManagPassWord.CustomClasses
{
    public static class DeviceUtility
    {
        public static bool IsVirtual => DeviceInfo.Current.DeviceType switch
        {
            DeviceType.Physical => false,
            DeviceType.Virtual => true,
            _ => false
        };
        public static bool IsAndroid => DeviceInfo.Current.Platform == DevicePlatform.Android;
        public static bool IsDeskTop => DeviceInfo.Current.Platform == DevicePlatform.WinUI;
        public static string DeviceModel => DeviceInfo.Current.Model;
        public static string DeviceManufacturer => DeviceInfo.Current.Manufacturer;
        public static string DeviceName => DeviceInfo.Current.Name;
        public static DeviceIdiom DeviceIdiom => DeviceInfo.Current.Idiom;
        public static DevicePlatform DevicePlatform => DeviceInfo.Current.Platform;
        public static string DeviceVersion => DeviceInfo.Current.VersionString;

    }
}
