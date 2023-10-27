using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMageTowerCollect : Mission
{
    public override void CheckMission(List<TowerStatus> _missionTowerList, MissionViewer _missionViewer, TowerManager _towerManager)
    {
        if (isClear)
        {
            return;
        }

        int count = _towerManager.AllTowerList.FindAll(x => x.TowerStatus.towerType == TowerType.MAGE).Count;

        if (_missionTowerList.FindAll(x => x.towerType == TowerType.MAGE).Count == count)
        {
            // 미션 클리어 UI
            GameManager.Instance.GetGold(100);
            isClear = true;
            MissionClearTextUpdate(missionText, _missionViewer);
        }
    }
}
