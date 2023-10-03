using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpiderBehavior : MonoBehaviour
{
    [SerializeField] float ZoneDegat;
    [SerializeField] float ZoneSuivie;
    [SerializeField] bool isSetUp;
    [SerializeField] bool seePlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerDetection();
    }

    private void playerDetection()
    {
        Collider2D[] zoneInterraction = Physics2D.OverlapCircleAll(transform.position, ZoneDegat);
        foreach (Collider2D collision in zoneInterraction)
        {
            if (collision.gameObject.tag == "Player")
            {
                isSetUp = true;
                Vector2 direction = new Vector2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y);
                transform.up = direction;

            }
            else isSetUp = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ZoneDegat);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ZoneSuivie);
    }

}
