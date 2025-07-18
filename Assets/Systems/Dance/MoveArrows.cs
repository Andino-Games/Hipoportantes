using System;
using UnityEngine;

public class MoveArrows : MonoBehaviour
{
    public float beatTempo = 30f;
    private float faster,timeToFaster,speedIncrement, currentBeat;
    public bool NoMoreArrows;
    
    
    void Start()
    {
        currentBeat = DanceController.Instance.currentTempo;
        currentBeat = 30f / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (DanceController.Instance != null)
        {
            currentBeat = DanceController.Instance.currentTempo;
        }
        transform.position += new Vector3(0f, -currentBeat * Time.deltaTime, 0f);
        
        
    }

 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("End"))
        {
            NoMoreArrows = true;
            DanceController.Instance.MissedNote();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NoMoreArrows = false;
    }
}
