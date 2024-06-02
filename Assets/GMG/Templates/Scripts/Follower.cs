using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform myTransform;
    [SerializeField] private float followSpeed;
    [SerializeField] private Vector3 offset;
    private Vector3 vel;


    private void Update()
    {
        if (target == null || myTransform == null)
            return;
        myTransform.position = Vector3.Lerp(myTransform.position, target.position-offset, followSpeed * Time.deltaTime);
       // myTransform.position = Vector3.SmoothDamp(myTransform.position, target.position,ref vel, followSpeed);
    }
}
