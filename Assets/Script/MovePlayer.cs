using System;
using UnityEngine;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    private Camera _cam;
    private Vector3 currentPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask touchLayer;
    [SerializeField] private Image touchPointerUI;
    [SerializeField] private Sprite defaultPointerSprite; 
    [SerializeField] private Sprite movingPointerSprite; 
    
    void Start()
    {
        _cam = Camera.main;
        currentPosition = transform.position;
        if (touchPointerUI != null)
        {
            touchPointerUI.gameObject.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); 
            if (touchPointerUI != null)
            {
                touchPointerUI.gameObject.SetActive(true); // Muestra el puntero
                // Las posiciones de toque están en píxeles de pantalla, como los RectTransform de la UI
                touchPointerUI.rectTransform.position = touch.position;
            }
            Ray ray = _cam.ScreenPointToRay(touch.position); 
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchLayer))
            {
                currentPosition = hit.point;
                if (Vector3.Distance(transform.position, currentPosition) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentPosition, moveSpeed * Time.deltaTime);
                    if (touchPointerUI != null && movingPointerSprite != null)
                    {
                        touchPointerUI.sprite = movingPointerSprite; // Cambia el sprite si hay movimiento
                    }
                    
                }
                else
                {
                    // Si hay hit pero no movimiento (o el objeto ya está en la posición), usa el sprite por defecto
                    if (touchPointerUI != null && defaultPointerSprite != null)
                    {
                        touchPointerUI.sprite = defaultPointerSprite;
                    }
                }
                    
            }
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (touchPointerUI != null)
                {
                    touchPointerUI.gameObject.SetActive(false); // Oculta el puntero cuando se levanta el dedo
                }
            }
               
            
            
        }
        else // Si no hay toques en la pantalla
        {
            if (touchPointerUI != null)
            {
                touchPointerUI.gameObject.SetActive(false); // Oculta el puntero si no hay toques activos
            }
        }
    }
    
}
