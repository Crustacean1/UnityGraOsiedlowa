using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform MainCamera;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmout;
    public Vector3 zoomAmount;
    public float mouseZoomSpeed;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = MainCamera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraMovement();
        HandleMouseMovement();
    }

    void HandleMouseMovement()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount * mouseZoomSpeed;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }

    void HandleCameraMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if(Input.GetKey(KeyCode.S))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if(Input.GetKey(KeyCode.D))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if(Input.GetKey(KeyCode.A))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if(Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmout);
        }
        if(Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmout);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        MainCamera.localPosition = Vector3.Lerp(MainCamera.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
