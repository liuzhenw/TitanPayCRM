namespace Astra.Common
{
    public static class PathHelper
    {
        /// <summary>
        /// 获取解决方案根目录
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetSolutionDirectory()
        {
            var currentDir = Environment.CurrentDirectory;
            while (true)
            {
                var slnFile = new DirectoryInfo(currentDir)
                    .GetFiles()
                    .FirstOrDefault(f => f is { Exists: true, Extension: ".sln" });
                if (slnFile != null)
                    return currentDir;

                var parent = Directory.GetParent(currentDir);
                if (parent == null)
                    throw new Exception("未找到项目文件(*.sln)!");

                currentDir = parent.FullName;
            }
        }

        /// <summary>
        /// 从给定位置查找子文件夹
        /// </summary>
        /// <param name="location"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DirectoryInfo? FindDirectory(string location, string name)
        {
            if (!Directory.Exists(location))
                return null;

            var rootDirectory = new DirectoryInfo(location);
            while (true)
            {
                var subDirectories = rootDirectory.GetDirectories();
                foreach (var subDirectory in subDirectories)
                {
                    if (subDirectory.Name == name)
                        return subDirectory;

                    var result = FindDirectory(subDirectory.FullName, name);
                    if (result != null)
                        return result;
                }

                return null;
            }
        }
    }
}