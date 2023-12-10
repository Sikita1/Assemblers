using UnityEngine;

public class CrystalFactory : MonoBehaviour
{
    [SerializeField] private Crystal _crystal;

    public Crystal Create(Vector3 position, Quaternion quaternion, Transform container)
    {
        Crystal crystal = Instantiate(_crystal, position, quaternion, container);

        return crystal;
    }
}
