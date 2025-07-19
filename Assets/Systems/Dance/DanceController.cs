using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class DanceController : MonoBehaviour
{
    public static DanceController Instance;
    
    [SerializeField] private List<GameObject> arrows = new List<GameObject>();
    [SerializeField] private List<GameObject> spriteGoodNotes = new List<GameObject>();
    [SerializeField] private List<PlayableDirector> wrongNotes = new List<PlayableDirector>();
    [SerializeField] private List<Transform> positionNotes = new List<Transform>();
    
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private float leftMin,leftMax,rightMin,rightMax;

    public AudioSource sound;
    public bool isPlaying;
    public TMP_Text scoreText, finalScore;
    private int _score = 0, _noteCount = 0, _noteWrongCount = 0;
    
    [Header("Game Speed Settings")]
    public float initialBeat = 30f; 
    public float speedIncreaseAmount = 0.1f,speedDecraseAmount = 0.1f; 
    public float timeBetweenSpeedIncreases = 10f;
    
    [Header("Spawn Settings")]
    public float initialSpawnInterval = 1.5f;
    public float minSpawnInterval = 0.5f; 
    private float spawnTimer;
    public float currentTempo { get; private set; } 

    private float timer;

    [SerializeField] private PlayableDirector finalScreen;
    [SerializeField] private GameObject[] finalState;

    private bool activateTime;
    private SignalReceiver _signalReceiver;
    private int indexwrongNotes, wrongNote;

    private void Awake()
    {
        Instance = this;
        _signalReceiver = GetComponent<SignalReceiver>();
        currentTempo = initialBeat / 60f;
        timer = 0f;
        spawnTimer = 0f;
        scoreText.text = _score.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= timeBetweenSpeedIncreases)
        {
            IncreaseGameSpeed();
            timer = 0f;
            
        }
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

        if (!isPlaying)
        {
            sound.Play();
            isPlaying = true;
        }

        if (!sound.isPlaying && isPlaying)
        {
            MenuManager.Instance.AddScore(_score);
            GetComponent<Collider2D>().enabled = false;
            finalScreen.Play();
            finalScore.text = "Your Score: " + _score.ToString();
            state();
            Destroy(this);
        }
       
    }
    void IncreaseGameSpeed()
    {
        currentTempo += speedIncreaseAmount; 
        
        if (initialSpawnInterval > minSpawnInterval)
        {
            initialSpawnInterval -= speedDecraseAmount; 
        }

      
    }

    void SpawnRightArrow()
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
        scoreText.text = _score.ToString();

        int notes = Random.Range(5, 10);
        int goodNotes = Random.Range(0, spriteGoodNotes.Count);
        int pointNotes = Random.Range(0,positionNotes.Count);
        if (_noteCount >= notes)
        {
           GameObject iconsObject = Instantiate(spriteGoodNotes[goodNotes], positionNotes[pointNotes].position, quaternion.identity);
           Destroy(iconsObject,1.5f);
           _noteCount = 0;
        }
        
    }

    public void MissedNote()
    {
        _noteWrongCount++;
         indexwrongNotes = Random.Range(5, 10);
         wrongNote = Random.Range(0, wrongNotes.Count);

        if (_noteWrongCount >= indexwrongNotes)
        {
            wrongNotes[wrongNote].Play();
            _noteWrongCount = 0;
            Time.timeScale = 0f;
            
            
        }
        Debug.Log("Error en la note");
        
       
    }

    public void StopTimeline()
    {
        wrongNotes[wrongNote].Stop();
        Time.timeScale = 1f;

    }
    public void state()
    {
        if (_score >= 50) 
        {
            finalState[0].SetActive(true);
        }
        else
        {
            finalState[1].SetActive(true);
        }
    }
}
