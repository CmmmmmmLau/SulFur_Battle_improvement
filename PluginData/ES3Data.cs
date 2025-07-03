namespace BattleImprove.PluginData;

public abstract class ES3Data<T> where T: class, new() {
    private static readonly ES3Settings settings = new ES3Settings("BattleImprove.es3");
    private static readonly string key = typeof(T).Name;
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = ES3.KeyExists(key, settings)? ES3.Load<T>(key, settings) : new T();
            }
            return instance;
        }
    }
    
    public static void Save() {
        ES3.Save(key, instance, settings);
    }
}