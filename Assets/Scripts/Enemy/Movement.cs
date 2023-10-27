using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDirection;
    private float originSpeed;

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = Mathf.Max(0, value); }
    }

    private void Start()
    {
        originSpeed = moveSpeed;
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void SetDirection(Vector3 _dir)
    {
        moveDirection = _dir;
        transform.rotation = Quaternion.LookRotation(_dir); ;
    }

    public void ResetSpeed()
    {
        moveSpeed = originSpeed;
    }
}
