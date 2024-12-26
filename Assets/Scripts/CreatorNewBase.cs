using UnityEngine;

public class CreatorNewBase : MonoBehaviour
{
    [SerializeField] private BuildingGrid _buildingGrid;
    [SerializeField] private Building _prefabFlag;

    private void OnMouseUp()
    {
        Debug.Log(gameObject.GetComponent<Tower>().CollectorsEnoughToBuild());

        if (gameObject.GetComponent<Tower>().CollectorsEnoughToBuild())
            _buildingGrid.StartPlacingBuiding(_prefabFlag);
    }
}
