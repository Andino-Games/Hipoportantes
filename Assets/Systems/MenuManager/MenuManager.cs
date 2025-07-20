using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    private const string HappyKey = "Happinest";

    private float currentScore = 0;

    [Header("UI References")] 
    public Image happyBar,totalEmotions;
    public GameObject menuShow;
    

    [HideInInspector]
    public bool IsPress;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentScore = PlayerPrefs.GetFloat(HappyKey, 0);
            if (happyBar != null)
            {
                UpdateScoreUI();
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject menuShowObject = GameObject.FindWithTag("MenuPause");
        if (menuShowObject != null)
        {
            menuShow = menuShowObject;
            menuShow.SetActive(false);
            IsPress = false;
            Time.timeScale = 1f;
        }
        else
        {
            menuShow = null;
        }
    }

    public void AddScore(float amount)
    {
        currentScore += amount;
        UpdateScoreUI();
        SaveScore();
    }

    private void UpdateScoreUI()
    {
        happyBar.fillAmount = currentScore / 20f;
        float emotions = currentScore / 200f;
        totalEmotions.fillAmount = emotions;
    }

    private void SaveScore()
    {
        PlayerPrefs.SetFloat(HappyKey, currentScore);
        PlayerPrefs.Save();
        Debug.Log("Guardando score" + currentScore);
    }

    public void OpenMenu()
    {
        if (menuShow != null)
        {
            menuShow.SetActive(true);
            IsPress = true;
            Time.timeScale = 0f;
        }
    }

    public void CloseMenu()
    {
        if (menuShow != null)
        {
            menuShow.SetActive(false);
            IsPress = false;
            Time.timeScale = 1f;
        }
    }

    public void TooglePuse()
    {
        if (IsPress)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    
    public void RequestSceneChange(string sceneName)
    {
        
        if (SceneManager.GetActiveScene().name == "MiniPlay1")
        {
            
            if (DanceController.Instance != null && DanceController.Instance.IsGameActive)
            {
                Debug.Log("No se puede cambiar de escena ahora, el minijuego está en curso.");
                
                return; 
            }
        }

        
        CloseMenu();

        
        var sceneChanger = FindObjectOfType<Script.ScenChanger.ChangeScene>();
        if (sceneChanger != null)
        {
            sceneChanger.Change(sceneName);
        }
        else
        {
            
            Debug.LogWarning("No se encontró el objeto ChangeScene, usando SceneManager.LoadScene directamente.");
            SceneManager.LoadScene(sceneName);
        }
    }
}