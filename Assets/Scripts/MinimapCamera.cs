using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform trackedObject;
    Vector3 offset;
    

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - trackedObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = trackedObject.transform.position + offset;
    }
}
