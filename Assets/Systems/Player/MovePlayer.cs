using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovePlayer : MonoBehaviour
{
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
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            if (touchPointerUI != null)
                touchPointerUI.gameObject.SetActive(false);
            return;
        }

        if (touchPointerUI != null)
        {
            touchPointerUI.gameObject.SetActive(true);
            touchPointerUI.rectTransform.position = touch.position;
        }

        Ray ray = _cam.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchLayer))
        {
            currentPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z); // Mantenemos la Y original
        }
        
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            if (touchPointerUI != null)
                touchPointerUI.gameObject.SetActive(false);
        }
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


    // --- ¡AQUÍ EMPIEZA LA MAGIA DE LA ANIMACIÓN! ---

    // 1. Calculamos la dirección del movimiento
    Vector3 movementDirection = (transform.position - _lastPosition).normalized;
    _lastPosition = transform.position;

    // 2. Pasamos la información al Animator
    _animator.SetFloat("Speed", movementDirection.magnitude * 100); // Multiplicamos para que el valor no sea tan pequeño
    _animator.SetFloat("DirectionX", movementDirection.x);
    _animator.SetFloat("DirectionZ", movementDirection.z);

    // 3. Lógica para "flipear" el sprite
    if (movementDirection.x != 0)
    {
        _spriteRenderer.flipX = movementDirection.x < 0;
    }
    }
}

