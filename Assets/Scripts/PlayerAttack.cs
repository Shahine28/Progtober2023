using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] Animator _animator;
    [SerializeField] Transform _attackPoint;
    [SerializeField] float _attackRange;
    [SerializeField] LayerMask _enemyLayers;
    // Update is called once per frame
    void Awake()
    {
        _playerInput = new PlayerInput();
    }
    void Update()
    {
  
    }
    void Attack() 
    {
        _animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit "+ enemy.gameObject.name);
        }
    }

    public void LeftCLick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            /*Debug.Log("J'appuie");*/
            Attack();
            
        }
        else if (context.canceled)
        {
            /*Debug.Log("Je relache");*/
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

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }


}

