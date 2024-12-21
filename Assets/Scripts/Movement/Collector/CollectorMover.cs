using UnityEngine;

public class CollectorMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector3 _targetPosition = Vector3.zero;

    public Transform Target { get; private set; }

    private void Update()
    {
        if (Target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position,
                                                 _targetPosition,
                                                 _speed * Time.deltaTime);

        Vector3 rotationTarget = _targetPosition - transform.position;

        if (rotationTarget.sqrMagnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.LookRotation(rotationTarget);
    }

    public void SetTarget(Transform target)
    {
        Target = target;

        if (Target == null)
            return;

        _targetPosition = Target.position;
        _targetPosition.y = transform.position.y;
    }
}
