using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerInfoViewer : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerAttackDamageText;
    [SerializeField] private TextMeshProUGUI towerUpgradeDamageText;
    [SerializeField] private TextMeshProUGUI towerAttackRateText;
    [SerializeField] private TextMeshProUGUI towerUpgradeRateText;
    [SerializeField] private TextMeshProUGUI towerAttackRangeText;
    [SerializeField] private TextMeshProUGUI towerUpgradeRangeText;
    [SerializeField] private Image skillBorder;
    [SerializeField] private Image skillImage;

    [SerializeField] private TowerManager towerManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private List<Tower> towerInfoList;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Tower _tower)
    {
        for (int i = 0; i < towerInfoList.Count; i++)
        {
            if (towerManager.CurrentSelectedTower.TowerStatus.towerName == towerInfoList[i].TowerStatus.towerName)
            {
                towerInfoList[i].gameObject.SetActive(true);
            }
            else
            {
                towerInfoList[i].gameObject.SetActive(false);
            }
        }

        towerAttackDamageText.text = _tower.TowerStatus.attackDamage.ToString("F1");
        towerUpgradeDamageText.text = " (+" + _tower.TowerStatus.upgradeDamage.ToString("F1") + ")";
        towerAttackRateText.text = _tower.TowerStatus.attackRate.ToString("F1");
        towerAttackRangeText.text = _tower.TowerStatus.attackRange.ToString("F1");

        if (towerManager.CurrentSelectedTower.TowerSkill != null)
        {
            skillImage.sprite = towerManager.CurrentSelectedTower.TowerSkill.SkillSprite;
            skillBorder.gameObject.SetActive(true);
        }
        else
        {
            skillBorder.gameObject.SetActive(false); 
        }

        uiManager.OnPanel(this.gameObject);
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
    }
}
