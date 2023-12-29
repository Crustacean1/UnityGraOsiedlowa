using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Controls the camera's movement and orientation in the scene.
/// </summary>
public class CameraController : MonoBehaviour
{

    private Camera LocalCamera;
    private Vector3 prevMouse;
    private Vector2 cameraOrientation = new Vector2(0, -50);

    private float radius = 10;

    /// <summary>
    /// Speed of the camera movement.
    /// </summary>
    public float CameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        LocalCamera = gameObject.GetComponent<Camera>();
    }

    /// <summary>
    /// Computes the orientation of the camera based on its position and orientation angles.
    /// </summary>
    void computeOrientation()
    {
        Vector3 position = new Vector3(0, -1, -radius);
        var orientation = Quaternion.AngleAxis(cameraOrientation.x, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(cameraOrientation.y, new Vector3(-1, 0, 0));
        gameObject.transform.localRotation = orientation;
        position = orientation * position;
        gameObject.transform.position = position;
    }

    /// <summary>
    /// Called once per frame to update camera movement and orientation.
    /// </summary>
    void Update()
    {
        var mousePos = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            cameraOrientation += (Vector2)(mousePos - prevMouse) * CameraSpeed;
            cameraOrientation.y = Mathf.Max(-90,Mathf.Min(cameraOrientation.y, -15));
        }
        radius = Mathf.Max(Mathf.Min(radius - Input.mouseScrollDelta.y * 0.1f, 15), 1);
        computeOrientation();

        prevMouse = Input.mousePosition;
    }
}
