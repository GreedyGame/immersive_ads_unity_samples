using System.Collections;
using UnityEngine;
using DG.Tweening;
public class FlipPanel : MonoBehaviour
{
    [SerializeField] GameObject frontSide;
    [SerializeField] GameObject backSide;
    // Start is called before the first frame update
    void Start()
    {
        frontSide.SetActive(false);
        backSide.SetActive(false);

        StartCoroutine(DoFlip());
    }

    IEnumerator DoFlip()
    {
        while (true) 
        {
            backSide.SetActive(false);
            frontSide.SetActive(true); 
            yield return new WaitForSeconds(2f);
            transform.DORotate(new Vector2(0, 90), 1f, RotateMode.Fast);
            yield return new WaitForSeconds(1f);
            transform.DORotate(new Vector2(0, 0), 1f, RotateMode.Fast);
            backSide.SetActive(true);
            frontSide.SetActive(false);
            yield return new WaitForSeconds(2f);
            transform.DORotate(new Vector2(0, 90), 1f, RotateMode.Fast);
            yield return new WaitForSeconds(1f);
            transform.DORotate(new Vector2(0, 0), 1f, RotateMode.Fast);
        }

    }
}
