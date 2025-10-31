using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneLauncher : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void LaunchScene()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}