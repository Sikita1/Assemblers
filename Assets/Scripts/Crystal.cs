using UnityEngine;

public class Crystal : MonoBehaviour
{
    public bool IsBusy { get; private set; }

    public bool Release() => IsBusy = false;

    public bool Occupy() => IsBusy = true;
}
