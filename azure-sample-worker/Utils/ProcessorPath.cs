using System;
namespace Utils
{

    public class ProcessorPath
    {
        // string root = RoleEnvironment.GetLocalResource("LocalStorage").RootPath;

        public static string root = Environment.GetEnvironmentVariable("TEMP") + @"\";
        public static string srcDirPath = root + @"Src\";
        public static string dstDirPath = root + @"Dst\";

        public static string binPath = @"D:\home\Bin\";
    }

}
