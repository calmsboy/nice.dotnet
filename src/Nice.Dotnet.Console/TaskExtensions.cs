namespace Nice.Dotnet.ConsoleApp;

public static class TaskExtensions
{
    /// <summary>
    /// 任务超时取消
    /// </summary>
    /// <param name="func">业务任务(超时要取消任务的话 需要在耗时操作之前 判断cts如果取消就结束方法)</param>
    /// <param name="timeoutSeconds">超时时间 秒</param>
    /// <param name="cts">任务取消令牌</param>
    /// <returns>true执行成功 false超时取消</returns>
    public static async Task<bool> TimeoutCancelAsync(
        Func<CancellationTokenSource, Task> func, double timeoutSeconds, CancellationTokenSource cts)
    {
        Task task = func.Invoke(cts);
        Task delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), cts.Token);
        Task completeTask = await Task.WhenAny(task, delayTask);
        if (completeTask == task)
            return true;
        cts.Cancel();
        Console.WriteLine("[TimeoutCancelAsync]任务执行超时已取消。");
        return false;
    }

    /// <summary>
    /// 任务超时取消 (带泛型返回值)
    /// </summary>
    /// <param name="func">业务任务带返回值(超时要取消任务的话 需要在耗时操作之前 判断cts如果取消就结束方法)</param>
    /// <param name="timeoutSeconds">超时时间 秒</param>
    /// <param name="cts">任务取消令牌</param>
    /// <returns>IsSuccess:true执行成功 false超时取消  Result:任务执行成功的结果</returns>
    public static async Task<(bool IsSuccess, T? Result)> TimeoutCancelAsync<T>(
        Func<CancellationTokenSource, Task<T?>> func, double timeoutSeconds, CancellationTokenSource cts)
    {
        Task<T?> task = func.Invoke(cts);
        Task<T?> delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), cts.Token)
                .ContinueWith(_ => default(T?));
        Task completeTask = await Task.WhenAny<T?>(task, delayTask);
        if (completeTask == task)
            return (true, task.Result);
        cts.Cancel();
        Console.WriteLine("[TimeoutCancelAsync]任务执行超时已取消。");
        return (false, delayTask.Result);
    }


    /// <summary>
    /// 任务超时取消 然后重新执行
    /// </summary>
    /// <param name="func">业务任务(超时要取消任务的话 需要在耗时操作之前 判断cts如果取消就结束方法)</param>
    /// <param name="timeoutSeconds">超时时间 秒</param>
    /// <param name="maxRetryCount">最大重试次数</param>
    /// <param name="cts">任务取消令牌</param>
    /// <returns>是否成功</returns>
    public static async Task<bool> TimeoutRetryAsync(
        Func<CancellationTokenSource, Task> func, double timeoutSeconds, int maxRetryCount, CancellationTokenSource cts)
    {
        for (int i = 0; i <= maxRetryCount; i++)
        {
            if (cts.IsCancellationRequested)
                break;
            if (i > 0)
                Console.WriteLine($"[TimeoutRetryAsync]任务第{i}次重试开始...");
            CancellationTokenSource currentCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
            Task task = func.Invoke(currentCts);
            Task delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), currentCts.Token);
            Task completeTask = await Task.WhenAny(task, delayTask);
            if (completeTask == task)
            {
                currentCts.Dispose();
                return true;
            }
            currentCts.Cancel();
            Console.WriteLine("[TimeoutRetryAsync]任务执行超时已取消。");
        }
        return false;
    }

    /// <summary>
    /// 任务超时取消 然后重新执行 (带泛型返回值)
    /// </summary>
    /// <param name="func">业务任务带返回值(超时要取消任务的话 需要在耗时操作之前 判断cts如果取消就结束方法)</param>
    /// <param name="timeoutSeconds">超时时间 秒</param>
    /// <param name="maxRetryCount">最大重试次数</param>
    /// <param name="cts">任务取消令牌</param>
    /// <returns>IsSuccess:是否成功  Result:任务执行成功的结果</returns>
    public static async Task<(bool IsSuccess, T? Result)> TimeoutRetryAsync<T>(
        Func<CancellationTokenSource, Task<T?>> func, double timeoutSeconds, int maxRetryCount, CancellationTokenSource cts)
    {
        for (int i = 0; i <= maxRetryCount; i++)
        {
            if (cts.IsCancellationRequested)
                break;
            if (i > 0)
                Console.WriteLine($"[TimeoutRetryAsync]任务第{i}次重试开始...");
            CancellationTokenSource currentCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
            Task<T?> task = func.Invoke(currentCts);
            Task<T?> delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), currentCts.Token)
                .ContinueWith(_ => default(T?));
            Task completeTask = await Task.WhenAny<T?>(task, delayTask);
            if (completeTask == task)
            {
                currentCts.Dispose();
                return (true, await task);
            }
            currentCts.Cancel();
            Console.WriteLine("[TimeoutRetryAsync]任务执行超时已取消。");
        }
        return (false, default(T));
    }
}

