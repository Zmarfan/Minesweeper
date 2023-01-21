namespace Worms.engine.logger; 

public static class Logger {
    public static void Error(Exception e, string text = "") {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{text}; exception: {e}");
    }
    
    public static void Warning(string text, Exception e) {
        Console.BackgroundColor = ConsoleColor.DarkYellow;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{text}; exception: {e}");
    }
}