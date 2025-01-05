using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private CollectorMover _mover;
    [SerializeField] private CollectorMiner _botMiner;
    [SerializeField] private CollectorBuilder _botBuilder;
    [SerializeField] private TowerFactory _towerFactory;

    private Tower _tower;

    [SerializeField] private bool _isWork;

    public bool IsWork() => _isWork;

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

    public Tower GetTower() =>
        _tower;

    public void SetTaskOnCrystal(Crystal crystal)
    {
        _mover.SetTarget(crystal.transform);
        crystal.Occupy();
        _isWork = true;
    }

    public void SetTaskOnFlag(Flag flag)
    {
        _mover.SetTarget(flag.transform);
        flag.Occupy();
        flag.CloseForScanning();
        _isWork = true;
    }

    public void SetTower(Tower tower) =>
        _tower = tower;

    public void FinishTask()
    {
        Crystal crystal = null;

        _isWork = false;
        transform.position = _tower.PointSpawn;

        if (transform.GetComponentInChildren<Crystal>() != null)
        {
            crystal = transform.GetComponentInChildren<Crystal>();
            crystal.Release();
            Destroy(crystal.gameObject);
        }

        _mover.SetTarget(null);
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
        _isWork = false;
        transform.position = tower.PointSpawn;
    }
}
