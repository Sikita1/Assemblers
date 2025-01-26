using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TemporaryCrystalVault : MonoBehaviour
{
    private readonly List<Crystal> _crystals = new();

    public void OccupiedCrystal(Crystal crystal) =>
        _crystals.Add(crystal);

    public IEnumerable<Crystal> GetFreeCrystal(IEnumerable<Crystal> crystals) =>
        crystals.Where(crystal => crystals.Contains(crystal) == false);

    public void RemoveOccupiedCrystal(Crystal crystal) =>
        _crystals.Remove(crystal);
}
