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

    private List<Tower> fieldTowerList = new List<Tower>(); // 필드에 나와있는 타워 리스트
    private List<TowerStatus> missonTowerList = new List<TowerStatus>(); // 필드에 나와있는 타워 종류 리스트
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
                    // 타워 설치 가능여부 타일
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
                    Debug.Log("그곳에는 설치할 수 없습니다.");
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
            // 돈이 모자라다는 텍스트 UI 표시
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
        TowerLevelBeacon thisBeacon = transform.GetComponent<TowerLevelBeacon>(); // 타워 레벨별 비컨 클래스
        thisBeacon.SetUp(tower);
        tower.Setup(this, enemySpawner, towerField);
        fieldTowerList.Add(tower); // 현재 필드에 나와있는 타워 리스트
        MissionTower(tower); // 미션 체크용 리스트 갱신 함수

        missionSystem.CheckTowerMission(missonTowerList, this); // 미션 체크

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
        
        // 동일한 타워 탐색
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
            for (int i = 0; i < sameTowerList.Count; i++) // 동일한 타워중 선택한 타워와 가장 가까운 타워 탐색
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
            // 미션 체크용 타워 리스트 갱신
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
