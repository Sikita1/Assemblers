using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private CollectorMover _mover;
    [SerializeField] private CollectorMiner _botMiner;
    [SerializeField] private CollectorBuilder _botBuilder;
    [SerializeField] private TowerFactory _towerFactory;

    private Tower _tower;

    public bool IsWork { get; private set; }

    private void OnEnable()
    {
        _botMiner.CrystalMined += OnCrystalMine;
        _botBuilder.BuildTower += OnBuildTower;
    }

    private void OnDisable()
    {
        _botMiner.CrystalMined -= OnCrystalMine;
        _botBuilder.BuildTower -= OnBuildTower;
    }

    public void SetPrioritizeTask(Crystal crystal, Flag flag)
    {
        if (IsWork == false)
        {
            if (flag != null && flag.IsBusy == false)
            {
                if (flag.Tower != null && flag.Tower == _tower)
                {
                    _mover.SetTarget(flag.transform);
                    flag.Occupy();
                    flag.CloseForScanning();
                    IsWork = true;
                }
            }
            else if (crystal != null && crystal.IsBusy == false)
            {
                MiningCrystal(crystal);
            }
        }
    }

    public void SetTower(Tower tower) =>
        _tower = tower;

    public void FinishTask()
    {
        Crystal crystal = null;

        IsWork = false;
        transform.position = _tower.PointSpawn;

        if (transform.GetComponentInChildren<Crystal>() != null)
        {
            crystal = transform.GetComponentInChildren<Crystal>();
            crystal.Release();
            Destroy(crystal.gameObject);
        }

        _mover.SetTarget(null);
    }

    private void MiningCrystal(Crystal crystal)
    {
        _mover.SetTarget(crystal.transform);
        crystal.Occupy();
        IsWork = true;
    }

    private void OnCrystalMine()
    {
        _mover.SetTarget(_tower.transform);
    }

    private void OnBuildTower(Flag flag)
    {
        Tower tower = _towerFactory.Create(new Vector3(flag.transform.position.x,
                                                       _tower.GetPositionHight(),
                                                       flag.transform.position.z));
        SetTower(tower);
        transform.SetParent(tower.GetContainer());
        IsWork = false;
        transform.position = _tower.PointSpawn;
    }
}
