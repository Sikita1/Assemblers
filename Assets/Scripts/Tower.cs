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

    [SerializeField] private Scanner _scanner;

    public Vector3 PointSpawn { get; private set; }

    private List<Crystal> _crystals;
    private List<Collector> _collectors = new List<Collector>();

    private Flag _flag;

    private WaitForSeconds _delay;

    private float _interval = 0.1f;

    private float _minRandomPositionX = 20f;
    private float _maxRandomPositionX = 15f;
    private float _positionY = 3f;
    private float _minRandomPositionZ = 15;
    private float _maxRandomPositionZ = 20f;

    private Coroutine _corutine;

    public event UnityAction Delivery;

    private void Awake()
    {
        CreateCollectors();
    }

    private void Start()
    {
        _corutine = StartCoroutine(TaskAcquisition());
    }

    private void OnEnable()
    {
        _scanner.Scanned += OnScanned;
        _scanner.ScannedFlag += OnScannedFlag;
    }

    private void OnDisable()
    {
        _scanner.Scanned -= OnScanned;
        _scanner.ScannedFlag -= OnScannedFlag;
    }

    public bool CollectorsEnoughToBuild() =>
        _collectors.Count > 1;

    public float GetPositionHight() =>
        _positionY;

    public Transform GetContainer() =>
        _collectorFactory.GetContainer();

    private void OnScannedFlag(Flag flag)
    {
        flag = _scanner.GetFlag();
        _flag = flag;
    }

    private void OnScanned(List<Crystal> crystals)
    {
        _crystals = new List<Crystal>();
        crystals = _scanner.GetCrystals();
        _crystals = crystals;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Collector collector))
        {
            if (FindYourDebtCollector(collector))
            {
                if (collector.IsWork())
                {
                    collector.FinishTask();
                    Delivery?.Invoke();
                }
            }
        }
    }

    private void SetTask(Crystal crystal, Flag flag)
    {
        GetFreeCollector().SetPrioritizeTask(crystal, flag);
    }

    private void CreateCollectors()
    {
        Collector collector;

        for (int i = 0; i < _collectorsCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(gameObject.transform.position.x + _minRandomPositionX,
                                                        gameObject.transform.position.x + _maxRandomPositionX),
                                           _positionY,
                                           Random.Range(gameObject.transform.position.z + _minRandomPositionZ,
                                                        gameObject.transform.position.z + _maxRandomPositionZ));

            PointSpawn = position;
            collector = _collectorFactory.Create(position, this);
            _collectors.Add(collector);
        }
    }

    private Collector GetFreeCollector() =>
        _collectors.FirstOrDefault(collector => collector.IsWork() == false);

    private bool FindYourDebtCollector(Collector collector)
    {
        for (int i = 0; i < _collectors.Count; i++)
            if (collector == _collectors[i])
                return true;

        return false;
    }

    private Crystal TakeCrystal()
    {
        Crystal crystal = null;

        if (_crystals == null)
            return null;

        if (_crystals.Count > 0)
            crystal = _crystals.FirstOrDefault(crystal => crystal);

        return crystal;
    }

    private IEnumerator TaskAcquisition()
    {
        bool isWork = true;

        _delay = new WaitForSeconds(_interval);

        while (isWork)
        {
            Crystal crystal = TakeCrystal();
            Flag flag = _flag;

            if (GetFreeCollector() != null)
            {
                SetTask(crystal, flag);

                if (flag == null && crystal != null)
                    _crystals.Remove(crystal);
            }

            yield return _delay;
        }
    }
}
