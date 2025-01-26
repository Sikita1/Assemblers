using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    [Header("CollectorFactory")]
    [SerializeField] private int _collectorsCount;
    [SerializeField] private CollectorFactory _collectorFactory;
    [SerializeField] private Transform _container;

    [SerializeField] private Scanner _scanner;

    [SerializeField] private TowersCrystals _counterCrystal;
    [SerializeField] private Flag _flag;

    [SerializeField] TemporaryCrystalVault _temporaryCrystal = new TemporaryCrystalVault();

    private List<Crystal> _freeCrystals;
    private List<Crystal> _occupiedCrystals = new List<Crystal>();
    private List<Collector> _collectors = new List<Collector>();

    private WaitForSeconds _delay;

    private float _interval = 1f;

    private int _resourcesForNewCollector = 3;
    private int _resourcesForNewTower = 5;

    private float _minRandomPositionX = 20f;
    private float _maxRandomPositionX = 15f;
    private float _positionY = 3f;
    private float _minRandomPositionZ = 15;
    private float _maxRandomPositionZ = 20f;

    private Coroutine _corutine;

    public event UnityAction Delivery;

    public Vector3 PointSpawn { get; private set; }
    public bool FlagIsPlanted { get; private set; }

    private void Awake()
    {
        CreateCollectors();
    }

    private void Start()
    {
        if (_corutine != null)
            StopCoroutine(_corutine);

        _corutine = StartCoroutine(TaskAcquisition());
    }

    private void OnEnable()
    {
        //_scanner.Scanned += OnScannedCrystal;
        //_scanner.ScannedFlag += OnScannedFlag;
    }

    private void OnDisable()
    {
        //_scanner.Scanned -= OnScannedCrystal;
        //_scanner.ScannedFlag -= OnScannedFlag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Collector collector))
        {
            if (FindYourDebtCollector(collector))
            {
                if (collector.IsWork())
                {
                    Crystal crystal = collector.gameObject.transform.GetComponentInChildren<Crystal>();

                    if (crystal != null)
                    {
                        //_occupiedCrystals.Remove(crystal);
                        _temporaryCrystal.RemoveOccupiedCrystal(crystal);
                        collector.FinishTask();
                        Delivery?.Invoke();
                    }
                }
            }
        }
    }

    public bool CollectorsEnoughToBuild() =>
        _container.childCount > 1;

    public float GetPositionHight() =>
        _positionY;

    public Transform GetContainer() =>
        _collectorFactory.GetContainer();

    public void RemoveChildren(Collector collector) =>
        _collectors.Remove(collector);

    public void AddFromChildren(Collector collector) =>
        _collectors.Add(collector);

    //private void OnScannedFlag(Flag flag)
    //{
    //    flag = _scanner.GetFlag();
    //    _flag = flag;
    //}

    private void OnScannedCrystal(List<Crystal> crystals)
    {
        //_crystals = new List<Crystal>();
        //crystals = _scanner.GetCrystals();
        _freeCrystals = crystals;
    }

    private void SetPriorityTask()
    {
        IEnumerable<Crystal> crystals = _scanner.Scan();

        Collector collector = GetFreeCollector();

        if (collector == null)
            return;

        crystals = _temporaryCrystal.GetFreeCrystal(crystals);

        if (crystals.Any() == false)
            return;

        foreach (Crystal crystal in crystals)
        {
            if (collector != null)
            {
                _temporaryCrystal.OccupiedCrystal(crystal);
                collector.SetTaskOnCrystal(crystal);
            }
        }


        //    Collector collector = GetFreeCollector();

        //    if (flag != null && flag.IsBusy() == false)
        //    {
        //        if (flag.Tower == collector.GetTower())
        //        {
        //            if (_counterCrystal.Amount >= _resourcesForNewTower)
        //            {
        //                collector.SetTaskOnFlag(flag);
        //                _counterCrystal.WriteOffCrystals(_resourcesForNewTower);
        //            }
        //            else if (crystal != null && crystal.IsBusy() == false)
        //            {
        //                collector.SetTaskOnCrystal(crystal);
        //            }
        //        }
        //        else if (crystal != null && crystal.IsBusy() == false)
        //        {
        //            collector.SetTaskOnCrystal(crystal);
        //        }
        //    }
        //    else
        //    {
        //        if (_counterCrystal.Amount >= _resourcesForNewCollector)
        //        {
        //            CreateCollector();
        //            _counterCrystal.WriteOffCrystals(_resourcesForNewCollector);
        //        }
        //        else
        //        {
        //            if (crystal != null && crystal.IsBusy() == false)
        //            {
        //                collector.SetTaskOnCrystal(crystal);
        //            }
        //        }
        //    }
    }

    private void CreateCollectors()
    {
        for (int i = 0; i < _collectorsCount; i++)
            CreateCollector();
    }

    private void CreateCollector()
    {
        Collector collector;

        Vector3 position = GetRandomPositionNewCollector();

        PointSpawn = position;
        collector = _collectorFactory.Create(position, this);
        _collectors.Add(collector);
    }

    private Vector3 GetRandomPositionNewCollector()
    {
        return new Vector3(Random.Range(gameObject.transform.position.x + _minRandomPositionX,
                                                        gameObject.transform.position.x + _maxRandomPositionX),
                                           _positionY,
                                           Random.Range(gameObject.transform.position.z + _minRandomPositionZ,
                                                        gameObject.transform.position.z + _maxRandomPositionZ));
    }

    private Collector GetFreeCollector() =>
        _collectors.FirstOrDefault(collector => collector.IsWork() == false);

    private bool FindYourDebtCollector(Collector collector)
    {
        Collector[] ownCollector = _container.GetComponentsInChildren<Collector>();

        for (int i = 0; i < ownCollector.Length; i++)
            if (collector == ownCollector[i])
                return true;

        return false;
    }

    private Crystal TakeCrystal()
    {
        Crystal crystal = null;

        if (_freeCrystals == null)
            return null;

        if (_freeCrystals.Count > 0)
            crystal = _freeCrystals.FirstOrDefault(crystal => crystal);

        return crystal;
    }

    private IEnumerator TaskAcquisition()
    {
        bool isWork = true;

        _delay = new WaitForSeconds(_interval);

        while (isWork)
        {
            //Collector collector = GetFreeCollector();
            //Crystal crystal = TakeCrystal();

                SetPriorityTask();
            //if (collector != null && crystal != null)
            //{
                //_freeCrystals.Remove(crystal);
                //_occupiedCrystals.Add(crystal);
            //}

            //Flag flag = _flag;

            //if (GetFreeCollector() != null)
            //{
            //}
            //SetPriorityTask(crystal, flag);

            //if (flag == null && crystal != null)

            yield return _delay;
        }
    }
}
