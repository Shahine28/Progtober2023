using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] CapsuleCollider2D capsuleCollider;
    public float Gold = 0;
    [SerializeField] TextMeshProUGUI coinSTR;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coinSTR != null)
        {
            coinSTR.text = Gold.ToString();
        }
            
    }

/*    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Coin1")
        {
            Destroy(collision.gameObject);
            Gold += 1f;
        }
        if (collision.gameObject.tag == "Coin5")
        {
            Destroy(collision.gameObject);
            Gold += 5f;
        }
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin1")
        {  
            Destroy(collision.gameObject);
            Gold += 1f;
        }
        if (collision.gameObject.tag == "Coin5")
        {
            Destroy(collision.gameObject);
            Gold += 5f;
        }
    }
}
