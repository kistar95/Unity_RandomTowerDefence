using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Transform attackTarget;
    [SerializeField] private GameObject hit;
    [SerializeField] private GameObject flash;
    private Movement movement;
    private Rigidbody rigid;
    private bool useFirePointRotation;
    private float projectileDamage;

    public void Setup(Transform _attackTarget, float _damage)
    {
        movement = GetComponent<Movement>();
        rigid = GetComponent<Rigidbody>();
        attackTarget = _attackTarget;
        projectileDamage = _damage;
    }

    private void Awake()
    {
        GameManager.Instance.GameOverEvent.AddListener(GameOverProjectile);
    }

    private void Start()
    {
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    private void Update()
    {
        if (attackTarget.gameObject.activeSelf == true)
        {
            Vector3 dir = (attackTarget.position - transform.position).normalized;
            movement.SetDirection(dir);
        }
        else if (attackTarget.gameObject.activeSelf == false)
        {
            Destroy(this.gameObject);
        }
    }

    private void GameOverProjectile()
    {
        attackTarget = null;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == attackTarget)
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();
            enemy.OnDamaged(projectileDamage);

            rigid.constraints = RigidbodyConstraints.FreezeAll;
            movement.MoveSpeed = 0;

            if (hit != null)
            {
                GameObject hitInstance = Instantiate(hit);
                hitInstance.transform.position = attackTarget.position;
                hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0);

                //Destroy hit effects depending on particle Duration time
                ParticleSystem hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                }
            }

            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
