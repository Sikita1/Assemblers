using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] private GameObject _prefabTower;

    public Tower Create(Vector3 position)
    {
        Tower tower = Instantiate(_prefabTower.GetComponentInChildren<Tower>(),
                                  position,
                                  Quaternion.identity);

        return tower;
    }
}
