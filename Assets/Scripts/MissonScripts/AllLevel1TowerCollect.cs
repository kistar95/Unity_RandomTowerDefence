using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLevel1TowerCollect : Mission
{
    public override void CheckMission(List<TowerStatus> _missionTowerList, MissionViewer _missionViewer, TowerManager _towerManager)
    {
        if (isClear)
        {
            return;
        }

        if (_missionTowerList.FindAll(x => x.towerLevel == 1).Count == _towerManager.Level1TowerList.Count)
        {
            GameManager.Instance.GetGold(100);
            isClear = true;
            MissionClearTextUpdate(missionText, _missionViewer);
        }
    }
}
