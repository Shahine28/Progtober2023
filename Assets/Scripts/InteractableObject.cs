using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] float ZoneInterraction;
    [SerializeField] GameObject toucheE;
    [SerializeField] Vector2 positionToucheE;
    bool toucheEIsPressed;
    GameObject toucheEInstance;
    [SerializeField] bool isUsed;
    [SerializeField] bool hasBeenUsed;
    [SerializeField] UnityEvent Event;
    [SerializeField] PlayerInput _playerInput;
    InputAction _toucheE;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }
    // Start is called before the first frame update
    void Start()
    {
        toucheEInstance = Instantiate(toucheE);
    }

    // Update is called once per frame
    void Update()
    {

        toucheEInstance.transform.position = new Vector2(transform.position.x + positionToucheE.x, transform.position.y + positionToucheE.y);
        toucheEInstance.SetActive(false);
        Collider2D[] zoneInterraction = Physics2D.OverlapCircleAll(transform.position, ZoneInterraction);
        foreach (Collider2D collision in zoneInterraction)
        {
            if (collision.gameObject.tag == "Player")
            {
                toucheEInstance.SetActive(true);
                if(toucheEIsPressed)
                {
                    isUsed = true;
                    hasBeenUsed = true;
                    Event.Invoke();
                    
                }
            }
        }
        if(isUsed)
        {
            if (!toucheEIsPressed) isUsed = false;
        }
       if (toucheEIsPressed) toucheEIsPressed = false;
    }

    private void OnEnable()
    {
        _toucheE = _playerInput.Player.Interract;
        _toucheE.Enable();
        _toucheE.performed += Interract;
    }

    private void OnDisable()
    {
        _toucheE.Disable();
        _toucheE.canceled += Interract;
    }

    private void Interract(InputAction.CallbackContext context)
    {
        toucheEIsPressed = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ZoneInterraction);
    }
}
