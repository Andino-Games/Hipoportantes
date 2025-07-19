using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("La barra de carga que se actualizará.")]
    public Slider loadingBar;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        
        yield return new WaitForSeconds(0.5f);

        
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneData.SceneToLoad);
        
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (loadingBar != null)
            {
                loadingBar.value = progress;
            }
            
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}