using TMPro;
using UnityEngine;

public class ViewCountCrystal : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Tower _tower;

    private int _crystals = 0;

    private void OnEnable()
    {
        _tower.Delivery += OnCrystalDelivered;
    }

    private void OnDisable()
    {
        _tower.Delivery -= OnCrystalDelivered;
    }

    private void OnCrystalDelivered()
    {
        _crystals++;

        DrowCountCrystals();
    }

    private void DrowCountCrystals()
    {
        _text.text = $"Кристаллы: {_crystals}";
    }
}
