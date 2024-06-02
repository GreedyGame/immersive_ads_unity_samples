
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private float timeToDisable;

    private void OnEnable()
    {
        Invoke(nameof(Disabel), timeToDisable);
    }
    void Disabel()
    {
        objectToDisable.SetActive(false);
    }
    
}
