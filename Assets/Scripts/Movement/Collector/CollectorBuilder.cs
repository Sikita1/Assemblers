using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CollectorMover))]
public class CollectorBuilder : MonoBehaviour
{
    [SerializeField] private SliderBuilder _slider;

    private CollectorMover _botMover;
    private WaitForSeconds _wait;

    private float _delay = 1f;
    private float _currentTime;

    private Coroutine _coroutine;

    public event UnityAction<Flag> BuildTower;

    public float TimeBuilder { get; private set; } = 5f;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
        _botMover = GetComponent<CollectorMover>();
    }

    private void Start()
    {
        _slider.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Flag flag))
            if (flag.IsBusy())
                if (_botMover.Target != null)
                    if (flag == _botMover.Target.GetComponent<Flag>())
                    {
                        if (_coroutine != null)
                            StopCoroutine(Construction(flag));

                        _coroutine = StartCoroutine(Construction(flag));
                    }
    }

    private IEnumerator Construction(Flag flag)
    {
        _slider.gameObject.SetActive(true);
        _currentTime = 0;

        flag.Hide();

        _slider.Reset();
        _slider.PercentChange();

        while (_currentTime != TimeBuilder)
        {
            yield return _wait;
            _currentTime++;
        }

        yield return _slider.MaxDelta;
        BuildTower?.Invoke(flag);
        flag.Release();
        Destroy(flag.GetComponentInParent<Building>().gameObject);
        _slider.gameObject.SetActive(false);
    }
}
