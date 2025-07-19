using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Collider))]
public class MinigameTrigger : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("El nombre de la escena del minijuego a cargar (ej. 'MiniPlay1').")]
    public string sceneNameToLoad;

    [Tooltip("El nombre de tu escena de pantalla de carga.")]
    public string loadingSceneName = "LoadScreen"; 

    [Header("Configuración del Trigger")]
    [Tooltip("La etiqueta del objeto que puede activar este portal (ej. 'Player').")]
    public string activatingTag = "Player";

    private bool isTransitioning = false;

    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag(activatingTag) && !isTransitioning)
        {
            
            isTransitioning = true;

            

            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
            }

            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Aseguren");
            }

            
            SceneData.SceneToLoad = sceneNameToLoad;

           
            Debug.Log($"Iniciando transición a la escena '{loadingSceneName}' para cargar '{sceneNameToLoad}'.");
            SceneManager.LoadScene(loadingSceneName);
        }
    }

    
    private void OnValidate()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"El Collider en el objeto '{gameObject.name}' debe tener 'Is Trigger' activado para que MinigameTrigger funcione.", this);
        }
    }
}