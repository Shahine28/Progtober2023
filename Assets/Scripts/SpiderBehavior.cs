using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpiderBehavior : MonoBehaviour
{
    [Header("Player Detection")]
    public float ZoneDegat;
    public float ZoneSuivie;
    [SerializeField] private bool isSetUp;
    public bool seePlayer;

    [Header("Spider Weapon")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireForce;
    [SerializeField] float cadence;
    float cadenceTmp;
    // Start is called before the first frame update
    void Start()
    {
        cadenceTmp = cadence;
    }

    // Update is called once per frame
    void Update()
    {
        if (seePlayer)
        {
            if (gameObject.GetComponent<Pathfinding.AIDestinationSetter>() != null)
            {
                gameObject.GetComponent<Pathfinding.AIDestinationSetter>().enabled = true;
            }
        }
        else if (!seePlayer) 
        {
            if (gameObject.GetComponent<Pathfinding.AIDestinationSetter>() != null)
            {
                gameObject.GetComponent<Pathfinding.AIDestinationSetter>().enabled = false;
            } 
        }
            
        playerDetection();
        
    }

    private void playerDetection()
    {
        Collider2D[] zoneDegat = Physics2D.OverlapCircleAll(transform.position, ZoneDegat);
        if (zoneDegat.Length > 0)
        {
            foreach (Collider2D collision in zoneDegat)
            {
                if (collision.gameObject.tag == "Player")
                {
                    isSetUp = true;
                    Vector2 direction = new Vector2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y);
                    transform.up = direction;
                    cadence -= Time.deltaTime;

                    if (cadence <= 0)
                    {
                        Shoot();
                        cadence = cadenceTmp;
                    }

                }
                else isSetUp = false;
            }
        }
        else
        {
            cadence = cadenceTmp;
            isSetUp = false;
        }

        Collider2D[] zoneSuivie = Physics2D.OverlapCircleAll(transform.position, ZoneSuivie);
        if (zoneSuivie.Length > 0)
        {
            foreach (Collider2D collision in zoneSuivie)
            {
                if (collision.gameObject.tag == "Player")
                {
                    seePlayer = true;
                }
                else seePlayer = false;
            }
        }
        else seePlayer = false;



    }

    void Shoot()
    {
        GameObject Bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ZoneDegat);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ZoneSuivie);
    } 
}


