using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTower : Tower
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileStartPosition;
    private bool isSkillCoolDown = false;

    public override IEnumerator SearchEnemy()
    {
        towerAnim.SetTrigger("Idle");
        while (true)
        {
            attackTarget = FindTarget();
            towerStatus.upgradeDamage = (towerStatus.attackDamage * 0.1f * GameManager.Instance.MageTowerUpgradeLevel);

            if (attackTarget != null)
            {
                if (skillPrefab != null)
                {
                    StartCoroutine("SkillAttackEnemy");
                }
                ChangeState(TowerState.AttackEnemy);
            }
            yield return null;
        }
    }

    public override IEnumerator AttackEnemy()
    {
        while (true)
        {
            towerStatus.upgradeDamage = (towerStatus.attackDamage * 0.1f * GameManager.Instance.MageTowerUpgradeLevel);
            if (!isPossibleAttack())
            {
                if (skillPrefab != null)
                {
                    StopCoroutine("SkillAttackEnemy");
                }
                ChangeState(TowerState.SearchEnemy);
                break;
            }
            Attack();
            yield return new WaitForSeconds(towerStatus.attackRate);
        }
    }

    private IEnumerator SkillAttackEnemy()
    {
        while (true)
        {
            if (!isSkillCoolDown)
            {
                Skill();
                StartCoroutine("SkillCoolDown");
            }
            yield return null;
        }
    }

    private IEnumerator SkillCoolDown()
    {
        yield return new WaitForSeconds(TowerSkill.SkillCooltime);
        isSkillCoolDown = false;
    }

    public override void Attack()
    {
        towerAnim.SetTrigger("Attack");
        audioSource.PlayOneShot(attackSound);

        GameObject clone = Instantiate(projectilePrefab);
        clone.transform.position = projectileStartPosition.position;
        Projectile projectile = clone.GetComponent<Projectile>();
        projectile.Setup(attackTarget, towerStatus.attackDamage + towerStatus.upgradeDamage);
    }

    public void Skill()
    {
        GameObject clone = Instantiate(skillPrefab);
        clone.transform.position = attackTarget.position;
        Skill skill = clone.GetComponent<Skill>();
        skill.Setup(attackTarget, towerStatus.skillDamage + (towerStatus.skillDamage * 0.2f * GameManager.Instance.MageTowerUpgradeLevel));

        isSkillCoolDown = true;
    }
}
