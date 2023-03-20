namespace GameEngine.engine.helper; 

public static class ListUtils {
    public static List<T> Of<T>(params T[] entries) {
        return new List<T>(entries);
    }

    public static List<T> Empty<T>() {
        return new List<T>();
    }
}