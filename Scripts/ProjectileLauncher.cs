using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform launchPoint;
    public Transform launchPointFourtyFive;
    public UIManager uiManagerReference;
    private Projectile projectileScript;


    public bool fourtyFive;


    public void FireProjectile()
    {
        if (uiManagerReference.arrowCount != 0)
        {
            fourtyFive = false;
            GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 originalScale = projectile.transform.localScale;
            projectile.transform.localScale = new Vector3(originalScale.x * transform.parent.localScale.x > 0 ? 1 : -1,
                originalScale.y, originalScale.z);

            uiManagerReference.arrowCount--;
            print($"Arrow Count: {uiManagerReference.arrowCount}");
            projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.spawnerCharacterObject = this.transform;

        }

    }

    public void FireProjectileFourtyFiveDegree()
    {
        if (uiManagerReference.arrowCount != 0)
        {
            fourtyFive = true;

            int localScaleModifier = gameObject.transform.parent.localScale.x > 0 ? 1 : -1;
            localScaleModifier = localScaleModifier > 0 ? 45 : -45;


            Quaternion rotation = Quaternion.Euler(projectilePrefab.transform.rotation.x, projectilePrefab.transform.rotation.y, 45 * transform.parent.localScale.x > 0 ? 1 : -1);
            GameObject projectile = Instantiate(projectilePrefab, launchPointFourtyFive.position, Quaternion.Euler(0, 0, -localScaleModifier));
            Vector3 originalScale = projectile.transform.localScale;
            projectile.transform.localScale = new Vector3(originalScale.x * transform.parent.localScale.x > 0 ? 1 : -1,
                originalScale.y, originalScale.z);

            uiManagerReference.arrowCount--;
            projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.spawnerCharacterObject = this.transform;
            print($"Arrow Count: {uiManagerReference.arrowCount}");
        }
            


    }
}
