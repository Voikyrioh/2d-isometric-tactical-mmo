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
        
        public List<Cell> getPath(Cell start, Cell end, bool ignoreWalkability = false, bool diagonals = false)
        {
            openList.Add(start);
            objective = end;
            
            var gScore = new Dictionary<Cell, int>();
            gScore[start] = 0;
            
            var fScore = new Dictionary<Cell, int>();
            fScore[start] = heuristic(start, end, diagonals);

            while (openList.Count > 0)
            {
                var current = openList.OrderBy(x => fScore[x]).First();
                openList.Remove(current);
                if (current == end) return reconstructPath(end);
                
                foreach (var neighbor in GetNeighbors(current, ignoreWalkability, diagonals))
                {
                    var newCostToNeighbor = gScore[current] + 1;
                    if (openList.Contains(neighbor) && newCostToNeighbor >= gScore[neighbor]) continue;
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = newCostToNeighbor;
                    fScore[neighbor] = newCostToNeighbor + heuristic(neighbor, end, diagonals);
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
            return path;
        }

        private int heuristic(Cell a, Cell b, bool diagonals)
        {
            var diffX = Mathf.Abs(a.tilePosition.x - b.tilePosition.x);
            var diffY = Mathf.Abs(a.tilePosition.y - b.tilePosition.y);

            return diffX + diffY;
        }

        private List<Cell> GetNeighbors(Cell cell, bool ignoreWalkability, bool diagonals)
        {
            var neighbors = new List<Cell>();
            var pos = cell.tilePosition;
            var possiblePositions = new List<Vector2Int>();
            possiblePositions.AddRange(new Vector2Int[] {
                new(pos.x + 1, pos.y),
                new(pos.x, pos.y + 1),
                new(pos.x - 1, pos.y),
                new(pos.x, pos.y - 1)
            });

            if (diagonals) 
            {
                possiblePositions.AddRange(new Vector2Int[] {
                    new(pos.x + 1, pos.y + 1),
                    new(pos.x + 1, pos.y - 1),
                    new(pos.x - 1, pos.y + 1),
                    new(pos.x - 1, pos.y - 1)
                });
            }

            foreach (var p in possiblePositions) {
                var neighbor = _map.GetCellFromPos(p);
                if(neighbor && (neighbor.Walkable || (ignoreWalkability && neighbor == objective)) && neighbor != cell && !cameFrom.ContainsValue(neighbor) )
                    neighbors.Add(neighbor);
            }
            
            return neighbors;
        }
    }
}