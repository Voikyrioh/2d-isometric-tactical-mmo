using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Utils
{
    public class PathFinder
    {
        private MapManager _map;
        private List<Cell> openList = new ();
        private Dictionary<Cell, Cell> cameFrom = new ();
        private Cell objective;
        public PathFinder(MapManager map)
        {
            _map = map;
        }
        
        public List<Cell> getPath(Cell start, Cell end, bool ignoreWalkability = false)
        {
            openList.Add(start);
            objective = end;
            
            var gScore = new Dictionary<Cell, int>();
            gScore[start] = 0;
            
            var fScore = new Dictionary<Cell, int>();
            fScore[start] = heuristic(start, end);

            while (openList.Count > 0)
            {
                var current = openList.OrderBy(x => fScore[x]).First();
                openList.Remove(current);
                if (current == end) return reconstructPath(end);
                
                foreach (var neighbor in GetNeighbors(current, ignoreWalkability))
                {
                    var newCostToNeighbor = gScore[current] + 1;
                    if (openList.Contains(neighbor) && newCostToNeighbor >= gScore[neighbor]) continue;
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = newCostToNeighbor;
                    fScore[neighbor] = newCostToNeighbor + heuristic(neighbor, end);
                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }   
                
            }
            
            return new List<Cell>();
        }

        private List<Cell> reconstructPath(Cell current)
        {
            var path = new List<Cell>();
            path.Add(current);
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            path.RemoveAt(0);
            return path;
        }
        
        private int heuristic(Cell a, Cell b) => Mathf.Abs(a.tilePosition.x - b.tilePosition.x) + Mathf.Abs(a.tilePosition.y - b.tilePosition.y);

        private List<Cell> GetNeighbors(Cell cell, bool ignoreWalkability)
        {
            var neighbors = new List<Cell>();
            var pos = cell.tilePosition;

            for (var x = -1; x <= 1; x++)
                for (var y = -1; y <= 1; y++)
                {
                    var neighbor = _map.GetCellFromPos(new Vector2Int(pos.x + x, pos.y + y));
                    if(neighbor && (neighbor.Walkable || (ignoreWalkability && neighbor == objective)) && neighbor != cell && !cameFrom.ContainsValue(neighbor) )
                        neighbors.Add(neighbor);
                }
            
            return neighbors;
        }
    }
}