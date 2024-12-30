using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Update()
    {
        transform.eulerAngles = Camera.main.transform.eulerAngles;
    }
}
