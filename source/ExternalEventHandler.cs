namespace Craftify.Revit.Extensions;

public class ExternalEventHandler(Action<Exception>? exceptionHandler = default)
    : IExternalEventHandler
{
    private Func<UIApplication, object>? _function;
    private Action<UIApplication>? _action;
    private Action<Exception>? _exceptionHandler = exceptionHandler;
    private Action? _finallyHandler;

    public void SetExecutionCommand(Func<UIApplication, object> function)
    {
        _function = function;
        _action = null;
    }

    public void SetExecutionCommand(Action<UIApplication> action)
    {
        _action = action;
        _function = null;
    }

    public void Catch(Action<Exception> exceptionHandler) => _exceptionHandler = exceptionHandler;

    public void Finally(Action finallyHandler) => _finallyHandler = finallyHandler;

    public void Execute(UIApplication app)
    {
        try
        {
            _function?.Invoke(app);
            _action?.Invoke(app);
        }
        catch (Exception exception)
        {
            _exceptionHandler?.Invoke(exception);
        }
        finally
        {
            _finallyHandler?.Invoke();
        }
    }

    public string GetName() => ToString();
}
