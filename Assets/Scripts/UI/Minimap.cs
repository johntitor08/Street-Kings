using UnityEngine;

public class Minimap : MonoBehaviour
{
    private void LateUpdate()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
            return;

        Vector3 pos = player.position;
        transform.SetPositionAndRotation(new Vector3(pos.x, pos.y, -10), Quaternion.Euler(0, player.eulerAngles.y, 0));
    }
}
