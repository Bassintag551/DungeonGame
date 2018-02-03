using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapColliderGenerator : MonoBehaviour
{
    private Tilemap _tilemap;
    private Vector3Int _min;
    private bool[,] _assigned;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        _min = bounds.min;
        Vector3Int pos = _min;
        _assigned = new bool[bounds.size.x, bounds.size.y];
        for (; pos.x < bounds.max.x; pos.x += 1)
        {
            for (pos.y = bounds.min.y; pos.y < bounds.max.y; pos.y += 1)
            {
                Vector3Int rel = pos - bounds.min;
                if (_assigned[rel.x, rel.y] || !_tilemap.HasTile(pos)) continue;
                Vector2Int size = RectAt(pos);
                AddCollider(pos, size);
                for (int x = 0; x < size.x; x += 1)
                {
                    for (int y = 0; y <= size.y; y += 1)
                    {
                        _assigned[x + rel.x, y + rel.y] = true;                        
                    }
                }
            }
        }
    }

    private void AddCollider(Vector3Int pos, Vector2Int size)
    {
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.size = size + new Vector2Int(0, 1);
        boxCollider.offset = new Vector2Int(pos.x, pos.y) + boxCollider.size / 2;
    }

    private bool CheckRow(Vector3Int pos, int endX, int y)
    {
        pos.y = y;
        for (; pos.x < endX; pos.x += 1)
            if (!_tilemap.HasTile(pos) || _assigned[pos.x - _min.x, pos.y - _min.y])
                return false;
        return true;
    }

    private Vector2Int RectAt(Vector3Int pos)
    {
        Vector3Int start = pos;
        while (_tilemap.HasTile(pos))
            pos.x += 1;
        while (CheckRow(start, pos.x, pos.y + 1))
            pos.y += 1;
        Vector3Int size = pos - start;
        return new Vector2Int(size.x, size.y);
    }
}