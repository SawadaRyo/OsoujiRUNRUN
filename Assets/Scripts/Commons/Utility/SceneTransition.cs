using UnityEngine.SceneManagement;

namespace Commons.Utility
{
    /// <summary>
    /// シーン遷移用のUtilityクラス
    /// </summary>
    public static  class SceneTransition
    {
        public static void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}