using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class MoreAsync
{
    public static async Task WithCancellation(this Task task, bool throwIfCancelled, CancellationToken ct)
    {
        TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        try
        {
            using (ct.Register(delegate (object s)
            {
                ((TaskCompletionSource<bool>)s).TrySetResult(result: true);
            }, taskCompletionSource))
            {
                await Task.WhenAny(task, taskCompletionSource.Task).ConfigureAwait(continueOnCapturedContext: false);
                if (ct.IsCancellationRequested)
                {
                    if (throwIfCancelled)
                    {
                        ct.ThrowIfCancellationRequested();
                    }

                    return;
                }
            }
        }
        catch (ObjectDisposedException)
        {
            await task.ConfigureAwait(continueOnCapturedContext: false);
        }

        await task.ConfigureAwait(continueOnCapturedContext: false);
    }

    public static async Task<T> WithCancellation<T>(this Task<T> task, bool throwIfCancelled, CancellationToken ct)
    {
        TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        try
        {
            using (ct.Register(delegate (object s)
            {
                ((TaskCompletionSource<bool>)s).TrySetResult(result: true);
            }, taskCompletionSource))
            {
                await Task.WhenAny(task, taskCompletionSource.Task).ConfigureAwait(continueOnCapturedContext: false);
                if (ct.IsCancellationRequested)
                {
                    if (throwIfCancelled)
                    {
                        ct.ThrowIfCancellationRequested();
                    }

                    return default(T);
                }
            }
        }
        catch (ObjectDisposedException)
        {
            return await task.ConfigureAwait(continueOnCapturedContext: false);
        }

        return await task.ConfigureAwait(continueOnCapturedContext: false);
    }

    public static async void FireAndForgetTask(this Task asyncTask, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
    {
        try
        {
            await asyncTask.ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"Fire and forget task was cancelled ({line}@{file}: {member})");
        }
    }

    public static void BusyWait(this Task task, Action idle)
    {
        while (!task.IsCompleted)
        {
            idle();
        }
    }

    public static bool TryRunSynchronously(this Task task)
    {
        if (task == null || task.Status != 0)
        {
            return false;
        }

        try
        {
            task.RunSynchronously();
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public static void CancelAndDispose(ref CancellationTokenSource tokenSource)
    {
        tokenSource?.Cancel();
        tokenSource?.Dispose();
        tokenSource = null;
    }

    public static void CancelAndRenew(ref CancellationTokenSource tokenSource)
    {
        CancellationTokenSource cancellationTokenSource = Interlocked.Exchange(ref tokenSource, new CancellationTokenSource());
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    public static void CancelAndRenew(ref CancellationTokenSource tokenSource, TimeSpan newDelay)
    {
        CancellationTokenSource cancellationTokenSource = Interlocked.Exchange(ref tokenSource, new CancellationTokenSource(newDelay));
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    public static async Task RunUntil(Func<bool> condition, CancellationToken ct)
    {
        if (condition())
        {
            return;
        }

        await Task.Run(delegate
        {
            while (!ct.IsCancellationRequested && !condition())
            {
            }
        }, ct).ConfigureAwait(continueOnCapturedContext: false);
    }

    public static async Task RunUntil(Func<bool> condition, TimeSpan interval, CancellationToken ct)
    {
        if (!condition())
        {
            while (!ct.IsCancellationRequested && !condition())
            {
                await Task.Delay(interval, ct).ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }

    public static TResult RunTaskAndContinueWith<TResult>(Func<CancellationToken, TResult> taskFunc, Action<TResult> onContinueAction, CancellationToken cancellationToken)
    {
        Task<TResult> task = Task.Factory.StartNew(() => taskFunc(cancellationToken), cancellationToken);
        task.ContinueWith(delegate (Task<TResult> t)
        {
            if (onContinueAction != null && !t.IsCanceled && !t.IsFaulted)
            {
                SynchronizationContext.Current.Post(delegate
                {
                    onContinueAction(t.Result);
                }, null);
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
        task.Wait();
        return task.Result;
    }
}