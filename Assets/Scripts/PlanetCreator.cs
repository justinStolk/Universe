using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreator : MonoBehaviour
{
    public bool getPlacementInput { get; set; }

    [SerializeField] private SolarSystemGenerator generator; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeRandomPlanet();
        }
    }

    public void MakeRandomPlanet()
    {

        Vector3 spawnPoint = new(Random.Range(generator.MinPlanetDistance, generator.MaxDistanceFromStar), Random.Range(-30, 30), Random.Range(generator.MinPlanetDistance, generator.MaxPlanetDistance));
        generator.SetPlanetVelocity(generator.CreatePlanet(spawnPoint));
    }

}
