using UnityEngine;


public class MenuSceneAudio : MonoBehaviour
{
    void Start()
    {
        
        AudioManager.Instance.StopMusic();
                
        
        AudioManager.Instance.PlaySFX("Cinema");
    }
}