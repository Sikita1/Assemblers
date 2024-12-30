using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] private Building _prefabTower;

    public Tower Create(Vector3 position)
    {
        Building tower = Instantiate(_prefabTower,
                                  position,
                                  Quaternion.identity);

        return tower.GetComponentInChildren<Tower>();
    }
}
