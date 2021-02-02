using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseMob : MonoBehaviour
{
    [Header("Insert Body")]
    public Transform Body;

    public CapsuleCollider2D headCol;
    public BoxCollider2D bodyCol;
    protected Rigidbody2D rb;
    public int WanderingRange;
    public float spd;
    public float IdleTime;
    public float WalkTime;
    protected bool stopWander;
    public bool goWander;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnValidate()
    {
        headCol = Body.GetComponentInChildren<CapsuleCollider2D>();
        bodyCol = Body.GetComponentInChildren<BoxCollider2D>();
        headCol.gameObject.name = "Head Collider";
        bodyCol.gameObject.name = "Body Collider";
    }
    public virtual Vector2 Wandering()
    {
        int RandomNumberx = Random.Range(-WanderingRange, WanderingRange);
        int RandomNumbery = Random.Range(-WanderingRange, WanderingRange);
        Vector2 wanderDir = new Vector2(RandomNumberx, RandomNumbery).normalized;
        return wanderDir;
    }

    public IEnumerator doWander()
    {
        float idleSec = Random.Range(1, IdleTime);
        float walkSec = Random.Range(1, WalkTime);
        rb.velocity = Wandering() * spd;
        goWander = false;

        yield return new WaitForSeconds(walkSec);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(idleSec);
        goWander = true;
    }
    public virtual void Update()
    {
        if (goWander)
        {
            StartCoroutine(doWander());
        }
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, WanderingRange);
    }
}
