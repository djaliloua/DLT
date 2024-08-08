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

    }
}
