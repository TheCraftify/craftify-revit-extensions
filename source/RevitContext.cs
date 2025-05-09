using InvalidOperationException = System.InvalidOperationException;

namespace Craftify.Revit.Extensions;

#pragma warning disable S3881
public class RevitContext : IDisposable
{
    private readonly ExternalEventHandler _externalEventHandler;
    private readonly ExternalEvent _externalEvent;

    private static RevitContext _instance = default!;
    private static readonly object _lock = new();

    private RevitContext()
    {
        _externalEventHandler = new ExternalEventHandler();
        _externalEvent = ExternalEvent.Create(_externalEventHandler);
    }

    public static TResult Execute<TResult>(
        Func<UIApplication, TResult> function,
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default
    )
    {
        EnsureCreated();
        return _instance.InstanceExecute(function, exceptionHandler, finallyHandler);
    }

    public static void Execute(
        Action<UIApplication> action,
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default
    )
    {
        EnsureCreated();
        _instance.InstanceExecute(action, exceptionHandler, finallyHandler);
    }

    public static Task<TResult> ExecuteAsync<TResult>(
        Func<UIApplication, TResult> function,
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default,
        CancellationToken cancellationToken = default
    )
    {
        EnsureCreated();
        return _instance.InstanceExecuteAsync(
            function,
            exceptionHandler,
            finallyHandler,
            cancellationToken
        );
    }

    public static Task ExecuteAsync(
        Action<UIApplication> action,
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default,
        CancellationToken cancellationToken = default
    )
    {
        EnsureCreated();
        return _instance.InstanceExecuteAsync(
            action,
            exceptionHandler,
            finallyHandler,
            cancellationToken
        );
    }

    private Task<TResult> InstanceExecuteAsync<TResult>(
        Func<UIApplication, TResult> function,
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default,
        CancellationToken cancellationToken = default
    )
    {
        var taskCompletionSource = new TaskCompletionSource<TResult>(
            TaskCreationOptions.RunContinuationsAsynchronously
        );

        using var registration = cancellationToken.Register(() =>
        {
            taskCompletionSource.TrySetCanceled(cancellationToken);
        });

        SetHandlers(
            exception =>
            {
                exceptionHandler?.Invoke(exception);
                taskCompletionSource.TrySetException(exception);
            },
            () =>
            {
                finallyHandler?.Invoke();
                if (!taskCompletionSource.Task.IsCompleted)
                {
                    taskCompletionSource.TrySetCanceled();
                }
            }
        );

        _externalEventHandler.SetExecutionCommand(uiApp =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                taskCompletionSource.TrySetCanceled(cancellationToken);
                return;
            }

            try
            {
                var result = function(uiApp) ?? throw new InvalidOperationException();
                taskCompletionSource.TrySetResult(result);
            }
            catch (Exception exception)
            {
                taskCompletionSource.TrySetException(exception);
            }
        });

        _externalEvent.Raise();
        return taskCompletionSource.Task;
    }

    private Task InstanceExecuteAsync(
        Action<UIApplication> action,
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default,
        CancellationToken cancellationToken = default
    )
    {
        var taskCompletionSource = new TaskCompletionSource<object?>(
            TaskCreationOptions.RunContinuationsAsynchronously
        );

        using var registration = cancellationToken.Register(() =>
        {
            taskCompletionSource.TrySetCanceled(cancellationToken);
        });

        SetHandlers(
            exception =>
            {
                exceptionHandler?.Invoke(exception);
                taskCompletionSource.TrySetException(exception);
            },
            () =>
            {
                finallyHandler?.Invoke();
                if (!taskCompletionSource.Task.IsCompleted)
                {
                    taskCompletionSource.TrySetCanceled();
                }
            }
        );

        _externalEventHandler.SetExecutionCommand(uiApp =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                taskCompletionSource.TrySetCanceled(cancellationToken);
                return;
            }

            try
            {
                action(uiApp);
                taskCompletionSource.TrySetResult(null);
            }
            catch (Exception exception)
            {
                taskCompletionSource.TrySetException(exception);
            }
        });

        _externalEvent.Raise();
        return taskCompletionSource.Task;
    }

    private TResult InstanceExecute<TResult>(
        Func<UIApplication, TResult> function,
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default
    )
    {
        TResult? result = default!;
        SetHandlers(exceptionHandler, finallyHandler);

        _externalEventHandler.SetExecutionCommand(uiApplication =>
            result = function(uiApplication) ?? throw new InvalidOperationException()
        );

        _externalEvent.Raise();
        return result;
    }

    private void InstanceExecute(
        Action<UIApplication> action,
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default
    )
    {
        SetHandlers(exceptionHandler, finallyHandler);
        _externalEventHandler.SetExecutionCommand(uiApplication => action(uiApplication));
        _externalEvent.Raise();
    }

    private static void EnsureCreated()
    {
        if (_instance is null)
        {
            lock (_lock)
            {
                _instance ??= new RevitContext();
            }
        }
    }

    private void SetHandlers(
        Action<Exception>? exceptionHandler = default,
        Action? finallyHandler = default
    )
    {
        if (exceptionHandler is not null)
        {
            _externalEventHandler.Catch(exceptionHandler);
        }
        if (finallyHandler is not null)
        {
            _externalEventHandler.Finally(finallyHandler);
        }
    }

    public void Dispose() => _externalEvent.Dispose();
}
