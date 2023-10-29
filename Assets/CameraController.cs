using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Camera LocalCamera;
    private Vector3 prevMouse;
    private Vector2 cameraOrientation = new Vector2(0,-50);

    private float radius = 5;

    public float CameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        LocalCamera = gameObject.GetComponent<Camera>();
    }

    void computeOrientation(){
        Vector3 position = new Vector3(0, 0, - radius);
        var orientation = Quaternion.AngleAxis(cameraOrientation.x, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(cameraOrientation.y, new Vector3(-1, 0, 0));
        gameObject.transform.localRotation = orientation;
        position = orientation * position;
        gameObject.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        
        if(Input.GetMouseButton(1))
        {
	    cameraOrientation += (Vector2) (mousePos - prevMouse) * CameraSpeed;
        }
        radius = Mathf.Max(Mathf.Min(radius - Input.mouseScrollDelta.y * 0.1f, 10),1);
	computeOrientation();

        prevMouse = Input.mousePosition;
    }
}
