using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playerController : MonoBehaviour
{
    [Header("Player")]
    public float moveSpd = 4f;
    public Transform playerArm;

    [Header("Aim")]
    public RangedWeapon rw;
    [SerializeField] GameObject lineRendererPrefabs;
    [Range(0f, 360f)]
    [SerializeField] float horizontalViewAngle; // 시야각

    Rigidbody2D rb;
    GameObject lRend1;
    GameObject lRend2;
    Vector2 moveInput;
    float mouseAngle;
    float resetMoveSpd;
    float resetSpread;
    float resetViewAngle;
    float horizontalViewHalfAngle = 0f;
    private void Start()
    {
        DOTween.SetTweensCapacity(500, 50);

        resetMoveSpd = moveSpd;
        resetSpread = rw.Spread;
        horizontalViewAngle = rw.Spread * 2;
        resetViewAngle = horizontalViewAngle;

        rb = GetComponent<Rigidbody2D>();
        lRend1 = Instantiate(lineRendererPrefabs);
        lRend1.SetActive(false);
        lRend2 = Instantiate(lineRendererPrefabs);
        lRend2.SetActive(false);
    }

    private void Update()
    {
        playerMove();
        playerAim();
        mouseMovementBehaviour();
    }
    void playerAim()
    {
        if(Input.GetMouseButton(1))
        {
            DrawLine();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            DOTween.KillAll();
            horizontalViewAngle = resetViewAngle;
            rw.Spread = resetSpread;
            moveSpd = resetMoveSpd;
            lRend1.gameObject.SetActive(false);
            lRend2.gameObject.SetActive(false);
        }
    }
    void playerMove()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        rb.velocity = moveInput * moveSpd;
    }
    void mouseMovementBehaviour()
    {
        // change transform.localScale as mouse moves.
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);

        if (mousePos.x <= screenPoint.x) // 만약 마우스의 x축이 9시 방향이면
        {
            transform.localScale = Vector3.one;
            playerArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            playerArm.localScale = Vector3.one;
        }

        // gun Aim at mouse Position

        Vector3 aimDir = (mousePos - screenPoint).normalized;
        float angleValue = Mathf.Atan2(aimDir.y,aimDir.x) * Mathf.Rad2Deg;
        playerArm.eulerAngles = new Vector3(0, 0, angleValue);
        mouseAngle = angleValue;
    }
    void DrawLine()
    {
        Vector3 playerPos = transform.position;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseDistance = Vector2.Distance(worldMousePos, playerPos);

        if(rb.velocity.sqrMagnitude <= 0.1 )
        {
            DOTween.To(() => horizontalViewAngle, x => horizontalViewAngle = x, 0, rw.aimingSec);
            DOTween.To(() => rw.Spread, x => rw.Spread = x, 0, rw.aimingSec);
        }
        else
        {
            DOTween.To(() => horizontalViewAngle, x => horizontalViewAngle = x, resetViewAngle * rw.Erg, rw.aimingSec); // 10 * 0.1
            DOTween.To(() => rw.Spread, x => rw.Spread = x, resetSpread * rw.Erg, rw.aimingSec);
            moveSpd = resetMoveSpd * rw.Mob;
        }

        horizontalViewHalfAngle = horizontalViewAngle * 0.5f;
        Vector2 horRightDir = AngleToDirZ(-horizontalViewHalfAngle);
        Vector2 horLeftDir = AngleToDirZ(horizontalViewHalfAngle);

        LineRenderer lRendA = lRend1.GetComponent<LineRenderer>();
        LineRenderer lRendB = lRend2.GetComponent<LineRenderer>();

        lRendA.transform.position = transform.position;
        lRendA.transform.eulerAngles = new Vector3(0, 0, mouseAngle);
        lRendA.SetPosition(0, Vector2.zero); ;
        lRendA.SetPosition(1, horLeftDir * mouseDistance);
        lRendA.gameObject.SetActive(true);

        lRendB.transform.position = transform.position;
        lRendB.transform.eulerAngles = new Vector3(0, 0, mouseAngle);
        lRendB.SetPosition(0, Vector2.zero) ;
        lRendB.SetPosition(1, horRightDir *mouseDistance);
        lRendB.gameObject.SetActive(true);
    }
    private Vector2 AngleToDirZ(float Deg) // 시야중심선
    {
        float radian = Deg  * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
