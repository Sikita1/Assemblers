using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Renderer _mainRenderer;
    [SerializeField] private Vector2Int _size = Vector2Int.one;

    private float _colorAlfa = 0.3f;
    private Color _color;

    public bool IsCreated { get; private set; }

    private void Awake()
    {
        _color = _mainRenderer.material.color;
    }

    public Vector2Int GetSize =>
        _size;

    public void SetTransperent(bool available)
    {
        if (available)
            _color = Color.green;
        else
            _color = Color.red;

        _color.a = _colorAlfa;
        _mainRenderer.material.color = _color;
    }

    public void SetNormalColor()
    {
        _color.a = 0f;
        _mainRenderer.material.color = _color;
    }

    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Gizmos.color = new Color(0f, 1f, 0f, .3f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 3.3f, 1));
            }
        }
    }
}
