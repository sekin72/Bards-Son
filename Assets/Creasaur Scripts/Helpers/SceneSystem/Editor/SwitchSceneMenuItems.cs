using UnityEditor;
using UnityEditor.SceneManagement;

public static class SwitchSceneMenuItems
{
    private const string MenuPrefix = "SwitchScene/";
    private const int SceneOpenButtonsPriority = 2000;
    private const string ScenesPath = "Assets/Scenes/";

    [MenuItem(MenuPrefix + "Splash", false, SceneOpenButtonsPriority)]
    public static void SplashScreen()
    {
        OpenScene(ScenesPath + "Splash.unity");
    }
    
    [MenuItem(MenuPrefix + "Opening", false, SceneOpenButtonsPriority)]
    public static void OpenLoading()
    {
        OpenScene(ScenesPath + "Opening.unity");
    }

    [MenuItem(MenuPrefix + "Loading", false, SceneOpenButtonsPriority)]
    public static void OpenCommon()
    {
        OpenScene(ScenesPath + "Loading.unity");
    }

    [MenuItem(MenuPrefix + "Game", false, SceneOpenButtonsPriority)]
    public static void OpenMainMenu()
    {
        OpenScene(ScenesPath + "Game.unity");
    }
    
    private static void OpenScene(string path)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(path);
        }
    }
}
