using UnityEngine;
using UnityEngine.Events;

public class BotMiner : MonoBehaviour
{
    public event UnityAction OnCrystalMined;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Crystal>(out Crystal crystal))
        {
            if (crystal.IsBusy)
            {
                OnCrystalMined?.Invoke();
                crystal.transform.SetParent(transform);
            }
        }
    }
}
