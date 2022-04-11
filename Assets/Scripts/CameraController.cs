using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float flySpeed = 20;
    public float maxVerticalAngle = 45;
    public float turnSpeed = 20;

    private float orthoSize = 250;
    private Camera cam;
    private float rotationalValue;
    private float setRotation;
    private float thrust;
    private float rotVertical;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = orthoSize;
    }

    private void Update()
    {
        thrust = 0;
        rotationalValue = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        rotVertical -= Input.GetAxis("Vertical") * turnSpeed * Time.deltaTime;
        if (rotVertical < -maxVerticalAngle)
        {
            rotVertical = -maxVerticalAngle;
            
        }
        else if(rotVertical > maxVerticalAngle)
        {
            rotVertical = maxVerticalAngle;
        }
        //float targetRotation = transform.eulerAngles.x + rotVertical;
        setRotation = rotVertical;
        if (Input.GetKey(KeyCode.Q))
        {
            thrust = 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            thrust = -1;
        }

    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.Rotate(new(0, rotationalValue, 0));
        transform.rotation = Quaternion.Euler(setRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.position += transform.forward * thrust * flySpeed * Time.deltaTime;

    }

    public void SetOrthographicView(bool ortho)
    {
        if (ortho)
        {
            cam.orthographic = true;
            return;
        }
        cam.orthographic = false;
    }

}
