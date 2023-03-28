using System.Runtime.InteropServices;

namespace GameEngine.engine.window; 

internal static class WindowApi {
    private const string LIB = "user32.dll";

    public const int CLICK_MESSAGE = 0x0111;
    public const uint MENU_FLAG_STRING = 0x00000000;
    public const uint MENU_FLAG_GRAYED = 0x00000001;
    public const uint MENU_FLAG_CHECKED = 0x00000008;
    public const uint MENU_FLAG_POPUP = 0x00000010;
    public const uint MENU_FLAG_SEPARATOR = 0x00000800;
    
    [DllImport(LIB)]
    public static extern nint CreateMenu();

    [DllImport(LIB)]
    public static extern bool AppendMenu(nint hMenu, uint uFlags, uint uIDNewItem = 0, string? lpNewItem = null);

    [DllImport(LIB)]
    public static extern bool SetMenu(nint hWnd, nint hMenu);
    
    [DllImport(LIB)]
    public static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

    public static void CheckMenuItem(nint hMenu, uint uIDCheckItem, bool check) {
        CheckMenuItem(hMenu, uIDCheckItem, check ? MENU_FLAG_CHECKED : 0);
    }
    
    [DllImport(LIB)]
    private static extern nint CheckMenuItem(nint hMenu, uint uIDCheckItem, uint uCheck);
    
    public static void ChangeMenuItemDisableState(nint hMenu, uint itemId, bool disabled) {
        EnableMenuItem(hMenu, itemId, disabled ? MENU_FLAG_GRAYED : 0);
    }
    
    [DllImport(LIB)]
    private static extern nint EnableMenuItem(nint hMenu, uint uIDEnableItem, uint uEnable);
}
