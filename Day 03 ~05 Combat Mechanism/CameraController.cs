using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    public float moveRange = 3f;
    [Range(0,1)]
    public float spd = 0.3f;
    public Transform player;
    public bool DrawGizmos;
    public float maxZoom = 10f;
    public float minZoom = 5f;
    public float zoomLimiter = 20f;

    Camera mainCam;
    int playerLayer;
    Vector3 velocity;
    float time;



    private void Start()
    {
        mainCam = GetComponent<Camera>();
        playerLayer = 1 << LayerMask.NameToLayer("Player");
    }

    private void LateUpdate()
    {
        CameraMovement();
        Zoom();
        if (Input.GetMouseButton(1))
        {
            Vector3 centerPoint = GetCenterPoint();
            Vector3 newPos = centerPoint + offset;
            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, spd);
        }
        
    }

    void Zoom()
    {
        if(Input.GetMouseButton(1))
        {
            float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistance() / zoomLimiter);
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, newZoom, Time.deltaTime);
        }
        else
        {
            mainCam.orthographicSize = Mathf.SmoothDamp(mainCam.orthographicSize, minZoom, ref time, spd);
        }
    }


    float GetGreatestDistance()
    {
        var bounds = new Bounds(player.position, Vector3.zero);
        bounds.Encapsulate(mouseVec());

        return Mathf.Max(bounds.size.x,bounds.size.y);
    }

    Vector3 GetCenterPoint()
    {
        var bounds = new Bounds(player.position, Vector3.zero);
        bounds.Encapsulate(mouseVec());

        return bounds.center;
    }

    Vector3 mouseVec()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 mouseVec = new Vector3(screenPoint.x, screenPoint.y, transform.position.z);

        return mouseVec;
    }
    void CameraMovement()
    {
        Vector3 playerVec = new Vector3(player.position.x, player.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position,playerVec, ref velocity, spd);
    }

    private void OnDrawGizmos()
    {
        if(DrawGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, moveRange);
        }
    }
}
