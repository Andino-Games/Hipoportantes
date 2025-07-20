using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DanceController : MonoBehaviour
{
    public static DanceController Instance;

   
    public bool IsGameActive { get; private set; }
    // ---------------------------------

    [Header("Game Configuration")]
    [Tooltip("El ritmo inicial del juego. Afecta la velocidad de las flechas.")]
    public float initialBeat = 60f;
    [Tooltip("Segundos entre cada aumento de velocidad.")]
    public float timeBetweenSpeedIncreases = 10f;
    [Tooltip("Cuánto aumenta la velocidad cada vez.")]
    public float speedIncreaseAmount = 0.1f;

    [Header("Arrow Spawning")]
    [Tooltip("Prefabs de las flechas. 0=Izquierda, 1=Derecha.")]
    public GameObject[] arrows;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    [Tooltip("Rango de aparición vertical para las flechas.")]
    public float leftMin = -1f, leftMax = 1f;
    public float rightMin = -1f, rightMax = 1f;
    [Tooltip("Tiempo inicial entre la aparición de cada flecha.")]
    public float initialSpawnInterval = 2f;
    [Tooltip("El intervalo mínimo de aparición al que se puede llegar.")]
    public float minSpawnInterval = 0.5f;
    [Tooltip("Cuánto se reduce el intervalo de aparición en cada aumento de velocidad.")]
    public float speedDecraseAmount = 0.1f;

    [Header("UI & Feedback")]
    public TMP_Text scoreText;
    public Image HappyBar;
    public PlayableDirector finalScreen;
    [Tooltip("Objetos a mostrar al final. 0=Ganar, 1=Perder.")]
    public GameObject[] finalState;
    public List<GameObject> spriteGoodNotes;
    public List<Transform> positionNotes;
    public List<PlayableDirector> wrongNotes;

    [HideInInspector]
    public float currentTempo;

    private bool gameMusicStarted = false;
    private float _score = 0;
    private int _noteCount = 0;
    private int _noteWrongCount = 0;
    private float timer;
    private float spawnTimer;
    private int indexwrongNotes;
    private int wrongNote;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        
        IsGameActive = false;

        currentTempo = initialBeat / 60f;
        timer = 0f;
        spawnTimer = 0f;
        if (scoreText != null)
        {
            scoreText.text = _score.ToString();
        }
    }

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("cancion", false);
            gameMusicStarted = true;

            // --- ¡AQUÍ ACTIVAMOS LA BANDERA! ---
            // Justo cuando la música empieza, el juego se considera activo.
            IsGameActive = true;
        }
        else
        {
            Debug.LogError("¡AudioManager no encontrado! La música del juego no puede iniciar.");
        }
    }

    void Update()
    {
        if (!gameMusicStarted) return;

        HandleGameTimers();
        HandleArrowSpawning();
        CheckForGameEnd();
    }

    private void HandleGameTimers()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenSpeedIncreases)
        {
            IncreaseGameSpeed();
            timer = 0f;
        }
    }

    private void HandleArrowSpawning()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= initialSpawnInterval)
        {
            if (Random.Range(0, 2) == 0)
            {
                SpawnLeftArrow();
            }
            else
            {
                SpawnRightArrow();
            }
            spawnTimer = 0f;
        }
    }

    private void CheckForGameEnd()
    {
       
        if (AudioManager.Instance != null && !AudioManager.Instance.musicSource.isPlaying && gameMusicStarted)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        
        IsGameActive = false;

        this.enabled = false;

        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.AddScore(_score);
        }

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (finalScreen != null) finalScreen.Play();
        

        state();
    }

    void IncreaseGameSpeed()
    {
        currentTempo += speedIncreaseAmount;
        if (initialSpawnInterval > minSpawnInterval)
        {
            initialSpawnInterval -= speedDecraseAmount;
        }
    }

    public void SpawnRightArrow()
    {
        float randomYOffsetRight = Random.Range(rightMin, rightMax);
        Vector3 spawnPositionRight = rightSpawnPoint.position + new Vector3(0, randomYOffsetRight, 0);
        Instantiate(arrows[1], spawnPositionRight, Quaternion.identity);
    }

    public void SpawnLeftArrow()
    {
        float randomYOffsetLeft = Random.Range(leftMin, leftMax);
        Vector3 spawnPositionLeft = leftSpawnPoint.position + new Vector3(0, randomYOffsetLeft, 0);
        Instantiate(arrows[0], spawnPositionLeft, Quaternion.identity);
    }

    

    public void GoodNote()
    {
        _noteCount++;
        _score++;
        if (scoreText != null) scoreText.text = _score.ToString();
        
        float bar = _score / 22f ;
        HappyBar.fillAmount = bar ;
        
        int notes = Random.Range(3, 6);
        if (_noteCount >= notes && spriteGoodNotes.Count > 0 && positionNotes.Count > 0)
        {
            int goodNotesIndex = Random.Range(0, spriteGoodNotes.Count);
            int pointNotesIndex = Random.Range(0, positionNotes.Count);
            GameObject iconsObject = Instantiate(spriteGoodNotes[goodNotesIndex],
                positionNotes[pointNotesIndex].position,
                quaternion.identity);
            Destroy(iconsObject, 1.5f);
            _noteCount = 0;
        }
        Debug.Log("GoodNotes"+ _noteCount);
    }

    public void MissedNote()
    {
        _noteWrongCount++;
        indexwrongNotes = Random.Range(3, 6);

        if (_noteWrongCount >= indexwrongNotes && wrongNotes.Count > 0)
        {
            wrongNote = Random.Range(0, wrongNotes.Count);
            if (wrongNotes[wrongNote] != null)
            {
                wrongNotes[wrongNote].Play();
                _noteWrongCount = 0;
                Time.timeScale = 0f;
            }
        }
        Debug.Log("Error en la note");
    }

    public void StopTimeline()
    {
        if (wrongNotes.Count > 0 && wrongNote < wrongNotes.Count && wrongNotes[wrongNote] != null)
        {
            wrongNotes[wrongNote].Stop();
        }
        Time.timeScale = 1f;
    }

    public void state()
    {
        if (finalState == null || finalState.Length < 2) return;

        if (_score >= 10)
        {
            if (finalState[0] != null) finalState[0].SetActive(true);
        }
        else
        {
            if (finalState[1] != null) finalState[1].SetActive(true);
        }
    }
}