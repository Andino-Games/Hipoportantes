using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    
    public void IniciarJuego()
    {
        
        SceneManager.LoadScene("Lobby");
    }
}