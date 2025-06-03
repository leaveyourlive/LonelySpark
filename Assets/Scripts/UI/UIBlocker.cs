using UnityEngine;

public static class UIBlocker
{
    private static int openCount = 0;

    // true, если хоть один UIToggle открыт
    public static bool IsAnyMenuOpen => openCount > 0;

    // вызываем при открытии
    public static void NotifyUIOpened()
    {
        openCount = Mathf.Max(0, openCount + 1);
    }

    // вызываем при закрытии
    public static void NotifyUIClosed()
    {
        openCount = Mathf.Max(0, openCount - 1);
    }
}