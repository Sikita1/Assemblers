using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CollectorBuilder : MonoBehaviour
{
    public event UnityAction<Flag> BuildTower;

    [SerializeField] private CollectorMover _botMover;
    //[SerializeField] private Slider _slider;

    private WaitForSeconds _waitForSeconds;
    private float _delay = 1f;
    private float _currentTime;
    private float _timeBuilder = 5f;

    private Coroutine _coroutine;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Flag flag))
            if (flag.IsBusy)
                if (flag == _botMover.Target.GetComponent<Flag>())
                {
                    if (_coroutine != null)
                        StopCoroutine(Construction(flag));

                    _coroutine = StartCoroutine(Construction(flag));
                }
    }

    private IEnumerator Construction(Flag flag)
    {
        _currentTime = 0;

        while (_currentTime != _timeBuilder)
        {
            _currentTime++;
            //_slider.value = Mathf.MoveTowards(_slider.value, 10f, _delay);
            yield return _waitForSeconds;
        }

        yield return _timeBuilder;
        BuildTower?.Invoke(flag);
        flag.Release();
        Destroy(flag.GetComponentInParent<Building>().gameObject);
    }
}
