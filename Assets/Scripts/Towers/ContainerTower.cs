using UnityEngine;

public class ContainerTower : MonoBehaviour
{
    [SerializeField] private BuildingGrid _buildingGrid;

    public BuildingGrid GetBuildingGrid() =>
        _buildingGrid;
}
