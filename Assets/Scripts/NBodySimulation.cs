using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    public static int SimulationSpeed = 1;
    public static List<CelestialBody> Bodies { get { return Instance.bodies; } }

    List<CelestialBody> bodies = new();

    static NBodySimulation instance;
    static NBodySimulation Instance { get { if(instance == null) { instance = FindObjectOfType<NBodySimulation>(); } return instance; } }

    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = Universe.physicsTimeStep;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < bodies.Count; i++)
        {
            Vector3 accel = CalculateAcceleration(bodies[i].Position, bodies[i]);
            bodies[i].UpdateVelocity(accel, Universe.physicsTimeStep * SimulationSpeed);
        }
        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].UpdatePosition(Universe.physicsTimeStep * SimulationSpeed);
        }
    }
    public static Vector3 CalculateAcceleration(Vector3 point, CelestialBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance.bodies)
        {
            if (body != ignoreBody)
            {
                float sqrDst = (body.Position - point).sqrMagnitude;
                Vector3 forceDir = (body.Position - point).normalized;
                acceleration += forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
            }
        }

        return acceleration;
    }
    public void SetSimulationSpeed(int newSpeed)
    {
        SimulationSpeed = newSpeed;
    }

}
public static class Universe
{
    public static float gravitationalConstant = 0.0006f;
    public static float physicsTimeStep = 0.02f;
}