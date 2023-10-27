using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerUpgradeViewer : MonoBehaviour
{
    [SerializeField] private TowerUpgradeSystem towerUpgradeSystem;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private TMP_Text fighterTowerUpgradeLevel;
    [SerializeField] private TMP_Text fighterTowerUpgradeCost;
    [SerializeField] private TMP_Text marksmanTowerUpgradeLevel;
    [SerializeField] private TMP_Text marksmanTowerUpgradeCost;
    [SerializeField] private TMP_Text mageTowerUpgradeLevel;
    [SerializeField] private TMP_Text mageTowerUpgradeCost;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void UpdateUpgradeData()
    {
        fighterTowerUpgradeLevel.text = GameManager.Instance.FighterTowerUpgradeLevel.ToString();
        fighterTowerUpgradeCost.text = towerUpgradeSystem.FighterTowerUpgradeCost.ToString();
        marksmanTowerUpgradeLevel.text = GameManager.Instance.MarksmanTowerUpgradeLevel.ToString();
        marksmanTowerUpgradeCost.text = towerUpgradeSystem.MarksmanTowerUpgradeCost.ToString();
        mageTowerUpgradeLevel.text = GameManager.Instance.MageTowerUpgradeLevel.ToString();
        mageTowerUpgradeCost.text = towerUpgradeSystem.MageTowerUpgradeCost.ToString();
    }

    public void OnPanel()
    {
        UpdateUpgradeData();
        //gameObject.SetActive(true);
        uiManager.OnPanel(this.gameObject);
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
    }
}
