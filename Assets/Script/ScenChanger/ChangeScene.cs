using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.ScenChanger
{
    public class ChangeScene : MonoBehaviour
    {
        public void Change(string nameScene)
        {
            SceneManager.LoadScene(nameScene);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}