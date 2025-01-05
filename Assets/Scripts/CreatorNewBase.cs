using UnityEngine;

[RequireComponent(typeof(Tower))]
public class CreatorNewBase : MonoBehaviour
{
    [SerializeField] private Building _prefabFlag;
    [SerializeField] private BuildingGrid _buildingGrid;

    private void OnMouseUp()
    {
        Tower tower = gameObject.GetComponent<Tower>();

        if (tower.CollectorsEnoughToBuild())
        {
            if (_buildingGrid.PreviousFlag == null)
            {
                _buildingGrid.StartPlacingBuiding(_prefabFlag, tower);
            }
            else if (_buildingGrid.PreviousFlag != null && _buildingGrid.PreviousFlag.IsBusy() == false
                                                        && _buildingGrid.PreviousFlag.AvailableForScanning())
            {
                _buildingGrid.StartPlacingBuiding(_prefabFlag, tower);
            }
        }
    }

    public BuildingGrid GetBuildindGrid() =>
        _buildingGrid;

    public void SetBuildingGrid(BuildingGrid grid) =>
        _buildingGrid = grid;
}
