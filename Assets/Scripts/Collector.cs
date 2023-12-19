using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private CollectorMover _mover;
    [SerializeField] private CollectorMiner _botMiner;

    private Tower _tower;

    public bool IsWork { get; private set; } = false;

    private void OnEnable()
    {
        _botMiner.OnCrystalMined += OnCrystalMine;
    }

    private void OnDisable()
    {
        _botMiner.OnCrystalMined -= OnCrystalMine;
    }

    private void OnCrystalMine()
    {
        _mover.SetTarget(_tower.transform);
    }

    public void SetCrystal(Crystal crystal)
    {
        if (IsWork == false)
        {
            if (crystal != null)
            {
                _mover.SetTarget(crystal.transform);
                IsWork = true;
            }
        }
    }

    public void SetTower(Tower tower) =>
        _tower = tower;

    public void OnFinishTask()
    {
        IsWork = false;
        transform.position = _tower.PointSpawn;
        Crystal crystal = transform.GetComponentInChildren<Crystal>();
        Destroy(crystal.gameObject);
        _mover.SetTarget(null);
    }
}
