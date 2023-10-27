using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeSystem : MonoBehaviour
{
    [SerializeField] private int maxUpgradeLevel;
    private int fighterTowerUpgradeCost = 20;
    private int mageTowerUpgradeCost = 20;
    private int marksmanTowerUpgradeCost = 20;

    public int FighterTowerUpgradeCost
    {
        get { return fighterTowerUpgradeCost; }
    }
    public int MarksmanTowerUpgradeCost
    {
        get { return marksmanTowerUpgradeCost; }
    }
    public int MageTowerUpgradeCost
    {
        get { return mageTowerUpgradeCost; }
    }

    public void UpgradeFighterTower()
    {
        if (GameManager.Instance.FighterTowerUpgradeLevel >= maxUpgradeLevel || GameManager.Instance.CurrentGold < fighterTowerUpgradeCost)
        {
            // µ· ºÎÁ· UI Ãâ·Â
            return;
        }
        GameManager.Instance.UseGold(fighterTowerUpgradeCost);
        GameManager.Instance.FighterTowerUpgradeLevel++;
        fighterTowerUpgradeCost = fighterTowerUpgradeCost + 5;
    }

    public void UpgradeMageTower()
    {
        if (GameManager.Instance.MageTowerUpgradeLevel >= maxUpgradeLevel || GameManager.Instance.CurrentGold < mageTowerUpgradeCost)
        {
            // µ· ºÎÁ· UI Ãâ·Â
            return;
        }
        GameManager.Instance.UseGold(mageTowerUpgradeCost);
        GameManager.Instance.MageTowerUpgradeLevel++;
        mageTowerUpgradeCost = mageTowerUpgradeCost + 5;
    }

    public void UpgradeMarksmanTower()
    {
        if (GameManager.Instance.MarksmanTowerUpgradeLevel >= maxUpgradeLevel || GameManager.Instance.CurrentGold < marksmanTowerUpgradeCost)
        {
            // µ· ºÎÁ· UI Ãâ·Â
            return;
        }
        GameManager.Instance.UseGold(marksmanTowerUpgradeCost);
        GameManager.Instance.MarksmanTowerUpgradeLevel++;
        marksmanTowerUpgradeCost = marksmanTowerUpgradeCost + 5;
    }
}
