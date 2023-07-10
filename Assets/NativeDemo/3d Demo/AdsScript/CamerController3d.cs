using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CamerController3d : MonoBehaviour
{
    [SerializeField] private Transform initPosition;
    [SerializeField] private Transform[] AdPositions;
    [SerializeField] private float moveSpeed;
    private int currentAdPosition = 0;
    private bool isMoving;
    private bool isFirstMove= true;
    private void Awake()
    {
        isFirstMove = true;
        transform.position = initPosition.position;
        transform.DORotate(new Vector3(initPosition.eulerAngles.x, 360, 0), 30, RotateMode.FastBeyond360).From(new Vector3(initPosition.eulerAngles.x, 0, 0)).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetId("CameraLoop");
    }
    public void ShowNextAd()
    {
        if(isMoving)
        {
            return;
        }
        currentAdPosition++;
        if (currentAdPosition >= AdPositions.Length)
            currentAdPosition = 0;
        isMoving = true;
        if (isFirstMove)
        {
            DOTween.Kill("CameraLoop");
            MoveCam(true);
            isFirstMove = false;
        }
        else
        {
            MoveCam(false);
        }
    }

    private void MoveCam(bool isFirst)
    {
        Transform ADPosition = AdPositions[currentAdPosition];
        Vector3 secondPosition = new Vector3(ADPosition.position.x, initPosition.position.y, ADPosition.position.z);
        Vector3 targetPosition = ADPosition.position;
        if (isFirst)
        {
            MovePositions(secondPosition, targetPosition);
        }
        else
        {
            Vector3 fistPosition = new Vector3(transform.position.x, initPosition.position.y, transform.position.z);
            transform.DORotate(initPosition.rotation.eulerAngles, moveSpeed);
            transform.DOMove(fistPosition, moveSpeed).OnComplete(() =>
            {
                MovePositions(secondPosition, targetPosition);
            });
        }
    }
    void MovePositions(Vector3 first,Vector3 second)
    {

        transform.DOMove(first, moveSpeed).OnComplete(() =>
        {
            transform.DORotate(AdPositions[currentAdPosition].rotation.eulerAngles, moveSpeed);
            transform.DOMove(second, moveSpeed).OnComplete(() =>
            {
                isMoving = false;
            });
        });
    }

}
