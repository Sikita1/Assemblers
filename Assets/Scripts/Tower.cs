using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Tower : MonoBehaviour
{
    [Header("CollectorFactory")]
    [SerializeField] private int _collectorsCount;
    [SerializeField] private CollectorFactory _collectorFactory;
    
    [SerializeField] private CrystalSpawner _crystalSpawner;

    public event UnityAction OnFreeCollector;

    private List<Crystal> _crystals = new List<Crystal>();
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
        _crystalSpawner.Spawned += OnCrystalSpawned;
    }

    private void OnDisable()
    {
        _crystalSpawner.Spawned -= OnCrystalSpawned;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Collector>(out Collector collector))
            if (collector.IsWork)
                collector.OnFinishTask();
    }

    public void SetTask()
    {
        GetFreeCollector().SetCrystal(TakeCrystal());
    }

    private void OnCrystalSpawned(Crystal crystal)
    {
        _crystals.Add(crystal);
        crystal.Release();
    }

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

        WaitForSeconds wait = new WaitForSeconds(1f);

        while (isWork)
        {
            if (GetFreeCollector() != null && TakeCrystal() != null)
            {
                SetTask();
                TakeCrystal().Occupy();
                _crystals.Remove(TakeCrystal());
            }
        Debug.Log(_crystals.Count);

            yield return wait;
        }
    }
}
