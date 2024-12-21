using UnityEngine;

public class Crystal : MonoBehaviour
{
    public bool IsBusy { get; private set; } = false;

    public void Occupy() =>
        IsBusy = true;

    public void Release() =>
        IsBusy = false;
}
