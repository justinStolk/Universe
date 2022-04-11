using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{

    public float mass;
    public float radius = 0.5f;
    public float defaultForwardSpeed;
    public float axisRotationSpeed;
    public Orbiter[] satellites;

    [HideInInspector] public float gravitationalPull;
    [HideInInspector] public Vector3 gravityDirection;

    private Orbiter[] allOrbiters;
    private Rigidbody rigidbody;
    private float gravity;
    private float gravitationalConstant = 0.6f;

    private void OnValidate()
    {
        float diameter = 2 * radius;
        transform.localScale = new(diameter, diameter, diameter);
    }
    // Start is called before the first frame update
    void Start()
    {
        allOrbiters = FindObjectsOfType<Orbiter>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = mass;
        //rigidbody.velocity = new(0, 0, defaultForwardSpeed * Time.deltaTime);
        //gravity = 1;
    }

    // Update is called once per frame
    void Update()
    {

        //rigidbody.velocity = rigidbody.velocity = new(0, 0, defaultForwardSpeed * Time.deltaTime);
        //float targetAngle = (Mathf.Atan2(gravityDirection.x, gravityDirection.z) * Mathf.Rad2Deg) +90;
        //transform.rotation = Quaternion.Euler(0, targetAngle, 0);
    }
    void Attract(Orbiter orbiter)
    {
        Rigidbody rb = orbiter.GetComponent<Rigidbody>();
        Vector3 gravityDirection = transform.position - orbiter.transform.position;
        float distance = gravityDirection.sqrMagnitude;
        float gravitationalPull = (gravitationalConstant * mass * orbiter.mass) / (distance);
        Vector3 force = gravityDirection.normalized * gravitationalPull;

        float targetAngle = Mathf.Atan2(gravityDirection.x, gravityDirection.z) * Mathf.Rad2Deg;
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.left * force.sqrMagnitude;

        rb.velocity = (force + moveDirection ) * Time.fixedDeltaTime;
    }

    void AttractAll()
    {
        foreach(Orbiter o in allOrbiters)
        {
            if(o == this)
            {
                continue;
            }
            Attract(o);
        }
    }

    private void FixedUpdate()
    {
        AttractAll();
        //foreach (Orbiter o in satellites)
        //{
        //    Attract(o);
        //}
        rigidbody.rotation = Quaternion.Euler(0, rigidbody.rotation.eulerAngles.y + axisRotationSpeed, 0);
        //rigidbody.velocity = (new Vector3(0, 0, defaultForwardSpeed) + gravityDirection.normalized * gravitationalPull) * Time.fixedDeltaTime;
    }
}
