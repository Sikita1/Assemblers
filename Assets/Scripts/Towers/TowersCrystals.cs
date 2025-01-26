using TMPro;
using UnityEngine;

public class TowersCrystals : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Tower _tower;

    public int Amount { get; private set; } = 0;

    private void Start()
    {
        DrowCountCrystals();
    }

    private void OnEnable()
    {
        _tower.Delivery += OnDelivered;
    }

    private void OnDisable()
    {
        _tower.Delivery -= OnDelivered;
    }

    private void OnDelivered()
    {
        Amount++;

        DrowCountCrystals();
    }

    public void WriteOffCrystals(int count)
    {
        if (count < 0)
            count = 0;

        Amount -= count;
        
        DrowCountCrystals();
    }

    private void DrowCountCrystals() =>
        _text.text = $"Кристаллы: {Amount}";
}
