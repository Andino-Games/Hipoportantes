using UnityEngine;

namespace Audio
{
    public class AudioManagerLoader : MonoBehaviour
    {
        public GameObject audioManagerPrefab;

        private void Awake()
        {
            if (AudioManager.Instance == null)
            {
                GameObject audioManager = Instantiate(audioManagerPrefab);
                DontDestroyOnLoad(audioManager);
            }
        }
    }
}