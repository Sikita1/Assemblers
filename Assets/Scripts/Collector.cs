using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private CollectorMover _mover;
    [SerializeField] private CollectorMiner _botMiner;
    [SerializeField] private CollectorBuilder _botBuilder;
    [SerializeField] private TowerFactory _towerFactory;

    [SerializeField] private bool _isWork = false;

    private Tower _tower;

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

    public bool IsWork() =>
        _isWork;

    public Tower Tower =>
        _tower;

    public void SetPrioritizeTask(Crystal crystal, Flag flag)
    {
        if (_isWork == false)
        {
            if (flag != null && flag.IsBusy == false)
            {
                _mover.SetTarget(flag.transform);
                flag.Occupy();
                flag.CloseForScanning();
                _isWork = true;
            }
            else if (crystal != null && crystal.IsBusy == false)
            {
                _mover.SetTarget(crystal.transform);
                crystal.Occupy();
                _isWork = true;
            }
        }
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
        transform.position = _tower.PointSpawn;
    }
}
