using System.Collections;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private BotMover _mover;
    [SerializeField] private BotMiner _botMiner;

    private Crystal _target;
    private Tower _tower;

    public bool IsWork { get; private set; } = false;

    private void OnEnable()
    {
        _botMiner.OnCrystalMined += OnCrystalMine;
        //_tower.OnFreeCollector += OnSetTask;
    }

    private void OnDisable()
    {
        _botMiner.OnCrystalMined -= OnCrystalMine;
        //_tower.OnFreeCollector -= OnSetTask;
    }

    public bool Occupy() => IsWork = true;

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
        _mover.SetTarget(null);
        transform.position = _tower.PointSpawn;
        Crystal crystal = transform.GetComponentInChildren<Crystal>();
        Destroy(crystal.gameObject);
    }
}