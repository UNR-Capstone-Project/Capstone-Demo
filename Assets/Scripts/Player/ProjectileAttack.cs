using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public float projectileSpeed;

    private Vector3 projectileDirection;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += projectileDirection * projectileSpeed * Time.deltaTime;
    }

    public void setProjectileDirection(Vector3 direction)
    {
        projectileDirection = direction;
    }
}
