using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update*
    [SerializeField] float health = 1;
    [SerializeField] float maxHealth;
    public bool isDead;
    [SerializeField] Animator animator;
    [SerializeField] Color damageColor;
    [SerializeField] SpriteRenderer spriteRenderer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator TakeDamage(float damage)
    {
        health -= damage;
        if (!isDead)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(255, 255, 255);
        }

        if (health <= 0)
        {
            isDead = true;
            animator.SetTrigger("Dead");
            Destroy(gameObject, 10f);
        }
    }
    private void OnValidate()
    {
        if (health > maxHealth)
        {
               maxHealth = health;
        }

        if (health < 0)
        {
            health = Mathf.Abs(health);
        }

        if(maxHealth < 0)
        {
            maxHealth = Mathf.Abs(maxHealth); 
        }
    }
}
