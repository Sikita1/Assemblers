using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Color _colorAlfa;

    public bool IsBusy { get; private set; }
    public bool AvailableForScanning { get; private set; }
    public Tower Tower { get; private set; }

    public void SetTower(Tower tower)
        => Tower = tower;

    public void Occupy() =>
        IsBusy = true;

    public void Release() =>
        IsBusy = false;

    public bool OpenForScanning() =>
        AvailableForScanning = true;

    public bool CloseForScanning() =>
        AvailableForScanning = false;

    public void Hide()
    {
        ChangeColorRecursive(this.gameObject);
    }

    private void ChangeColorRecursive(GameObject flag)
    {
        Renderer renderer = flag.GetComponent<Renderer>();

        if(renderer != null && renderer.material != null)
        {
            _colorAlfa.a = 0f;
            renderer.material.color = _colorAlfa;
        }

        foreach (Transform child in flag.transform)
            ChangeColorRecursive(child.gameObject);
    }
}
