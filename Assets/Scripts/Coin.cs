using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().ToString() == "UnityEngine.CircleCollider2D")
        {
            if (collision.gameObject.CompareTag("Coin1"))
            {
                Destroy(collision.gameObject);
                Gold += 1f;
            }
            if (collision.gameObject.CompareTag("Coin5"))
            {
                Destroy(collision.gameObject);
                Gold += 5f;
            }
        }
    }
}
