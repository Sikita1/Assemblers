using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private int _crystals;
    [SerializeField] private CrystalFactory _crystalFactory;

    [SerializeField] private Transform _container;

    [SerializeField] private Transform _rightZone;
    [SerializeField] private Transform _leftZone;
    [SerializeField] private Transform _upZone;
    [SerializeField] private Transform _downZone;

    public event UnityAction<Crystal> Spawned;

    private WaitForSeconds _delay;

    private int _currentAmount = 1;
    private float _fullTurn = 360f;

    private float _interval = 0f;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    public Transform GetContainer() => _container;

    private IEnumerator Spawn()
    {
        _delay = new WaitForSeconds(_interval);

        while (_currentAmount <= _crystals)
        {
            Crystal crystal = CreateCrystal();

            _currentAmount++;

            Spawned?.Invoke(crystal);

            yield return _delay;
        }
    }

    private Crystal CreateCrystal()
    {
        Quaternion randomQuaternion = Quaternion.Euler(0, Random.Range(0, _fullTurn), 0);

        float positionX = Random.Range(_leftZone.position.x, _rightZone.position.x);
        float positionZ = Random.Range(_upZone.position.z, _downZone.position.z);

        Vector3 position = new Vector3(positionX, 0, positionZ);

        return _crystalFactory.Create(position, randomQuaternion, _container);
    }
}
