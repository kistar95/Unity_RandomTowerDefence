using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Mission : MonoBehaviour
{
    [SerializeField] protected TMP_Text missionText;
    protected bool isClear = false;

    public bool IsClear
    {
        get { return isClear; }
    }

    public abstract void CheckMission(List<TowerStatus> _missionTowerList, MissionViewer _missionViewer, TowerManager _towerManager);

    public void MissionClearTextUpdate(TMP_Text _missionText, MissionViewer _missionViewer)
    {
        _missionText.fontStyle = FontStyles.Strikethrough;
        StartCoroutine(_missionViewer.OnMissionClearPanel(_missionText.text));
    }
}
