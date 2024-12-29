using UnityEngine;

public class CreatorNewBase : MonoBehaviour
{
    [SerializeField] private BuildingGrid _buildingGrid;
    [SerializeField] private Building _prefabFlag;

    private void OnMouseUp()
    {
        if (gameObject.GetComponent<Tower>().CollectorsEnoughToBuild())
        {
            if (_buildingGrid.PreviousFlag == null)
            {
                _buildingGrid.StartPlacingBuiding(_prefabFlag);
            }
            else if (_buildingGrid.PreviousFlag != null && _buildingGrid.PreviousFlag.IsBusy == false
                && _buildingGrid.PreviousFlag.AvailableForScanning)
            {
                _buildingGrid.StartPlacingBuiding(_prefabFlag);
            }
        }
    }
}
