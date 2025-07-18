using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    private const string HappyKey = "Happinest";

    private int currentScore = 0;
    public TMP_Text scoreText;
    public GameObject menuShow;
    public SpriteRenderer pauseButton;

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
        }
        LoadScore();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        scoreText.text = currentScore.ToString();
        SaveScore();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt(HappyKey, currentScore);
        PlayerPrefs.Save();
        Debug.Log("Guardando score" + currentScore);
    }

    private void LoadScore()
    {
        currentScore = PlayerPrefs.GetInt(HappyKey, 0);
        scoreText.text = currentScore.ToString();
        Debug.Log("Cargando Score" + currentScore);
    }

    public void OpenMenu()
    {
        menuShow.gameObject.SetActive(true);
        // pauseButton = menu;
        IsPress = true;
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        menuShow.gameObject.SetActive(false);
        // pauseButton = menu;
        IsPress = false;
        Time.timeScale = 1f;
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
