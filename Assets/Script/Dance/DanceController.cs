using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DanceController : MonoBehaviour
{
    public static DanceController Instance;
    [SerializeField] private List<GameObject> arrows = new List<GameObject>();
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private float spawnInterval = 1.5f,leftMin,leftMax,rightMin,rightMax;

    public AudioSource sound;
    public bool isPlaying;
    public TMP_Text scoreText;
    private int _score = 0;
    
    private float timer;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnArrowLeft(leftMin,leftMax);
            SpawnArrowRight(rightMin,rightMax);
        }

        if (!isPlaying)
        {
            sound.Play();
            isPlaying = true;
        }

        if (!sound.isPlaying && isPlaying)
        {
            Destroy(this);
        }
    }
    public void SpawnArrowLeft(float offsetMin, float offsetMax)
    {
        float randomYOffset = Random.Range(offsetMin, offsetMax);
        Vector3 spawnPosition = leftSpawnPoint.position + new Vector3(0, randomYOffset, 0);
        Instantiate(arrows[0], spawnPosition, Quaternion.identity);
    }

    public void SpawnArrowRight(float offsetMin, float offsetMax)
    {
        float randomYOffset = Random.Range(offsetMin, offsetMax);
        Vector3 spawnPosition = rightSpawnPoint.position + new Vector3(0, randomYOffset, 0);
        Instantiate(arrows[1], spawnPosition, Quaternion.identity);
    }

    public void GoodNote()
    {
        _score++;
        scoreText.text = "Score: " + _score.ToString();
    }

    public void MissedNote()
    {
        Debug.Log("Error en la note");
    }
}
