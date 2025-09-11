using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileDamage;
    public float knockForce;

    public GameObject destroyParticlePrefab;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().takeDamage(projectileDamage);
            other.gameObject.GetComponent<Enemy>().takeKnockBack(projectileDirection, knockForce);
            destroyProjectile();
        }
        else if (!other.gameObject.CompareTag("Player"))
        {
            destroyProjectile();
        }
    }

    public void destroyProjectile()
    {
        GameObject particleInstance = Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
