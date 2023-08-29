namespace CombatDicesTeam.Combats;

public sealed class Singleton<T> where T : class, new()
{
    private static readonly object _lock = new object();
    private static T? _instance;

    private Singleton() { }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}