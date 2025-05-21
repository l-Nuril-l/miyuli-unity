using UnityEngine;

public static class PlatformChecker
{
    public static bool IsMobilePlatform()
    {
#if UNITY_EDITOR
        return false;
#else
            return Application.isMobilePlatform;
#endif
    }
}
