using UnityEngine;

public class CollectorFactory : MonoBehaviour
{
    [SerializeField] private Collector _prefab;
    [SerializeField] private Transform _container;

    public Collector Create(Vector3 position, Tower tower)
    {
        Collector collector = Instantiate(_prefab, position, Quaternion.identity, _container);
        collector.SetTower(tower);

        return collector;
    }
}
