using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : GravityObject
{
    public float surfaceGravity;
    public float radius;
    public Vector3 initialVelocity;
    public string bodyName = "Unnamed";
    public float mass { get; private set; }
    public Vector3 velocity { get; private set; }
    public Rigidbody Rigidbody { get { return rigidbody; }  }
    public Vector3 Position { get { return rigidbody.position; } }

    Transform meshHolder;
    Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = mass;
        velocity = initialVelocity;
    }


    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach(CelestialBody otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.rigidbody.position - rigidbody.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rigidbody.position - rigidbody.position).normalized;
                Vector3 accel = forceDir * Universe.gravitationalConstant * otherBody.mass / sqrDst;
                velocity += accel * timeStep;
            }
        }
    }
    public void UpdateVelocity(Vector3 accel, float timeStep)
    {
        velocity += accel * timeStep;
    }
    public void UpdatePosition(float timeStep)
    {
        rigidbody.MovePosition(rigidbody.position + velocity * timeStep);
    }
    public void UpdateComponentValues()
    {
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
        meshHolder = transform.GetChild(0);
        meshHolder.localScale = Vector3.one * radius;
        gameObject.name = bodyName; 
        rigidbody.mass = mass;
        velocity = initialVelocity;
    }
    void OnValidate()
    {
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
        meshHolder = transform.GetChild(0);
        meshHolder.localScale = Vector3.one * radius;
        gameObject.name = bodyName;
    }


}
