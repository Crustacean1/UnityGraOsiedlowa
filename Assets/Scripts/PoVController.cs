using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoVController : MonoBehaviour
{
    private float xTurn = 0;
    private float yTurn = 0;

    public float MovementSpeed;
    public float TurnSpeed;
    public Rigidbody Physical;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var delta = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            delta += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            delta += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            delta += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            delta += Vector3.right;
        }

        xTurn += TurnSpeed * Input.GetAxis("Mouse X");
        yTurn += TurnSpeed * Input.GetAxis("Mouse Y");

        transform.rotation = Quaternion.AngleAxis(xTurn, Vector3.up) * Quaternion.AngleAxis(yTurn, Vector3.left);

        var direction = transform.TransformVector(delta);
        direction.y = 0;
        direction.Normalize();

        Physical.AddForce(direction * MovementSpeed, ForceMode.Impulse);
        var unconstrained = Physical.gameObject.transform.localPosition;
        Physical.gameObject.transform.localPosition = new Vector3(
            Mathf.Clamp(unconstrained.x, -10, 10),
            unconstrained.y,
            Mathf.Clamp(unconstrained.z, -10, 10)
            );
    }
}
