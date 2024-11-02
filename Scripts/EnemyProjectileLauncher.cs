using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject verticalProjectilePrefab;

    public Transform launchPoint;
    public Transform launchPointVertical;
    public UIManager uiManagerReference;
    private Projectile projectileScript;
    private ProjectileVertical verticalProjectileScript;

    private Vector2 launchPosition;
    public bool upwards = false;
    public int verticalOffset = 8;


    private void Awake()
    {
        uiManagerReference = GameObject.Find("UI Manager").GetComponent<UIManager>();

    }

    public void FireProjectile()
    {
        if (uiManagerReference.arrowCount != 0)
        {
            GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 originalScale = projectile.transform.localScale;
            projectile.transform.localScale = new Vector3(originalScale.x * transform.parent.localScale.x > 0 ? 1 : -1,
                originalScale.y, originalScale.z);

            projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.enemyArrow = true;
            projectileScript.spawnerCharacterObject = this.transform;

        }

    }

    public void FireVerticalProjectile(int offset)
    {
        if (uiManagerReference.arrowCount != 0)
        {
            int rotation;
            if (transform.root.localScale.x < 0)
                offset = -offset;

            if (!upwards)
            {
                verticalOffset = 0;
                rotation = -90;
            }
            else
            {
                verticalOffset = 8;
                rotation = 90;
            }

            Vector2 launchPosition = new Vector2(launchPointVertical.position.x + offset, launchPointVertical.position.y - verticalOffset);
            GameObject verticalProjectile = Instantiate(verticalProjectilePrefab, launchPosition, quaternion.Euler(projectilePrefab.transform.rotation.x, projectilePrefab.transform.rotation.y, rotation));
        }
    }

    public void SetUpwardsValue(int _upwards)
    {
        upwards = _upwards > 0 ? true : false;
    }
}
