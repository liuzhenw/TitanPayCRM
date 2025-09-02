using Serilog;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Astra.DbMigrations
{
    public abstract class PendingMigrationsCheckerBase : ITransientDependency
    {
        protected static async Task TryAsync(Func<Task> task, int retryCount = 3)
        {
            while (true)
            {
                try
                {
                    await task();
                    return;
                }
                catch (Exception ex)
                {
                    retryCount--;
                    if (retryCount <= 0)
                    {
                        throw;
                    }

                    Log.Warning(
                        """
                        数据库迁移抛出异常: {ExceptionType}, 该操作还将尝试 {RetryCount} 次. 
                        异常信息: {Message}
                        {StackTrace}
                        """,
                        ex.GetType().Name, retryCount, ex.Message, ex.StackTrace);

                    await Task.Delay(TimeSpan.FromSeconds(RandomHelper.GetRandom(5, 15)));
                }
            }
        }
    }
}