using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float Damage;
    [SerializeField] float Speed;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * Speed;
        /*Vector3 direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        float rot = Mathf.Atan2(-direction.y, -direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot+90); */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /*collision.gameObject.GetComponent<Health>().takeDamage(Damage);*/
        }
        Destroy(gameObject);
    }
}
