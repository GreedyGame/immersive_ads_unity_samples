using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace PubScale.SdkOne.NativeAds.Hightower
{
    public enum HorizontalStatus { LeftOpen, RightOpen, Closed, Open }
    public enum VerticalStatus { TopOpen, BottomOpen, Closed }
    public class FloorHandler : PoolObject
    {
        [SerializeField] public Sprite[] windowSprites;
        [SerializeField] public Sprite[] plantsSprite;
        [SerializeField] public Sprite[] doorSprite;
        [SerializeField] public Sprite[] bgTextures;
        [SerializeField] public Color normal;
        [SerializeField] public Color glass;
        [SerializeField] public SpriteRenderer windowRenderer;
        [SerializeField] public SpriteRenderer doorRenderer;
        [SerializeField] public SpriteRenderer foilageRenderer;
        [SerializeField] public SpriteRenderer bgRenderer;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Collider2D col;
        [SerializeField] private SpriteRenderer topWall;
        [SerializeField] private SpriteRenderer bottomWall;
        [SerializeField] private SpriteRenderer leftWall;
        [SerializeField] private SpriteRenderer rightWall;
        [SerializeField] private GameObject arrow;
        [SerializeField] private GameObject best;
        [SerializeField] private GameObject[] obstacles;
        [SerializeField] private bool NormalColor;
        [SerializeField] private GameObject nativeAd;
        public bool first;
        private bool isHorizontal = false;
        private int XStart = -1;

        public override void OnObjectReuse()
        {
            base.OnObjectReuse();
        }
        public void ResetFloor()
        {
            foreach (var item in obstacles)
            {
                item.SetActive(false);
            }
            if (isHorizontal)
            {
                leftWall.gameObject.SetActive(true);
                rightWall.gameObject.SetActive(true);
                rightWall.color = glass;
                rightWall.gameObject.tag = "glass";
            }
            else
            {
                leftWall.gameObject.SetActive(true);
                rightWall.gameObject.SetActive(true);
            }
        }
        void ResetInitFloor()
        {
            best.SetActive(false);
            arrow.SetActive(false);
            foreach (var item in obstacles)
            {

                item.SetActive(false);
            }
            bottomWall.gameObject.SetActive(true);
            col.enabled = true;
            SpriteRenderer r = bottomWall.GetComponent<SpriteRenderer>();
            Color c = r.color;
            r.color = new Color(c.r, c.g, c.b, 1);
            windowRenderer.gameObject.SetActive(false);
            foilageRenderer.gameObject.SetActive(false);
            doorRenderer.gameObject.SetActive(false);
            MakeLevel();
        }
        public void EnableArrow()
        {
            doorRenderer.gameObject.SetActive(false);
            windowRenderer.gameObject.SetActive(false);
            foilageRenderer.gameObject.SetActive(false);
            arrow.SetActive(true);
        }
        public void EnableBest()
        {
            doorRenderer.gameObject.SetActive(false);
            windowRenderer.gameObject.SetActive(false);
            foilageRenderer.gameObject.SetActive(false);
            best.SetActive(true);
        }
        public void MakeLevel()
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    if (Random.Range(0, 4) == 1)
                        return;
                    windowRenderer.gameObject.SetActive(true);
                    windowRenderer.sprite = GetSprite(windowSprites);
                    windowRenderer.transform.localPosition = GetX(windowRenderer.transform.localPosition);
                    break;
                case 1:
                    if (Random.Range(0, 4) % 2 == 0)
                        return;
                    doorRenderer.gameObject.SetActive(true);
                    doorRenderer.sprite = GetSprite(doorSprite);
                    doorRenderer.transform.localPosition = GetX(doorRenderer.transform.localPosition);
                    break;
            }
            if (Random.Range(0, 4) == 1)
            {
                foilageRenderer.sortingOrder = Random.Range(0, 2) == 0 ? -38 : 1;
                foilageRenderer.gameObject.SetActive(true);
                foilageRenderer.sprite = GetSprite(plantsSprite);
                foilageRenderer.transform.localPosition = GetX(foilageRenderer.transform.localPosition);

            }
        }
        public void EnableSaw()
        {
            GameObject selected = obstacles[Random.Range(0, obstacles.Length)];
            selected.SetActive(true);
            doorRenderer.gameObject.SetActive(false);
            windowRenderer.gameObject.SetActive(false);
            foilageRenderer.gameObject.SetActive(false);
            //coin.SetActive(true);
            //coin.transform.localPosition = GetX(coin.transform.localPosition);
        }
        public void BreakGlass(ParticleSystem glassParticle, GameObject gameObject)
        {
            AudioManager.instance.Play("glassBreak", 0.5f, true);
            if (gameObject == leftWall.gameObject)
            {
                glassParticle.transform.position = leftWall.transform.position;
                glassParticle.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                glassParticle.Play();
                leftWall.gameObject.SetActive(false);
            }
            else if (gameObject == rightWall.gameObject)
            {
                glassParticle.transform.rotation = Quaternion.Euler(Vector3.forward * 270);
                glassParticle.transform.position = rightWall.transform.position;
                glassParticle.Play();
                rightWall.gameObject.SetActive(false);
            }
        }
        public void SetGlass()
        {
            if (leftWall.gameObject.activeInHierarchy)
            {
                leftWall.color = glass;
                leftWall.gameObject.tag = "glass";
            }
            else if (rightWall.gameObject.activeInHierarchy)
            {
                rightWall.color = glass;
                rightWall.gameObject.tag = "glass";
            }
        }
        Vector3 GetX(Vector3 pos)
        {
            return new Vector3(Random.Range(-2.1f, 2.1f), pos.y, pos.z);
        }
        Sprite GetSprite(Sprite[] sprites)
        {
            return sprites[Random.Range(0, sprites.Length)];
        }
        public void SetFollowStatus(int XStatus)
        {
            foreach (var item in obstacles)
            {

                item.SetActive(false);
            }
            XStart = XStatus;
        }
        public int GetXStatus()
        {
            return XStart;
        }
        public Vector3 InitFloor(HorizontalStatus horizonal, VerticalStatus vertical, bool isHorizontal)
        {
            ResetInitFloor();
            XStart = -1;
            col.enabled = true;
            leftWall.gameObject.tag = "Untagged";
            rightWall.gameObject.tag = "Untagged";
            leftWall.color = rightWall.color = normal;
            switch (horizonal)
            {
                case HorizontalStatus.LeftOpen:
                    leftWall.gameObject.SetActive(false);
                    rightWall.gameObject.SetActive(true);
                    bgRenderer.sprite = bgTextures[0];
                    break;
                case HorizontalStatus.RightOpen:
                    leftWall.gameObject.SetActive(true);
                    rightWall.gameObject.SetActive(false);
                    bgRenderer.sprite = bgTextures[0];
                    break;
                case HorizontalStatus.Closed:
                    leftWall.gameObject.SetActive(true);
                    rightWall.gameObject.SetActive(true);
                    //nativeAd.gameObject.SetActive(true);
                    bgRenderer.sprite = bgTextures[1];
                    break;
                case HorizontalStatus.Open:
                    leftWall.gameObject.SetActive(false);
                    rightWall.gameObject.SetActive(false);
                    bgRenderer.sprite = bgTextures[0];
                    break;
            }
            switch (vertical)
            {
                case VerticalStatus.TopOpen:
                    topWall.gameObject.SetActive(false);
                    bottomWall.gameObject.SetActive(true);
                    break;
                case VerticalStatus.BottomOpen:
                    topWall.gameObject.SetActive(true);
                    bottomWall.gameObject.SetActive(false);
                    break;
                case VerticalStatus.Closed:
                    topWall.gameObject.SetActive(true);
                    bottomWall.gameObject.SetActive(true);
                    break;
            }
            this.isHorizontal = isHorizontal;
            return spawnPoint.position;
        }
        public void PlayerOnFloor()
        {
            col.enabled = false;
            //if (first)
            //    return;
            //bottomWall.GetComponent<SpriteRenderer>().DOFade(0, timeFade).From(1).OnComplete(() =>
            //{
            //    bottomWall.SetActive(false);
            //});
        }
    }
}