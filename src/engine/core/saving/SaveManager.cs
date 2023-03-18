using Newtonsoft.Json;

namespace Worms.engine.core.saving; 

public static class SaveManager {
    private static readonly JsonSerializerSettings SETTINGS = new() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
    
    public static void Save<T>(string name, T data) {
        Directory.CreateDirectory(SaveDirectory());
        string dataString = JsonConvert.SerializeObject(data, SETTINGS);
        File.WriteAllText(FilePath(name), dataString);
    }

    public static T Load<T>(string name) {
        string data = File.ReadAllText(FilePath(name));
        return JsonConvert.DeserializeObject<T>(data, SETTINGS)!;
    }

    private static string FilePath(string name) {
        return $"{SaveDirectory()}\\{name}.txt";
    }
    
    private static string SaveDirectory() {
        return $"{Directory.GetCurrentDirectory()}\\saveData";
    }
}