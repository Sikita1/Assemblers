using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Scanner : MonoBehaviour
{
    private const string LayerCrystal = "Crystal";

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _radius;
    [SerializeField] private Transform _transform;

    public event UnityAction<List<Crystal>> Scanned;

    private RaycastHit[] _allCrystals;
    private List<Crystal> _crystals = new List<Crystal>();
    private WaitForSeconds _waitForSeconds;
    private float _delay = 3f;

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

    private IEnumerator Scan()
    {
        while (true)
        {
            _allCrystals = Physics.SphereCastAll(_transform.position, _radius, Vector3.one,
                                                 _maxDistance, LayerMask.GetMask(LayerCrystal));

            for (int i = 0; i < _allCrystals.Length; i++)
            {
                if(_allCrystals[i].collider.gameObject.TryGetComponent<Crystal>(out Crystal crystal))
                    _crystals.Add(crystal);
            }

            Scanned?.Invoke(_crystals);

            yield return _waitForSeconds;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_transform.position, _radius);
    }
}
