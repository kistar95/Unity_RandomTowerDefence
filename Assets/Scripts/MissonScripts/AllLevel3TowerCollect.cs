using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLevel3TowerCollect : Mission
{
    public override void CheckMission(List<TowerStatus> _missionTowerList, MissionViewer _missionViewer, TowerManager _towerManager)
    {
        if (isClear)
        {
            return;
        }

        if (_missionTowerList.FindAll(x => x.towerLevel == 3).Count == _towerManager.Level3TowerList.Count)
        {
            // 미션 클리어 UI
            GameManager.Instance.GetGold(200);
            isClear = true;
            MissionClearTextUpdate(missionText, _missionViewer);
        }
    }
}
