using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevelBeacon : MonoBehaviour
{
    [SerializeField] private List<GameObject> beaconList;
    private GameObject beacon;
    private Tower targetTower;

    public void SetUp(Tower _tower)
    {
        targetTower = _tower;
        beacon = Instantiate(beaconList[targetTower.TowerStatus.towerLevel - 1]);
        beacon.transform.position = targetTower.transform.position + (Vector3.up * 0.005f);
        beacon.transform.SetParent(_tower.transform);
    }
}
