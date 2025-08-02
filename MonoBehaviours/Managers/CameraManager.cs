using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Vector2 minPosition; // 카메라 이동 최소좌표
    public Transform target;   
    public Vector3 offset;
    public float smoothSpeed = 5f;
    
    public float zoomSpeed = 3f;
    public float minZoom = 1f;
    public float maxZoom = 5f;

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player)
        {
            target = player.transform;
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return ;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}