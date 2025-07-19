// MinigameTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameTrigger : MonoBehaviour
{
    [Tooltip("Escena a cargar.")]
    public string sceneNameToLoad;

    [Tooltip("Nombre pantalla de carga.")]
    public string loadingSceneName = "LoadScreen";

    [Tooltip("Activador de Trigger")]
    public string activatingTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(activatingTag))
        {
            AudioManager.Instance.PlaySFX("Aseguren");
            // 1. Guarda el nombre de la escena del minijuego en nuestro script estático.
            SceneData.SceneToLoad = sceneNameToLoad;

            // 2. Carga la escena de la pantalla de carga.
            Debug.Log("Iniciando carga para: " + sceneNameToLoad);
            SceneManager.LoadScene(loadingSceneName);
            AudioManager.Instance.PlaySFX("Cancion");
        }
    }
}