using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;
    [SerializeField] private InputGame _playerInput;

    [SerializeField] private int inventorySize = 54;
    private void Awake()
    {
        _playerInput = new InputGame();

    }

    private void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }
    public void inventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            /*Debug.Log("J'appuie");*/
            if (!inventoryUI.isActiveAndEnabled) 
            {
                inventoryUI.Show();
            }
            else
            {
                inventoryUI.Hide();
            }

        }
        else if (context.canceled)
        {
            /*Debug.Log("Je relache");*/
        }
    }

    private void OnEnable()
    {
        _playerInput.Player.Inventory.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Inventory.Disable();

    }

}
