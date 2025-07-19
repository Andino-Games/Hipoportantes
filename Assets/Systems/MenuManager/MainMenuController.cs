using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class MainMenuController : MonoBehaviour
{
    // Esta es la función que llamaremos desde el botón "PlayButton"
    public void IniciarJuego()
    {
        // Carga la escena del juego principal. 
        // El nombre debe ser exacto al de tu archivo de escena.
        SceneManager.LoadScene("Lobby");
    }
}