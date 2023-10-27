using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private MissionSystem missionSystem;
    [SerializeField] private TowerInfoViewer towerInfoViewer;
    [SerializeField] private List<Tower> allTowerList;
    [SerializeField] private List<GameObject> level1TowerList;
    [SerializeField] private List<GameObject> level2TowerList;
    [SerializeField] private List<GameObject> level3TowerList;
    
    [SerializeField] private int towerMaxLevel;
    [SerializeField] private int towerCost;

    [SerializeField] private GameObject checkBuildTowerTile;
    [SerializeField] private GameObject selectedEffect;

    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip combineSound;
    [SerializeField] private AudioClip sellSound;

    private List<Tower> fieldTowerList = new List<Tower>(); // �ʵ忡 �����ִ� Ÿ�� ����Ʈ
    private List<TowerStatus> missonTowerList = new List<TowerStatus>(); // �ʵ忡 �����ִ� Ÿ�� ���� ����Ʈ
    private Tower currentSelectedTower;
    private Tower sameTower;

    private GameObject spawnWhetherTileClone;
    private SpawnWhetherTile spawnWhetherTile;

    private bool onTowerSpawnButton = false;

    private Camera mainCam;
    private Ray ray;
    private RaycastHit hit;
    private AudioSource audioSource;

    public List<Tower> AllTowerList
    {
        get { return allTowerList; }
    }
    public List<GameObject> Level1TowerList
    {
        get { return level1TowerList; }
    }
    public List<GameObject> Level2TowerList
    {
        get { return level2TowerList; }
    }
    public List<GameObject> Level3TowerList
    {
        get { return level3TowerList; }
    }
    public List<TowerStatus> MissionTowerList
    {
        get { return missonTowerList; }
    }
    public Tower CurrentSelectedTower
    {
        get { return currentSelectedTower; }
    }

    void Awake()
    {
        mainCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
        GameManager.Instance.GameOverEvent.AddListener(GameOverTower);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (onTowerSpawnButton)
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Field")))
            {
                if (hit.transform.CompareTag("Field"))
                {
                    // Ÿ�� ��ġ ���ɿ��� Ÿ��
                    TowerField towerField = hit.transform.GetComponent<TowerField>();
                    spawnWhetherTile.SetUp(towerField);
                    spawnWhetherTileClone.transform.position = hit.transform.position + (Vector3.up * 1.05f);

                    if (Input.GetMouseButtonDown(0))
                    {
                        SpawnTower(hit.transform);
                        onTowerSpawnButton = false;
                        Destroy(spawnWhetherTileClone);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    onTowerSpawnButton = false;
                    Destroy(spawnWhetherTileClone);
                    Debug.Log("�װ����� ��ġ�� �� �����ϴ�.");
                }
            }
        }
        else if (!onTowerSpawnButton)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = mainCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform.CompareTag("Tower"))
                    {
                        currentSelectedTower = hit.transform.GetComponent<Tower>();
                        towerInfoViewer.OnPanel(currentSelectedTower);
                        OnSelectedTowerEffect(currentSelectedTower.transform);
                    }
                    else
                    {
                        currentSelectedTower = null;
                        towerInfoViewer.OffPanel();
                        OffSelectedTowerEffect();
                    }
                }
            }
        }
    }

    public void OnSelectedTowerEffect(Transform _selectedTower)
    {
        selectedEffect.SetActive(true);
        selectedEffect.transform.position = _selectedTower.position + (Vector3.up * 0.005f);
    }

    public void OffSelectedTowerEffect()
    {
        selectedEffect.SetActive(false);
    }

    public void ReadySpawnTower()
    {
        if (onTowerSpawnButton)
        {
            return;
        }
        if (GameManager.Instance.CurrentGold < towerCost)
        {
            // ���� ���ڶ�ٴ� �ؽ�Ʈ UI ǥ��
            return;
        }

        onTowerSpawnButton = true;
        spawnWhetherTileClone = Instantiate(checkBuildTowerTile);
        spawnWhetherTile = spawnWhetherTileClone.GetComponent<SpawnWhetherTile>();
    }

    public void SpawnTower(Transform _hitTransform)
    {
        TowerField towerField = _hitTransform.GetComponent<TowerField>();
        if (towerField.IsBuild == true)
        {
            return;
        }

        GameManager.Instance.UseGold(towerCost);
        towerField.IsBuild = true;
        GameObject clone = Instantiate(level1TowerList[Random.Range(0, level1TowerList.Count)]);
        clone.transform.position = _hitTransform.position + Vector3.up;
        Tower tower = clone.GetComponent<Tower>();
        TowerLevelBeacon thisBeacon = transform.GetComponent<TowerLevelBeacon>(); // Ÿ�� ������ ���� Ŭ����
        thisBeacon.SetUp(tower);
        tower.Setup(this, enemySpawner, towerField);
        fieldTowerList.Add(tower); // ���� �ʵ忡 �����ִ� Ÿ�� ����Ʈ
        MissionTower(tower); // �̼� üũ�� ����Ʈ ���� �Լ�

        missionSystem.CheckTowerMission(missonTowerList, this); // �̼� üũ

        audioSource.PlayOneShot(spawnSound);
    }

    public void SellTower()
    {
        if (currentSelectedTower == null)
        {
            return;
        }

        currentSelectedTower.ThisTowerField.IsBuild = false;
        fieldTowerList.Remove(currentSelectedTower);

        if (!fieldTowerList.Exists(x => x.TowerStatus.towerName == currentSelectedTower.TowerStatus.towerName))
        {
            missonTowerList.Remove(currentSelectedTower.TowerStatus);
        }

        switch (currentSelectedTower.TowerStatus.towerLevel)
        {
            case 1:
                GameManager.Instance.GetGold(25);
                break;
            case 2:
                GameManager.Instance.GetGold(50);
                break;
            case 3:
                GameManager.Instance.GetGold(75);
                break;
        }

        Destroy(currentSelectedTower.gameObject);

        currentSelectedTower = null;
        towerInfoViewer.OffPanel();
        OffSelectedTowerEffect();

        audioSource.PlayOneShot(sellSound);
    }

    private void MissionTower(Tower _tower)
    {
        if (missonTowerList.Count == 0)
        {
            missonTowerList.Add(_tower.TowerStatus);
        }
        else if(missonTowerList.Count > 0)
        {
            if (!missonTowerList.Exists(x => x.towerName == _tower.TowerStatus.towerName))
            {
                missonTowerList.Add(_tower.TowerStatus);
            }
        }
    }

    public void CombineTower()
    {
        if (currentSelectedTower == null)
        {
            return;
        }
        
        // ������ Ÿ�� Ž��
        List<Tower> sameTowerList = 
            fieldTowerList.FindAll(x => x.TowerStatus.towerName == currentSelectedTower.TowerStatus.towerName 
            && x.TowerStatus.towerLevel <= towerMaxLevel);

        float closetDistance = 1000f;

        if (sameTowerList.Count < 2)
        {
            return;
        }
        if (sameTowerList.Count >= 2)
        {
            for (int i = 0; i < sameTowerList.Count; i++) // ������ Ÿ���� ������ Ÿ���� ���� ����� Ÿ�� Ž��
            {
                float distance = Vector3.Distance(currentSelectedTower.transform.position, sameTowerList[i].transform.position);
                if (distance <= closetDistance && distance > 0)
                {
                    closetDistance = distance;
                    sameTower = sameTowerList[i];
                }
            }
            currentSelectedTower.ThisTowerField.IsBuild = false;
            sameTower.ThisTowerField.IsBuild = false;
            GameObject combinedTower = null;

            switch (currentSelectedTower.TowerStatus.towerLevel)
            {
                case 1:
                    combinedTower = Instantiate(level2TowerList[Random.Range(0, level2TowerList.Count)]);
                    break;
                case 2:
                    combinedTower = Instantiate(level3TowerList[Random.Range(0, level3TowerList.Count)]);
                    break;
            }

            combinedTower.transform.position = currentSelectedTower.transform.position;
            Tower tower = combinedTower.GetComponent<Tower>();
            TowerLevelBeacon thisBeacon = transform.GetComponent<TowerLevelBeacon>();
            thisBeacon.SetUp(tower);
            tower.Setup(this, enemySpawner, currentSelectedTower.ThisTowerField);

            fieldTowerList.Add(tower);
            fieldTowerList.Remove(currentSelectedTower);
            fieldTowerList.Remove(sameTower);
            // �̼� üũ�� Ÿ�� ����Ʈ ����
            if (!fieldTowerList.Exists(x => x.TowerStatus.towerName == currentSelectedTower.TowerStatus.towerName))
            {
                missonTowerList.Remove(currentSelectedTower.TowerStatus);
            }
            if (!missonTowerList.Exists(x => x.towerName == tower.TowerStatus.towerName))
            {
                missonTowerList.Add(tower.TowerStatus);
            }

            Destroy(currentSelectedTower.gameObject);
            Destroy(sameTower.gameObject);

            currentSelectedTower = null;
            towerInfoViewer.OffPanel();

            audioSource.PlayOneShot(combineSound);
        }
    }

    private void GameOverTower()
    {
        for (int i = 0; i < fieldTowerList.Count; i++)
        {
            Destroy(fieldTowerList[i].gameObject);
        }
        fieldTowerList.Clear();
        missonTowerList.Clear();
    }
}
