using UnityEngine;

public class Balloon : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + 15);
    }
}
