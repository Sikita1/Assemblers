using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Renderer _mainRenderer;
    [SerializeField] private Vector2Int _size = Vector2Int.one;

    public Vector2Int GetSize =>
        _size;

    public void SetTransperent(bool available)
    {
        Color color = _mainRenderer.material.color;
        if (available)
        {
            _mainRenderer.material.color = Color.green;
            color.a = .3f;
        }
        else
        {
            _mainRenderer.material.color = Color.red;
            color.a = .3f;
        }

    }

    public void SetNormalColor()
    {
        _mainRenderer.material.color = Color.white;
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
