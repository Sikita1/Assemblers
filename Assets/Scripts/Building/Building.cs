using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Renderer _mainRenderer;
    [SerializeField] private Vector2Int _size = Vector2Int.one;

    private float _colorAlfa = 0.3f;
    private Color _color;

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
        //Vector3 vector = transform.position + new Vector3(_size.x / 2, 0, _size.y / 2);

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Gizmos.color = new Color(0f, 1f, 0f, .3f);
                //Gizmos.DrawCube(vector + new Vector3(x, 0, y), new Vector3(1, 1f, 1));
                Gizmos.DrawCube(transform.position, new Vector3(_size.x, 1f, _size.y));
            }
        }

    }
}
