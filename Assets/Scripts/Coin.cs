using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] CapsuleCollider2D capsuleCollider;
    [SerializeField] float Gold = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Coin1")
        {
            Gold += 1f;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Coin5")
        {
            Gold += 5f;
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin1")
        {
            Gold += 1f;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Coin5")
        {
            Gold += 5f;
            Destroy(collision.gameObject);
        }
    }
}
