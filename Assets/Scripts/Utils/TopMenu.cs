#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace RandomPlatformer.Utils
{
#if UNITY_EDITOR
    public class TopMenu
    {
        [MenuItem("RandomPlatformer/Play")]
        public static void OpenMainScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
            EditorApplication.EnterPlaymode();
        }
    }
#endif
}