/// <summary>
/// Provides timer utilities for invoking delayed actions.
/// </summary>
public static class Timer
{
    /// <summary>
    /// Invokes the specified action after a delay.
    /// </summary>
    /// <param name="methodName">Action to invoke.</param>
    /// <param name="delay">Delay in seconds.</param>
    public static void InvokeAfter(string methodName, float delay)
    {
        if (!string.IsNullOrEmpty(methodName))
        {
            TimerBehaviour.Instance.Invoke(methodName, delay);
        }
    }
}
