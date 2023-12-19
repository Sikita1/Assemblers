using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tower : MonoBehaviour
{
    [Header("CollectorFactory")]
    [SerializeField] private int _collectorsCount;
    [SerializeField] private CollectorFactory _collectorFactory;

    [SerializeField] private Scanner _scanner;
    //[SerializeField] private CrystalSpawner _crystalSpawner;

    private List<Crystal> _crystals;
    private List<Collector> _collectors = new List<Collector>();

    public Vector3 PointSpawn { get; private set; }

    private Coroutine _corutine;

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
        _scanner.Scanned += OnGetList;
        //_crystalSpawner.Spawned += OnCrystalSpawned;
    }

    private void OnDisable()
    {
        _scanner.Scanned -= OnGetList;
        //_crystalSpawner.Spawned -= OnCrystalSpawned;
    }

    //private void Update()
    //{
    //    Debug.Log(_crystals.Count);
    //}

    private void OnGetList(List<Crystal> crystals)
    {
        _crystals = new List<Crystal>();
        crystals = _scanner.GetCrystals();
        _crystals = crystals;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Collector>(out Collector collector))
            if (collector.IsWork)
                collector.OnFinishTask();
    }

    private void SetTask(Crystal crystal)
    {
        GetFreeCollector().SetCrystal(crystal);
    }

    //private void OnCrystalSpawned(Crystal crystal)
    //{
    //    _crystals.Add(crystal);
    //}

    private void CreateCollectors()
    {
        Collector collector;

        for (int i = 0; i < _collectorsCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(gameObject.transform.position.x - 20,
                                                        gameObject.transform.position.x - 15),
                                          3,
                                          Random.Range(gameObject.transform.position.z + 15,
                                                       gameObject.transform.position.z + 20));

            PointSpawn = position;

            collector = _collectorFactory.Create(position, this);

            _collectors.Add(collector);
        }
    }

    private Collector GetFreeCollector() =>
        _collectors.FirstOrDefault(collector => collector.IsWork == false);

    public Crystal TakeCrystal()
    {
        Crystal crystal = null;

        if (_crystals.Count > 0)
            crystal = _crystals.FirstOrDefault(crystal => crystal.IsBusy == false);

        return crystal;
    }

    private IEnumerator TaskAcquisition()
    {
        bool isWork = true;

        WaitForSeconds wait = new WaitForSeconds(0f);

        while (isWork)
        {
            Crystal crystal = TakeCrystal();

            if (GetFreeCollector() != null && crystal != null)
            {
                SetTask(crystal);
                crystal.Occupy();
                _crystals.Remove(crystal);
            }

            yield return wait;
        }
    }
}
