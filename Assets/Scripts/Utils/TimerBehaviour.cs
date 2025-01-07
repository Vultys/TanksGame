using UnityEngine;

/// <summary>
/// Singleton MonoBehaviour to manage timer-based invocations and coroutines.
/// </summary>
public class TimerBehaviour : MonoBehaviour
{
    private static TimerBehaviour _instance;

    public static TimerBehaviour Instance
    {
        get
        {
            if (_instance == null)
            {
                var gameObject = new GameObject("TimerBehaviour");
                _instance = gameObject.AddComponent<TimerBehaviour>();
                DontDestroyOnLoad(gameObject);
            }
            return _instance;
        }
    }
}
