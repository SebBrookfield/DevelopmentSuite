using System;
using System.Threading;
using System.Threading.Tasks;

namespace Development.Suite.App.ExtensionMethods;

public static class ManualResetEventExtensions
{
    public static Task WaitAsync(this ManualResetEventSlim manualResetEventSlim, TimeSpan timeout)
    {
        var taskCompletionSource = new TaskCompletionSource();

        var registration = ThreadPool.RegisterWaitForSingleObject(manualResetEventSlim.WaitHandle, (state, timedOut) =>
        {
            if (state is not TaskCompletionSource tcs)
                return;

            if (timedOut)
                tcs.TrySetCanceled();
            else
                tcs.TrySetResult();

        }, taskCompletionSource, (int)timeout.TotalMilliseconds, executeOnlyOnce: true);
        
        return taskCompletionSource.Task.ContinueWith((_, state) =>
        {
            ((RegisteredWaitHandle?)state)?.Unregister(null);
        }, registration, TaskScheduler.Default);
    }
}