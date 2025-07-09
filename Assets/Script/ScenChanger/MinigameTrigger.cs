// MinigameTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameTrigger : MonoBehaviour
{
    [Tooltip("El nombre de la escena del minijuego que se cargará.")]
    public string sceneNameToLoad;

    [Tooltip("La etiqueta del objeto que puede activar este trigger (normalmente 'Player').")]
    public string activatingTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que entró en el trigger tiene la etiqueta correcta.
        if (other.CompareTag(activatingTag))
        {
            Debug.Log("Jugador ha entrado en el trigger. Cargando escena: " + sceneNameToLoad);
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}