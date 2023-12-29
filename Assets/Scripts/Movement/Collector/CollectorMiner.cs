using UnityEngine;
using UnityEngine.Events;

public class CollectorMiner : MonoBehaviour
{
    public event UnityAction CrystalMined;

    [SerializeField] private CollectorMover _botMover;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Crystal>(out Crystal crystal))
        {
            if (crystal == _botMover.Target.GetComponent<Crystal>())
            {
                CrystalMined?.Invoke();
                crystal.transform.SetParent(transform);
                Destroy(crystal.GetComponent<BoxCollider>());
            }
        }
    }
}
