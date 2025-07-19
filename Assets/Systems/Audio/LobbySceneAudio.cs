using UnityEngine;


public class LobbySceneAudio : MonoBehaviour
{
    void Start()
    {
        
        AudioManager.Instance.PlayMusic("HipoHipo", true);
    }
}