using System.Collections.Generic;
using DefaultNamespace;
using Entities;
using JetBrains.Annotations;
using SuperTiled2Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    private Tilemap[] _tilemap;
    private SuperObjectLayer _entities;
    public Vector3 cellSize;
    
    private Dictionary<Vector2Int, Cell> cells = new ();

    private void Awake()
    {
        _tilemap = GetComponentsInChildren<Tilemap>();
        _entities = GetComponentInChildren<SuperObjectLayer>();
        cellSize = _tilemap[0].cellSize;
    }
    
    public void CreateCells(GameObject cellContainer)
    {
        for (var z = _tilemap.Length - 1; z >= 0; z--)
        {
            var tilemap = _tilemap[z];
            var mapBounds = tilemap.cellBounds;
            var trueZ = 1 - (_tilemap.Length - z);
            tilemap.GetComponent<TilemapRenderer>().sortingOrder = trueZ;

            for (var x = mapBounds.min.x; x < mapBounds.max.x; x++)
            {
                for (var y = mapBounds.min.y; y < mapBounds.max.y; y++)
                {
                    if (tilemap.HasTile(new Vector3Int(x, y, 0)) &&
                        tilemap.GetTile(new Vector3Int(x, y, 0)) is SuperTile tile)
                    {
                        var walkable = tile.GetPropertyValueAsBool("walkable", false);
                        if (!cells.ContainsKey(new Vector2Int(x, y)))
                        {
                            var cell = Instantiate(cellPrefab, cellContainer.transform);
                            var position = tilemap.CellToWorld(new Vector3Int(x, y, z));
                            cell.Walkable = walkable;
                            cell.transform.position = new Vector3(
                                position.x + cellSize.x / 2,
                                position.y + cellSize.y, 
                                position.z
                            );
                            cell.tilePosition = new Vector3Int(x, y, trueZ);
                            cell.gameObject.name = $"Cell ({x},{y})";
                            cell.GetComponent<SpriteRenderer>().sortingOrder = trueZ;
                            cells.Add(new Vector2Int(x, y), cell);
                        }
                    }
                }
            }
        }
    }

    public void CreateEntities()
    {
        foreach (var entity in _entities.GetComponentsInChildren<GameEntity>())
        {
            var entityPosition = entity.transform.position;
            var cell = GetCellFromWorld(entityPosition);
            if (cell != null)
            {
                entity.posX = cell.tilePosition.x;
                entity.posY = cell.tilePosition.y;
                entity.gameObject.transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y + cellSize.y/2, cell.transform.position.z + 1);
                cell.SetEntity(entity);
            }
        }
    }

    [CanBeNull]
    public Cell GetCellFromWorld(Vector3 mousePosition)
    {
        for (var z = _tilemap.Length - 1; z >= 0; z--)
        {
            var tilemap = _tilemap[z];
            var tilePos = tilemap.WorldToCell(new Vector3(mousePosition.x - cellSize.x/2, mousePosition.y - cellSize.y, 0));
            return cells.ContainsKey(new Vector2Int(tilePos.x, tilePos.y)) ? cells[new Vector2Int(tilePos.x, tilePos.y)] : null;
        }
        
        return null;
    }
    
    [CanBeNull] public Cell GetCellFromPos(Vector2Int pos) => cells.ContainsKey(pos) ? cells[pos] : null;
}
