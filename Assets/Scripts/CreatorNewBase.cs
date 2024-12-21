using UnityEngine;

public class CreatorNewBase : MonoBehaviour
{
    [SerializeField] private BuildingGrid _buildingGrid;
    [SerializeField] private Building _prefabFlag;

    private void OnMouseUp()
    {
        _buildingGrid.StartPlacingBuiding(_prefabFlag);
    }
}
