using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSystem : MonoBehaviour
{
    [SerializeField] private List<Mission> missionList;
    [SerializeField] private MissionViewer missionViewer;
    [SerializeField] private KillCountMission killMission;

    public List<Mission> MissionList
    {
        get { return missionList; }
    }

    public void CheckTowerMission(List<TowerStatus> _missionTowerList, TowerManager _towerManager)
    {
        for (int i = 0; i < missionList.Count; i++)
        {
            missionList[i].CheckMission(_missionTowerList, missionViewer, _towerManager);
        }
    }

    public void CheckKillMission()
    {
        killMission.KillMission(missionViewer);
    }
}
