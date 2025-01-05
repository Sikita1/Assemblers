using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] private Building _prefabTower;
    [SerializeField] private Transform _containerNewBase;

    public Tower Create(Vector3 position)
    {
        Building tower = Instantiate(_prefabTower,
                                     position,
                                     Quaternion.identity,
                                     _containerNewBase);

        return tower.GetComponentInChildren<Tower>();
    }
}
