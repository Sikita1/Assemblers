using UnityEngine;

public class CreatorNewBase : MonoBehaviour
{
    [SerializeField] private BuildingGrid _buildingGrid;
    [SerializeField] private Building _prefabFlag;

    private void OnMouseUp()
    {
        Tower tower = gameObject.GetComponent<Tower>();

        if (tower.CollectorsEnoughToBuild())
        {
            if (_buildingGrid.PreviousFlag == null)
            {
                _buildingGrid.StartPlacingBuiding(_prefabFlag, tower);
            }
            else if (_buildingGrid.PreviousFlag != null && _buildingGrid.PreviousFlag.IsBusy == false
                                                        && _buildingGrid.PreviousFlag.AvailableForScanning)
            {
                _buildingGrid.StartPlacingBuiding(_prefabFlag, tower);
            }
        }
    }
}
