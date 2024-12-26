using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Scanner : MonoBehaviour
{
    private const string LayerCrystal = "Crystal";
    private const string LayerFlag = "Flag";

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _radius;
    [SerializeField] private Transform _transform;

    public event UnityAction<List<Crystal>> Scanned;
    public event UnityAction<Flag> ScannedFlag;

    private Collider[] _allCrystals;
    private Collider[] _allFlags;
    private List<Crystal> _crystals = new List<Crystal>();
    private WaitForSeconds _waitForSeconds;
    private float _delay = 3f;

    private Flag _flag;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_delay);
    }

    private void Start()
    {
        StartCoroutine(Scan());
    }

    public List<Crystal> GetCrystals() =>
        _crystals;

    public Flag GetFlag() =>
        _flag;

    private IEnumerator Scan()
    {
        while (enabled)
        {
            _allCrystals = ResultScan(LayerCrystal);
            _allFlags = ResultScan(LayerFlag);

            for (int i = 0; i < _allCrystals.Length; i++)
                if (_allCrystals[i].gameObject.TryGetComponent(out Crystal crystal))
                    _crystals.Add(crystal);

            Scanned?.Invoke(_crystals);

            for (int i = 0; i < _allFlags.Length; i++)
                if (_allFlags[i].gameObject.TryGetComponent(out Flag flag))
                    if(flag.AvailableForScanning)
                        _flag = flag;

            ScannedFlag?.Invoke(_flag);

            yield return _waitForSeconds;
        }
    }

    private Collider[] ResultScan(string layerMask) =>
        Physics.OverlapSphere(_transform.position,
                              _radius,
                              LayerMask.GetMask(layerMask));
}
