using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panelList;
    
    public GameObject CurrentOnPanel { get; set; }

    public void OnPanel(GameObject _currentOnPanel)
    {
        CurrentOnPanel = _currentOnPanel;
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].SetActive(false);
        }
        CurrentOnPanel.SetActive(true);
    }

    public void OffPanel()
    {
        CurrentOnPanel = null;
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].SetActive(false);
        }
    }
}
