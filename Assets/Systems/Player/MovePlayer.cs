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
       

        if (touchPointerUI != null)
        {
            touchPointerUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        MobileInput();
  
    }
    
    public void PcInput()
    {
        if (Input.GetMouseButton(0))
        {
            // Check if the mouse is over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (touchPointerUI != null)
                    touchPointerUI.gameObject.SetActive(false);
                return;
            }

            // Show and position the UI pointer
            if (touchPointerUI != null)
            {
                touchPointerUI.gameObject.SetActive(true);
                touchPointerUI.rectTransform.position = Input.mousePosition;
            }

            // Raycast from mouse position
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchLayer))
            {
                // Set the target position, keeping the player's original Y
                currentPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
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
        else
        {
            // When mouse button is released, hide the pointer
            if (touchPointerUI != null)
                touchPointerUI.gameObject.SetActive(false);
        }
    }

    public void MobileInput()
    {
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
                currentPosition =
                    new Vector3(hit.point.x, transform.position.y, hit.point.z); // Mantenemos la Y original
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
    }
}

