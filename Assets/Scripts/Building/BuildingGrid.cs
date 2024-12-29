using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    public Vector2Int GridSize = new Vector2Int(10, 10);

    private Building[,] _grid;
    private Building _flyingBuilding;
    private Camera _mainCamera;

    public Flag PreviousFlag { get; private set; }

    private void Awake()
    {
        _grid = new Building[GridSize.x, GridSize.y];
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - _flyingBuilding.GetSize.x)
                    available = false;
                if (y < 0 || y > GridSize.y - _flyingBuilding.GetSize.y)
                    available = false;

                if (available && IsPlaceTaken(x, y))
                    available = false;

                _flyingBuilding.transform.position = new Vector3(x, .1f, y);
                _flyingBuilding.SetTransperent(available);

                if (available && Input.GetMouseButtonDown(0))
                    if (_flag != null)
                    {
                        PlaceFlyingBiulding(x, y);
                        PreviousFlag = _flag;
                    }
            }
        }
    }

    public void StartPlacingBuiding(Building buildingPrefab)
    {
        if (_flyingBuilding != null)
            Destroy(_flyingBuilding.gameObject);

        _flyingBuilding = Instantiate(buildingPrefab);
        _flag = _flyingBuilding.GetComponentInChildren<Flag>();

        _flag.SetTower(_flyingBuilding.GetComponent<Tower>());
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingBuilding.GetSize.x; x++)
            for (int y = 0; y < _flyingBuilding.GetSize.y; y++)
                if (_grid[placeX + x, placeY + y] != null)
                    return true;

        return false;
    }

    private void EstablishBuildingSite(int placeX, int placeY, int x, int y)
    {
        _grid[placeX + x, placeY + y] = _flyingBuilding;
    }

    private void PlaceFlyingBiulding(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingBuilding.GetSize.x; x++)
        {
            for (int y = 0; y < _flyingBuilding.GetSize.y; y++)
            {
                if (PreviousFlag != null && PreviousFlag.IsBusy == false
                                          && PreviousFlag.AvailableForScanning)
                {
                    EstablishBuildingSite(placeX, placeY, x, y);
                    Destroy(PreviousFlag.GetComponentInParent<Building>().gameObject);
                }
                else
                {
                    EstablishBuildingSite(placeX, placeY, x, y);
                }
            }
        }

        if (_flag != null)
            _flag.OpenForScanning();

        _flyingBuilding.SetNormalColor();
        _flyingBuilding = null;
    }
}
