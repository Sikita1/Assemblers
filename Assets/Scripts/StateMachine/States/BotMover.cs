using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _target;

    private void Update()
    {
        if (_target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(_target.transform.position - transform.position);
    }

    public void SetTarget(Transform target) => _target = target;
}
