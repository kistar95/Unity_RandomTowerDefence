using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundAttack : Skill
{
    private void Start()
    {
        StartCoroutine("ColliderEnableAndDisable");
    }

    public override IEnumerator ColliderEnableAndDisable()
    {
        yield return new WaitForSeconds(0.1f);
        skillCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        skillCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.OnDamaged(skillDamage);
        }
    }
}
