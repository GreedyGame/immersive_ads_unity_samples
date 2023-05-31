using System;
using System.Collections;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class LevelSettings
    {
        public bool canHaveObstacle = false;
    }
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private FloorHandler startFloor;
        [SerializeField] private GameObject floor;
        [SerializeField] private float distance = 8;
        public LevelSettings levelSettings = new LevelSettings();
        private Vector3 spawnPos;
        private FloorHandler floorHandler;
        private bool spawnLeft;
        private bool firstSpawn;
        private bool SecondSpawn = false;
        private Coroutine generatorCoroutine;
        private Transform player;
        public bool haveObstacle = false;
        private int numberOfFloors;
        private int bestFloor;
        private Vector2 returnPos;
        private HorizontalStatus previousStatus = HorizontalStatus.Closed;
        public void InitGenerator(Transform player)
        {
            haveObstacle = false;
            previousStatus = HorizontalStatus.Closed;
            this.player = player;
            bestFloor = PlayerPrefs.GetInt("BestScore", 0);
            spawnPoint.position = startFloor.InitFloor(HorizontalStatus.Closed, VerticalStatus.TopOpen, false);
            if (generatorCoroutine == null)
                generatorCoroutine = StartCoroutine(GenerateLevel());
        }
        public void RestartGenerator()
        {
            haveObstacle = true;
            previousStatus = HorizontalStatus.Closed;
            if (generatorCoroutine == null)
                generatorCoroutine = StartCoroutine(GenerateLevel());
        }
        public Vector3 GetPos()
        {
            return new Vector3(returnPos.x, returnPos.y, -10);
        }
        public void SpawnHorizontal(bool ans)
        {
            firstSpawn = true;
            spawnLeft = ans;
        }
        void Check()
        {
            numberOfFloors++;
            if (numberOfFloors == bestFloor - 1 && bestFloor > 5)
                floorHandler.EnableBest();
        }
        IEnumerator GenerateLevel()
        {
            while (true)
            {
                if (spawnLeft)
                {
                    if (spawnPoint.position.x - player.position.x < 30)
                    {

                        Check();
                        GameObject floorObject2 = PoolingSystem.instance.Instantiate(floor, spawnPoint.position, Quaternion.identity);
                        spawnPoint.position = new Vector3(spawnPoint.position.x +4.9f, spawnPoint.position.y , spawnPoint.position.z);
                        floorHandler = floorObject2.GetComponent<FloorHandler>();
                        spawnPos = floorHandler.InitFloor(HorizontalStatus.Open, VerticalStatus.TopOpen, true);
                        if (SecondSpawn)
                        {
                            SecondSpawn = false;
                            floorHandler.SetFollowStatus(1);
                        }
                        if (firstSpawn)
                        {
                            SecondSpawn = true;
                            firstSpawn = false;
                            spawnPoint.position = floorHandler.InitFloor(HorizontalStatus.RightOpen, VerticalStatus.Closed, true);
                            floorHandler.EnableArrow();
                            spawnPoint.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y - 0.7f, spawnPoint.position.z);
                            floorHandler.SetFollowStatus(1);
                        }
                    }
                }
                else
                {
                    if (spawnPoint.position.y - player.position.y < distance)
                    {
                        Check();
                        if (firstSpawn)
                        {
                            spawnPoint.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
                        }
                        GameObject floorObject = PoolingSystem.instance.Instantiate(floor, spawnPoint.position, Quaternion.identity);
                        floorHandler = floorObject.GetComponent<FloorHandler>();
                        spawnPoint.position = floorHandler.InitFloor(/*HorizontalStatus.Closed*/ GetAndSetStatus(), VerticalStatus.TopOpen, false);
                        if (UnityEngine.Random.Range(0, 2) == 0 && levelSettings.canHaveObstacle && !haveObstacle)
                        {
                            haveObstacle = true;
                            floorHandler.EnableSaw();
                        }
                        else
                        {
                            if (UnityEngine.Random.Range(0, 2) == 0 && levelSettings.canHaveObstacle)
                                floorHandler.SetGlass();

                            haveObstacle = false;
                        }
                        if (firstSpawn)
                        {
                            firstSpawn = false;
                            spawnPos = floorHandler.InitFloor(HorizontalStatus.LeftOpen, VerticalStatus.TopOpen, false);
                            floorHandler.SetFollowStatus(0);
                            returnPos = spawnPos;
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }
        HorizontalStatus GetAndSetStatus()
        {
            HorizontalStatus wallStatus = (HorizontalStatus)UnityEngine.Random.Range(0, 4);
            switch (previousStatus)
            {
                case HorizontalStatus.LeftOpen:
                    wallStatus = GetStatus(new HorizontalStatus[] { HorizontalStatus.RightOpen, HorizontalStatus.Closed });
                    break;
                case HorizontalStatus.RightOpen:
                    wallStatus = GetStatus(new HorizontalStatus[] { HorizontalStatus.LeftOpen, HorizontalStatus.Closed });
                    break;
                case HorizontalStatus.Open:
                    wallStatus = GetStatus(new HorizontalStatus[] { HorizontalStatus.LeftOpen, HorizontalStatus.RightOpen, HorizontalStatus.Closed });
                    break;
                case HorizontalStatus.Closed:
                    wallStatus = GetStatus(new HorizontalStatus[] { HorizontalStatus.LeftOpen, HorizontalStatus.RightOpen, HorizontalStatus.Open });
                    break;
            }
            previousStatus = wallStatus;
            return wallStatus;
        }
        HorizontalStatus GetStatus(HorizontalStatus[] horizontals)
        {
            return horizontals[UnityEngine.Random.Range(0, horizontals.Length)];
        }
        public void StopGenerating()
        {
            if (generatorCoroutine != null)
            {
                StopCoroutine(generatorCoroutine);
                generatorCoroutine = null;
            }
        }
    }
}