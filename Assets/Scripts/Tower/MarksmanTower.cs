using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarksmanTower : Tower
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileStartPosition;
    [SerializeField] private int criticalChanceRate;
    [SerializeField] private float criticalMultiplier;

    public override IEnumerator SearchEnemy()
    {
        towerAnim.SetTrigger("Idle");
        
        while (true)
        {
            attackTargets = FindTargets();
            towerStatus.upgradeDamage = (towerStatus.attackDamage * 0.15f * GameManager.Instance.MarksmanTowerUpgradeLevel);

            if (attackTargets[0] != null)
            {
                ChangeState(TowerState.AttackEnemy);
            }
            yield return null;
        }
    }

    public override IEnumerator AttackEnemy()
    {
        while (true)
        {
            attackTargets = FindTargets();
            towerStatus.upgradeDamage = (towerStatus.attackDamage * 0.15f * GameManager.Instance.MarksmanTowerUpgradeLevel);
            
            if (attackTargets[0] == null)
            {
                ChangeState(TowerState.SearchEnemy);
                break;
            }
            Attack();
            yield return new WaitForSeconds(towerStatus.attackRate);
        }
    }

    public bool isCriticalShot()
    {
        int rate = Random.Range(1, 101);

        if (rate < criticalChanceRate)
        {
            return true;
        }

        return false;
    }

    public override void Attack()
    {
        towerAnim.SetTrigger("Attack1");
        audioSource.PlayOneShot(attackSound);
        for (int i = 0; i < attackTargets.Length; i++)
        {
            if (attackTargets[i] == null)
            {
                continue;
            }
            GameObject clone = Instantiate(projectilePrefab);
            clone.transform.position = projectileStartPosition.position;
            Projectile arrow = clone.GetComponent<Projectile>();
            
            if (isCriticalShot())
            {
                arrow.Setup(attackTargets[i], (towerStatus.attackDamage + towerStatus.upgradeDamage) * criticalMultiplier);
                Debug.Log("Ä¡¸íÅ¸!");
            }
            else
            {
                arrow.Setup(attackTargets[i], towerStatus.attackDamage + towerStatus.upgradeDamage);
            }
        }
    }
}
