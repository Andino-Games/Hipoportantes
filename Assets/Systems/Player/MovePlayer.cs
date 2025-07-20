using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovePlayer : MonoBehaviour
{
    public enum InputType
    {
        Mobile,
        PC
    }
    private Camera _cam;
    private Vector3 currentPosition;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _lastPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask touchLayer;
    [SerializeField] private Image touchPointerUI;
    [SerializeField] private Sprite defaultPointerSprite; 
    [SerializeField] private Sprite movingPointerSprite;
    [SerializeField] private string TypeOfTouch;

    public InputType stateControl = InputType.Mobile;
    void Start()
    {
        _cam = Camera.main;
        currentPosition = transform.position;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lastPosition = transform.position;

        if (touchPointerUI != null)
        {
            touchPointerUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
    // Lógica para el Puntero Táctil (la dejamos como la tienes)
        switch (stateControl)
        {
            case InputType.Mobile:
                Celular();
                break;
            case InputType.PC:
                InputPc();
                break;
             
                
        }
        // Mover el personaje
        if (Vector3.Distance(transform.position, currentPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, moveSpeed * Time.deltaTime);
            if (touchPointerUI != null && movingPointerSprite != null)
                touchPointerUI.sprite = movingPointerSprite;
        }
        else
        {
            if (touchPointerUI != null && defaultPointerSprite != null)
                touchPointerUI.sprite = defaultPointerSprite;
        }
   }

    public void Celular()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandlePointerInput(touch.position, touch.fingerId, touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled);
      
        }
    
       
    }

    private void InputPc()
    {
        if (Input.GetMouseButton(0)) 
        {
            HandlePointerInput(Input.mousePosition, -1, false); 
        }
        
        else if (Input.GetMouseButtonUp(0)) 
        {
            HandlePointerInput(Input.mousePosition, -1, true);
        }
    }
    private void HandlePointerInput(Vector3 screenPosition, int pointerId, bool endedOrCanceled)
    {
        if (EventSystem.current.IsPointerOverGameObject(pointerId))
        {
            if (touchPointerUI != null)
                touchPointerUI.gameObject.SetActive(false);
            return;
        }

        // Mostrar y posicionar el puntero UI
        if (touchPointerUI != null)
        {
            touchPointerUI.gameObject.SetActive(true);
            touchPointerUI.rectTransform.position = screenPosition;
        }

        // Calcular la posición de destino
        Ray ray = _cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchLayer))
        {
            currentPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z); // Mantenemos la Y original
        }
        
        if (endedOrCanceled)
        {
            if (touchPointerUI != null)
                touchPointerUI.gameObject.SetActive(false);
        }
    }
}

