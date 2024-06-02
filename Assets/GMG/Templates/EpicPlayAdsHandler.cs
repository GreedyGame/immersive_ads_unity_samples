using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicPlayAdsHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] AdsObject;
    [SerializeField] private float delayEnable;

    private void Awake()
    {
        Invoke(nameof(DelayFunction), Random.Range(2, delayEnable));
    }
    void DelayFunction()
    {
        AdsObject[Random.Range(0, AdsObject.Length)].SetActive(true);
    }

}
