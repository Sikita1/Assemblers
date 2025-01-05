using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CollectorMover))]
public class CollectorMiner : MonoBehaviour
{
    public event UnityAction CrystalMined;

    private CollectorMover _botMover;

    private void Awake()
    {
        _botMover = GetComponent<CollectorMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Crystal crystal))
        {
            if (crystal == null)
                return;

            if (_botMover != null)
            {
                if (_botMover.Target != null)
                    if (crystal == _botMover.Target.GetComponent<Crystal>())
                    {
                        crystal.transform.SetParent(transform);
                        Destroy(crystal.GetComponent<BoxCollider>());
                        CrystalMined?.Invoke();
                    }
            }
        }
    }
}
