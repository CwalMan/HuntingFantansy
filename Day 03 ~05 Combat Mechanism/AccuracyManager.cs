using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyManager : MonoBehaviour
{
    [SerializeField] private bool debugMode = false;
    [SerializeField] GameObject lRendObject;
    public float MaxViewAngle = 120f;
    [Range(0f, 360f)]
    [SerializeField] private float horizontalViewAngle = 120f; // 시야각

    public float mouseAngle;
    private float horizontalViewHalfAngle = 0f;
    public float aimingSpd = 0.3f;
    public RangedWeapon rw;
    Vector2 barrelDir;

    GameObject lRend1;
    GameObject lRend2;
    float timer;
    private void Start()
    {
        lRend1 = Instantiate(lRendObject);
        lRend1.SetActive(false);
        lRend2 = Instantiate(lRendObject);
        lRend2.SetActive(false);

        rw = GetComponentInChildren<RangedWeapon>();
    }

    private void Update()
    {
        Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = (Input.mousePosition - sp).normalized;
        mouseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        barrelDir = dir;
        if (Input.GetMouseButton(1))
        {
            DrawLine();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            lRend1.SetActive(false);
            lRend2.SetActive(false);
            horizontalViewAngle = MaxViewAngle;
        }
    }


    private Vector3 AngleToDirZ(float angleInDegree) // 시야중심선
    {
        float radian = angleInDegree * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f);
    }


    void DrawLine()
    {
        horizontalViewAngle = Mathf.SmoothDamp(horizontalViewAngle, 0, ref timer, aimingSpd);
        horizontalViewHalfAngle = horizontalViewAngle * 0.5f;
        Vector3 horizontalRightDir = AngleToDirZ(-horizontalViewHalfAngle + mouseAngle);
        Vector3 horizontalLeftDir = AngleToDirZ(horizontalViewHalfAngle + mouseAngle);
        Vector3 lookDir = AngleToDirZ(mouseAngle);

        LineRenderer lRendA = lRend1.GetComponent<LineRenderer>();
        LineRenderer lRendB = lRend2.GetComponent<LineRenderer>();

        lRendA.SetPosition(0, transform.position);
        lRendA.SetPosition(1, horizontalRightDir * rw.maxRange);
        lRendA.gameObject.SetActive(true);

        lRendB.SetPosition(0, transform.position);
        lRendB.SetPosition(1, horizontalLeftDir * rw.maxRange);
        lRendB.gameObject.SetActive(true);
    }
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            horizontalViewHalfAngle = horizontalViewAngle * 0.5f;

            Vector3 originPos = transform.position;

            Gizmos.DrawWireSphere(originPos, rw.maxRange);

            Vector3 horizontalRightDir = AngleToDirZ(-horizontalViewHalfAngle + mouseAngle);
            Vector3 horizontalLeftDir = AngleToDirZ(horizontalViewHalfAngle + mouseAngle);

            Vector3 lookDir = AngleToDirZ(mouseAngle);

            Debug.DrawRay(originPos, horizontalLeftDir * rw.maxRange, Color.black);
            Debug.DrawRay(originPos, horizontalRightDir * rw.maxRange, Color.black);
            Debug.DrawRay(originPos, lookDir * rw.maxRange, Color.blue);

        }
    }
}
