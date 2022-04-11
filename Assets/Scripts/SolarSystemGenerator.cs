using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemGenerator : MonoBehaviour
{

    public int NumberOfPlanets = 7;
    [Range(0.5f, 20)]
    public float solarIntensity = 5;
    public float MinimumRadius, MaximumRadius;
    public float MinimumStarSize, MaximumStarSize;
    public float MinimumGravity, MaximumGravity;
    public float MinPlanetDistance, MaxPlanetDistance, MaxDistanceFromStar;

    string[] planetPrefixes = {"Por", "Ral", "Tun","Man" ,"Nor" ,"Mai" ,"Kon", "Vor" };
    string[] planetSuffixes = {"-Mer", "sal", "feu", "nor", "-X 7", "gual", "vor", "mir" };
    Vector3 lastPosition = Vector3.zero;
    CelestialBody star;
    List<CelestialBody> planets = new();


    // Start is called before the first frame update
    void Awake()
    {
        CreateStar(); 
        Vector3 position = new Vector3(0, 0, Random.Range(MinPlanetDistance, MaxPlanetDistance));
        for (int i = 0; i < NumberOfPlanets; i++)
        {
            CreatePlanet(position);
            position.z += Random.Range(MinPlanetDistance, MaxPlanetDistance);
        }
        foreach(CelestialBody body in planets)
        {
            SetPlanetVelocity(body);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetPlanetVelocity(CelestialBody planet)
    {
            float planetPull = 0;
            foreach (CelestialBody other in planets)
            {
                if (planet != other)
                {
                    planetPull += Mathf.Sqrt((Universe.gravitationalConstant * other.mass) / Vector3.Distance(planet.Position, other.Position));
                }
            }
            float dist = Vector3.Distance(planet.Position, star.Position);
            Vector3 dir = (planet.Position - star.Position).normalized;
            Vector3 dirShift = Vector3.one;
            dirShift.x *= dir.z;
            dirShift.y *= dir.y;
            dirShift.z *= dir.x;
            Debug.Log(dirShift);
            float vel = Mathf.Sqrt((Universe.gravitationalConstant * star.mass) / dist);
            //Debug.Log(dir * vel);
            planet.initialVelocity = dirShift * (vel + planetPull / planets.Count);
            planet.UpdateComponentValues();
    }

    void CreateStar()
    {
        GameObject solarSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject sun = new GameObject(name = "Sun");
        var sunMesh = Instantiate(solarSphere, sun.transform);
        MeshRenderer sunMeshRend = sunMesh.GetComponent<MeshRenderer>();
        sunMeshRend.material = Resources.Load<Material>("SolarMaterial");
        CelestialBody solarBody = sun.AddComponent<CelestialBody>();
        solarBody.bodyName = "Sun";
        solarBody.radius = Random.Range(MinimumStarSize, MaximumStarSize);
        solarBody.surfaceGravity = Random.Range(180, 260);
        solarBody.Rigidbody.useGravity = false;
        solarBody.UpdateComponentValues();
        Light solarLight = solarBody.gameObject.AddComponent<Light>();
        solarLight.type = LightType.Point;
        solarLight.color = sunMeshRend.material.color;
        solarLight.range = solarBody.radius * solarBody.radius;
        solarLight.intensity = solarIntensity;
        NBodySimulation.Bodies.Add(solarBody);
        star = solarBody;
        Destroy(solarSphere);
    }

    public CelestialBody CreatePlanet(Vector3 position)
    {
        GameObject planetSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject newPlanet = new GameObject(name = "Generic Planet");
        newPlanet.transform.position = position;
        var planetMesh = Instantiate(planetSphere, newPlanet.transform);
        CelestialBody newBody = newPlanet.AddComponent<CelestialBody>();
        newBody.Rigidbody.useGravity = false;
        newBody.bodyName = planetPrefixes[Random.Range(0, planetPrefixes.Length - 1)] + planetSuffixes[Random.Range(0, planetSuffixes.Length - 1)];
        newBody.surfaceGravity = Random.Range(MinimumGravity, MaximumGravity);
        newBody.radius = Random.Range(MinimumRadius, MaximumRadius);
        //float dist = Vector3.Distance(position, star.Position);
        //float vel = Mathf.Sqrt((Universe.gravitationalConstant * star.mass) / dist);
        //Debug.Log(vel);
        //newBody.initialVelocity = new(vel, 0, 0);
        //newBody.initialVelocity = new Vector3(Random.Range(-maxInitialVelocityValues.x, maxInitialVelocityValues.x), Random.Range(-maxInitialVelocityValues.y, maxInitialVelocityValues.y), Random.Range(-maxInitialVelocityValues.z, maxInitialVelocityValues.z));
        newBody.UpdateComponentValues();
        NBodySimulation.Bodies.Add(newBody);
        planets.Add(newBody);
        Destroy(planetSphere);
        return newBody;
    }
    public void DestroyUniverse()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //for(int i = planets.Count; i > 0; i--)
        //{
        //    if(planets[i - 1].bodyName == "Sun")
        //    {
        //        continue;
        //    }
        //    Destroy(planets[i - 1].gameObject);
        //}
        //for (int i = NBodySimulation.Bodies.Count; i > 0; i--)
        //{
        //    if (NBodySimulation.Bodies[i - 1].bodyName == "Sun")
        //    {
        //        continue;
        //    }
        //    Destroy(NBodySimulation.Bodies[i - 1].gameObject);
        //}
    }
}
