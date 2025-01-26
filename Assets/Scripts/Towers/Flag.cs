using UnityEngine;

public class Flag : MonoBehaviour
{
    private Color _colorAlfa;

    [SerializeField] private bool _isBusy;
    [SerializeField] private bool _availableForScanning;
    [SerializeField] private Tower _tower;

    public bool IsBusy() => _isBusy;
    public bool AvailableForScanning() => _availableForScanning;
    public Tower Tower => _tower;

    public void SetTower(Tower tower)
        => _tower = tower;

    public void Occupy() =>
        _isBusy = true;

    public void Release() =>
        _isBusy = false;

    public bool OpenForScanning() =>
        _availableForScanning = true;

    public bool CloseForScanning() =>
        _availableForScanning = false;

    public void Hide()
    {
        gameObject.SetActive(false);
        //ChangeColorRecursive(this.gameObject);
    }

    private void ChangeColorRecursive(GameObject flag)
    {
        //Renderer renderer = flag.GetComponent<Renderer>();

        //if(renderer != null && renderer.material != null)
        //{
        //    _colorAlfa.a = 0f;
        //    renderer.material.color = _colorAlfa;
        //}

        //foreach (Transform child in flag.transform)
        //    ChangeColorRecursive(child.gameObject);
        flag.SetActive(false);
    }
}
