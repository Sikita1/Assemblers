using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    private Flag _flag;

    public Vector2Int GridSize = new Vector2Int(10, 10);

    private Building[,] _grid;
    private Building _flyingBuilding;
    private Camera _mainCamera;

    private CreatorNewBase _creatorNewBase;

    private float _flyingBuildingPositionZ = 1.1f;
    private float _halfFlagAreaX =>
        _flyingBuilding.GetSize.x / 2;
    private float _halfFlagAreaY =>
        _flyingBuilding.GetSize.y / 2;

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

                if (x < _halfFlagAreaX || x > GridSize.x - _halfFlagAreaX)
                    available = false;
                if (y < _halfFlagAreaY || y > GridSize.y - _halfFlagAreaY)
                    available = false;

                if (available && IsPlaceTaken(x, y))
                    available = false;

                if (PreviousFlag != null && PreviousFlag.IsBusy()
                                         && PreviousFlag.AvailableForScanning() == false)
                    DeleteAnExistingFlag(_flyingBuilding.GetComponentInChildren<Flag>());
                
                _flyingBuilding.transform.position = new Vector3(x, _flyingBuildingPositionZ, y);
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

    public void StartPlacingBuiding(Building buildingPrefab, Tower tower)
    {
        if (_flyingBuilding != null)
            Destroy(_flyingBuilding.gameObject);

        _flyingBuilding = Instantiate(buildingPrefab);
        _flag = _flyingBuilding.GetComponentInChildren<Flag>();

        _flag.SetTower(tower);

        _creatorNewBase = _flyingBuilding.GetComponentInChildren<CreatorNewBase>();
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < _halfFlagAreaX; x++)
            for (int y = 0; y < _halfFlagAreaY; y++)
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
        for (int x = 0; x < _halfFlagAreaX; x++)
        {
            for (int y = 0; y < _halfFlagAreaY; y++)
            {
                if (PreviousFlag != null && PreviousFlag.IsBusy() == false
                                         && PreviousFlag.AvailableForScanning())
                {
                    EstablishBuildingSite(placeX, placeY, x, y);
                    DeleteAnExistingFlag(PreviousFlag);
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

    private void DeleteAnExistingFlag(Flag flag) =>
        Destroy(flag.GetComponentInParent<Building>().gameObject);
}
