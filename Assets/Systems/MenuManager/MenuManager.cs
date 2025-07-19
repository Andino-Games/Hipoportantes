using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    private const string HappyKey = "Happinest";

    private int currentScore = 0;

    [Header("UI References")]
    public TMP_Text scoreText; 
    public GameObject menuShow;
    public SpriteRenderer pauseButton;

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
            
            // Cargamos el puntaje del disco a la memoria, sin tocar la UI.
            currentScore = PlayerPrefs.GetInt(HappyKey, 0);
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

    // Este método se ejecuta cada vez que una escena nueva se carga.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Buscamos el objeto para el texto del score.
        GameObject scoreTextObject = GameObject.FindWithTag("ScoreText");
        if (scoreTextObject != null)
        {
            scoreText = scoreTextObject.GetComponent<TMP_Text>();
            UpdateScoreUI();
        }
        else
        {
            scoreText = null;
        }

        
        GameObject menuShowObject = GameObject.FindWithTag("PauseMenu");
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

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
        SaveScore();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt(HappyKey, currentScore);
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
}