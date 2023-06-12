using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    [SerializeField] private Rigidbody2D cubeRB;
    [SerializeField] private float force;

    public void AddForceNow()
    {
        Debug.Log("Adding");
        cubeRB.AddTorque(force / 2);
        cubeRB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}
