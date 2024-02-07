using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRope : MonoBehaviour
{
    public Vector3 pointToRotateAround; // The point you want to rotate around
    public Vector3 rotationAxis = Vector3.up; // The axis you want to rotate around
    public float rotationSpeed = 45.0f; // Degrees per second

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the transform around the given point and axis
        transform.RotateAround(pointToRotateAround, rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
