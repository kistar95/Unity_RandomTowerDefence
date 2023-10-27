using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text missionClearText;
    [SerializeField] private GameObject missionClearPanel;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioClip missionClearSound;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel()
    {
        uiManager.OnPanel(this.gameObject);
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator OnMissionClearPanel(string _missionName)
    {
        missionClearText.text = _missionName;
        missionClearPanel.SetActive(true);
        SoundManager.Instance.PlaySystemSound(missionClearSound);
        yield return new WaitForSeconds(3f);
        missionClearPanel.SetActive(false);
    }
}
