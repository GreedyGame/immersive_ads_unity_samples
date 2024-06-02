using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    public float rotationSpeed = 30f;

    void Update()
    {
        // Rotate the GameObject around its Y-axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
