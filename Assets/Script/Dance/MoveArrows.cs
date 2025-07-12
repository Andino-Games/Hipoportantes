using UnityEngine;

public class MoveArrows : MonoBehaviour
{
    public float beatTempo;
    public bool hasStarted;
    
    void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, -beatTempo * Time.deltaTime, 0f);
    }
   
    
}
