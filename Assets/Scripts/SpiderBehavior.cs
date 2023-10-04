using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpiderBehavior : MonoBehaviour
{
    [Header("Player Detection")]
    public float ZoneDegat;
    public float ZoneSuivie;
    public bool seePlayer;
    bool hasSeenPlayer;
    bool _LookRight;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private bool isSetUp;

    [Header("Animation")]
    [SerializeField] Animator animator;
    

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
        GetComponent<Pathfinding.AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
       Debug.Log(rb.velocity.sqrMagnitude);
        playerDetection();
        animator.SetBool("isSetIUp",isSetUp);
        animator.SetBool("hasSeenPlayer", hasSeenPlayer);
        /*spiderMovement();*/

    }

    private void playerDetection()
    {

        Collider2D[] zoneSuivie = Physics2D.OverlapCircleAll(transform.position, ZoneSuivie);
        Collider2D[] zoneDegat = Physics2D.OverlapCircleAll(transform.position, ZoneDegat);


        if (isInCollider(zoneSuivie, GameObject.FindGameObjectWithTag("Player")))
        {
            hasSeenPlayer = true;
            seePlayer = true;
        }
        else if (!isInCollider(zoneSuivie, GameObject.FindGameObjectWithTag("Player")))
        {
            seePlayer = false;
        }
        if (isInCollider(zoneDegat, GameObject.FindGameObjectWithTag("Player")))
        {
            isSetUp = true;
            seePlayer = true;
            cadence -= Time.deltaTime;

            if (cadence <= 0)
            {
                StartCoroutine(Shoot());
                cadence = cadenceTmp;
            }
        }
        

        if (!isInCollider(zoneDegat, GameObject.FindGameObjectWithTag("Player")))
        {
            isSetUp = false;
            cadence = cadenceTmp;
        }

        if(hasSeenPlayer)
        {
            
            GetComponent<Pathfinding.AIDestinationSetter>().enabled = true;
            Tourner(gameObject);

        }

    }

/*    void spiderMovement()
    {
        if (seePlayer)
        {
            GetComponent<Pathfinding.AIDestinationSetter>().enabled = true;
        }
*//*        if (!seePlayer)
        {
           
        }*//*
    }*/

    public void Tourner(GameObject obj)
    {
        if ((GameObject.FindGameObjectWithTag("Player").transform.position.x < transform.position.x && _LookRight) || (GameObject.FindGameObjectWithTag("Player").transform.position.x > transform.position.x && !_LookRight))
        {
            _LookRight = !_LookRight;
            obj.transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            obj.transform.Rotate(0f, 0f, 0f);
        }
    }

    IEnumerator Shoot()
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.8f);
        GameObject Bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Force);
        animator.SetBool("Attack",false);
    }
    bool isInCollider(Collider2D[] collider, GameObject gameObject)
    {
        foreach (Collider2D collider2D in collider)
        {
            if (collider2D.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ZoneDegat);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ZoneSuivie);
    } 


}


