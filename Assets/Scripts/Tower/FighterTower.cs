using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterTower : Tower
{
    private int attackCount = 0;

    public override IEnumerator SearchEnemy()
    {
        towerAnim.SetTrigger("Idle");
        while (true)
        {
            attackTarget = FindTarget();
            towerStatus.upgradeDamage = (towerStatus.attackDamage * 0.2f * GameManager.Instance.FighterTowerUpgradeLevel);

            if (attackTarget != null)
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
            towerStatus.upgradeDamage = (towerStatus.attackDamage * 0.2f * GameManager.Instance.FighterTowerUpgradeLevel);

            if (!isPossibleAttack())
            {
                ChangeState(TowerState.SearchEnemy);
                break;
            }
            Attack();
            yield return new WaitForSeconds(towerStatus.attackRate);
        }
    }

    public override void Attack()
    {
        Enemy enemy = attackTarget.GetComponent<Enemy>();

        switch (towerStatus.towerName)
        {
            case "SwordTowerLv1":
            case "SwordTowerLv2":
            case "SwordTowerLv3":
            case "SpearTowerLv1":
                towerAnim.SetTrigger("Attack");
                audioSource.PlayOneShot(attackSound);
                enemy.OnDamaged(towerStatus.attackDamage + towerStatus.upgradeDamage);
                break;
            case "SpearTowerLv2":
            case "SpearTowerLv3":
                if (attackCount > 3)
                {
                    towerAnim.SetTrigger("RoundAttack");
                    Skill();
                    attackCount = 0;
                }
                else
                {
                    towerAnim.SetTrigger("Attack");
                    audioSource.PlayOneShot(attackSound);
                    attackCount++;
                    enemy.OnDamaged(towerStatus.attackDamage + towerStatus.upgradeDamage);
                }
                break;
        }
    }

    public void Skill()
    {
        GameObject clone = Instantiate(skillPrefab);
        clone.transform.position = this.transform.position;
        Skill skill = clone.GetComponent<Skill>();
        skill.Setup(attackTarget, towerStatus.skillDamage + (towerStatus.skillDamage * 0.2f * GameManager.Instance.FighterTowerUpgradeLevel));
    }
}
