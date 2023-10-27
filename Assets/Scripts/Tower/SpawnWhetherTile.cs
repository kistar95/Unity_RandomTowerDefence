using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWhetherTile : MonoBehaviour
{
    [SerializeField] private Color spawnPossibleColor;
    [SerializeField] private Color spawnImpossibleColor;
    private Material mat;
    private TowerField towerField;

    public void SetUp(TowerField _towerField)
    {
        mat = GetComponent<MeshRenderer>().material;
        towerField = _towerField;
    }

    private void Update()
    {
        if (towerField == null)
        {
            return;
        }

        if (!towerField.IsBuild)
        {
            mat.color = spawnPossibleColor;
        }
        else
        {
            mat.color = spawnImpossibleColor;
        }
        
    }
}
