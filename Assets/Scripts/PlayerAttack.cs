using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;
    // Update is called once per frame
    void Awake()
    {
        _playerInput = new PlayerInput();
    }
    void Update()
    {
  
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("J'appuie");
        }
        else if (context.canceled)
        {
            Debug.Log("Je relache");
        }
    }
    private void OnEnable()
    {
        _playerInput.Player.Fire.Disable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Fire.Disable();
        
    }
}
