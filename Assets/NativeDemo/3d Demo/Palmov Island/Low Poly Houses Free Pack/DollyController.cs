using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DollyController : MonoBehaviour
{
    [SerializeField] private CinemachineDollyCart cinemachineDolly;
    [SerializeField] int[] AdsPoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] GameObject buttonObj;
    [SerializeField] private UnityEvent afterEvents;
     int currentAdIndex = 0;
     int currentIndex = 0;
    bool canLoop = false;
    private void LateUpdate()
    {
        if(cinemachineDolly.m_Position>=currentAdIndex&& !canLoop)
            cinemachineDolly.m_Speed = 0;
    }
    private void Awake()
    {
        cinemachineDolly.m_Speed = 0;
        cinemachineDolly.m_Position = currentAdIndex; 
        buttonObj.gameObject.SetActive(true);
    }
    public void MoveDolly()
    {
        currentIndex++;
        if(currentIndex>=AdsPoints.Length)
        {
            canLoop = true;
            afterEvents?.Invoke();
            currentAdIndex++;
            cinemachineDolly.m_Speed = 1;
            buttonObj.gameObject.SetActive(false);
            return;
        }
        currentAdIndex = AdsPoints[currentIndex];
        cinemachineDolly.m_Speed = moveSpeed;
    }
}
