using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 3f;
    public Transform target;
    public int range = 10;

    private void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= range) { 
            Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        }
    }
}

