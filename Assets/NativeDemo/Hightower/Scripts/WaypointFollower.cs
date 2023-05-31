using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class WaypointFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Collider2D col;
        [SerializeField] private float speed;
        [SerializeField] private float Delay;
        private int index;
        private bool canMove = true;

        private void Awake()
        {
            canMove = true;
        }
        private void OnEnable()
        {
            if (col != null)
                col.enabled = false;
            canMove = false;
            speed = Random.Range(speed, speed + 2);
            Delay = Random.Range(Delay, Delay + 2);
            Invoke(nameof(StartNow), Random.Range(0, 5f));
        }
        void StartNow()
        {
            canMove = true;
        }
        private void Update()
        {
            if (canMove)
            {
                target.localPosition = Vector2.MoveTowards(target.localPosition, waypoints[index].transform.localPosition, speed * Time.deltaTime);
                if (target.localPosition == waypoints[index].transform.localPosition)
                {
                    if (col != null)
                    {
                        if (index == 1)
                            col.enabled = false;
                        else
                            col.enabled = true;
                    }
                    index += 1;
                    StartCoroutine(DelayMovement());
                }
                if (index == waypoints.Length)
                {
                    index = 0;
                }
            }
        }
        IEnumerator DelayMovement()
        {
            canMove = false;
            yield return new WaitForSeconds(Delay);
            if (col != null)
                col.enabled = false;
            canMove = true;
        }
    }
}