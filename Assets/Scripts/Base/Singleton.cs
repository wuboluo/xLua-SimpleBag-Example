public class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance()
    {
        if (instance == null) instance = new T();

        return instance;
    }
}