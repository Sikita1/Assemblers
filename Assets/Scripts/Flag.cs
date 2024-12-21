using UnityEngine;

public class Flag : MonoBehaviour
{
    public bool IsBusy { get; private set; }
    public bool AvailableForScanning { get; private set; }

    public void Occupy() =>
        IsBusy = true;

    public void Release() =>
        IsBusy = false;

    public bool OpenForScanning() =>
        AvailableForScanning = true;

    public bool CloseForScanning() =>
        AvailableForScanning = false;
}
