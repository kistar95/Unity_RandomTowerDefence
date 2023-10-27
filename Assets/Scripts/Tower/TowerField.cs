using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerField : MonoBehaviour
{
    public bool IsBuild { get; set; }

    private void Awake()
    {
        IsBuild = false;
    }
}
