using UnityEngine;

public class ContainerTower : MonoBehaviour
{
    [SerializeField] private BuildingGrid _buildingGrid;

    private void Awake()
    {
        IntroduceEarth();
    }

    public void IntroduceEarth()
    {
        Building[] towers = gameObject.GetComponentsInChildren<Building>();

        if (towers != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                CreatorNewBase newBase = towers[i].GetComponentInChildren<CreatorNewBase>();

                if (newBase.GetBuildindGrid() == null)
                    newBase.SetBuildingGrid(_buildingGrid);
            }
        }
    }
}
