using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderView : MonoBehaviour
{
    private Slider _slider;

    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    private float _delay = 0.2f;
    private float _maxPercentBuilder = 100f;
    private float _minPercentBuilder = 0f;

    public float MaxDelta { get; private set; } = 5f;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
        _slider = GetComponent<Slider>();
    }

    public void Reset()
    {
        _slider.value = _minPercentBuilder;
    }

    public void PercentChange()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SliderChange(_maxPercentBuilder));
    }

    private IEnumerator SliderChange(float maxPercentBuilding)
    {
        while (_slider.value != maxPercentBuilding)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, maxPercentBuilding, MaxDelta);
            yield return _wait;
        }
    }
}
