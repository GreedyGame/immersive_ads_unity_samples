using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    [SerializeField] private string[] Strings;
    [SerializeField] private float waitTime=1f;
    [SerializeField] private TextMeshProUGUI targetTxt;
    [SerializeField] private TextMesh text;
    private Coroutine coroutine;
    private void Awake()
    {
        coroutine= StartCoroutine(AnimateTxt());
    }
    IEnumerator AnimateTxt()
    {
        for (int i = 0; i <= Strings.Length; i++)
        {
            if (targetTxt != null)
                targetTxt.text = Strings[i];
            else
            text.text= Strings[i];
            if (i == Strings.Length - 1)
                i = -1;
            yield return new WaitForSeconds(waitTime);
        }
    }
    private void OnDestroy()
    {
        StopCoroutine(coroutine);
    }
}
