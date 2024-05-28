namespace Nice.Dotnet.ConsoleApp
{
    internal class DiskDeviceInfo
    {
        public DiskDeviceInfo()
        {
        }
        /// <summary>
        /// 设备根路径
        /// </summary>
        public string RootPath => $"{Name}\\";
        public string Name { get; set; }
        public string VolumeName { get; set; }
        public string Caption { get; set; }
        public string Size { get; set; }
        public string PNPDeviceID { get; set; }
        public string SerialNumber { get; internal set; }
    }
}