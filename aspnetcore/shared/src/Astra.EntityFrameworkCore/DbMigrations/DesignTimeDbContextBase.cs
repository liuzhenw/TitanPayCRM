using Astra.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Astra.EntityFrameworkCore.DbMigrations;

public abstract class DesignTimeDbContextBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    public abstract TDbContext CreateDbContext(string[] args);
    
    protected static string GetConnectionStringFromProject(string projectName, string connectionStringName = "Default")
    {
        var projectDirectory = PathHelper.FindDirectory(PathHelper.GetSolutionDirectory(), projectName);
        if (projectDirectory == null)
            throw new DirectoryNotFoundException($"项目文件夹 '{projectName}' 未找到!");

        if (!Path.Exists(Path.Combine(projectDirectory.FullName, "appsettings.json")))
            throw new FileNotFoundException($"项目文件夹 '{projectName}' 下未找到 'appsettings.json' 文件!");

        return GetConnectionStringFromPath(projectDirectory.FullName);
    }

    protected static string GetConnectionStringFromPath(string path, string connectionStringName = "Default")
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.Development.json", false)
            .AddAppSettingsSecretsJson(reloadOnChange: false)
            .Build();
        var connectionString = configuration.GetConnectionString(connectionStringName);
        return connectionString ?? throw new KeyNotFoundException($"数据库连接字符串 '{connectionStringName}' 未找到!");
    }
}