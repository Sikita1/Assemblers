using UnityEngine;
using UnityEngine.Events;

public class CollectorBuilder : MonoBehaviour
{
    public event UnityAction<Flag> BuildTower;

    [SerializeField] private CollectorMover _botMover;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Flag flag))
        {
            if (flag == _botMover.Target.GetComponent<Flag>())
            {
                BuildTower?.Invoke(flag);
                flag.Release();
                Destroy(flag.GetComponent<BoxCollider>().gameObject);
            }
        }
    }
}
