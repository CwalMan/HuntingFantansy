using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveInput;
    public float moveSpd = 4f;
    public Transform playerArm;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        playerMove();
        playerAim();
    }
    void playerMove()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        rb.velocity = moveInput * moveSpd;
    }

    void playerAim()
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
        Vector2 offset = new Vector2(aimDir.x, aimDir.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        playerArm.eulerAngles = new Vector3(0, 0, angle);
    }
}
