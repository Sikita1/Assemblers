using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] private Building _prefabTower;
    //[SerializeField] private Transform _containerNewBase;

    public Tower Create(Vector3 position, Transform container)
    {
        Building tower = Instantiate(_prefabTower,
                                     position,
                                     Quaternion.identity,
                                     container);

        return tower.GetComponentInChildren<Tower>();
    }
}
