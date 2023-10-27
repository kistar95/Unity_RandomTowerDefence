using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCountMission : Mission
{
    [SerializeField] private TMP_Text missionText_1;
    [SerializeField] private TMP_Text missionText_2;
    private int count;

    public override void CheckMission(List<TowerStatus> _missionTowerList, MissionViewer _missionViewer, TowerManager _towerManager)
    {
        return;
    }

    public void KillMission(MissionViewer _missionViewer)
    {
        switch (GameManager.Instance.EnemyKillCount)
        {
            case 100:
                if (count == 0)
                {
                    MissionClearTextUpdate(missionText, _missionViewer);
                    GameManager.Instance.GetGold(100);
                    count++;
                }
                break;
            case 200:
                if (count == 1)
                {
                    MissionClearTextUpdate(missionText_1, _missionViewer);
                    GameManager.Instance.GetGold(150);
                    count++;
                }
                break;
            case 300:
                if (count == 2)
                {
                    MissionClearTextUpdate(missionText_2, _missionViewer);
                    GameManager.Instance.GetGold(200);
                    count++;
                }
                break;
        }
    }
}
