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

    public event UnityAction Delivery;

    private List<Crystal> _crystals;
    private List<Collector> _collectors = new List<Collector>();

    private WaitForSeconds _delay;

    private float _interval = 0.1f;

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
    }

    private void OnDisable()
    {
        _scanner.Scanned -= OnGetList;
    }

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
            {
                collector.OnFinishTask();
                Delivery?.Invoke();
            }
    }

    private void SetTask(Crystal crystal)
    {
        GetFreeCollector().SetCrystal(crystal);
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

            if (GetFreeCollector() != null && crystal != null)
            {
                SetTask(crystal);
                _crystals.Remove(crystal);
            }

            yield return _delay;
        }
    }
}
