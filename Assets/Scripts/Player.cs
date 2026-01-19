using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Managers;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Characters
{
    public enum Orientation
    {
        S = 0,
        So = 1,
        O = 2,
        No = 3,
        N = 4,
        Ne = 5,
        E = 6,
        Se = 7,
    }
    
    public class Player : MonoBehaviour
    {
        [Header( "Player Display" )]
        [SerializeField] public Orientation orientation = Orientation.So;
        [SerializeField] public Sprite[] spriteSheet;
        [Header( "Player Stats" )]
        [SerializeField] public float speed = 1f;
        [SerializeField] public int hp = 1;
        [SerializeField] public int maxHP = 1;
        
        private MapManager map;
        private SpriteRenderer renderer;
        private Vector2Int position = Vector2Int.zero;
        private Cell currentCell;
        
        private List<Cell> path;
        private int step;
        private Vector3 currentTarget;
        private Interactable targetEntity;
        public bool isMoving;
        public UnityEvent<int> hpChanged { get; } = new ();

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        public void Init(MapManager mapRef, PlayerCursor cursorRef)
        {
            map = mapRef;
        }

        public void SetPosition(Vector2Int pos)
        {
            var cell = map.GetCellFromPos(pos);
            if (!cell) return;
            gameObject.transform.position = new Vector3(
                cell.gameObject.transform.position.x, 
                cell.gameObject.transform.position.y + map.cellSize.y/2, 
                cell.gameObject.transform.position.z + 1
            );
            currentCell = cell;
            position = pos;
        }

        private void SetNextPath(List<Cell> newPath)
        {
            path = isMoving || newPath[0] != currentCell ? newPath : newPath.GetRange(1, newPath.Count -1);
            step = 0;
            if (path.Count == 0) return;
            SetTargetCoordinate(path[step]);
            UpdateCharacterOrientation(new Vector2Int(path[step].tilePosition.x, path[step].tilePosition.y));
        }
        
        public void MovePlayer(Vector2Int dest)
        {
            SetNextPath(new PathFinder(GameManager.Instance.mapInstance).getPath(isMoving ? path[step] : currentCell, GameManager.Instance.mapInstance.GetCellFromPos(dest), false, true));
            targetEntity = null;
        }

        private void UpdateCharacterOrientation(Vector2Int nextStep)
        {
            var direction = new Vector2Int(Math.Clamp(nextStep.x - position.x, -1, 1), Math.Clamp(nextStep.y - position.y, -1, 1));
            switch (direction)
            {
                case { x: -1, y: -1 }:
                    orientation = Orientation.S;
                    break;
                case { x: -1, y:  0 }:
                    orientation = Orientation.So;
                    break;
                case { x: -1, y:  1 }:
                    orientation = Orientation.O;
                    break;
                case { x: 0, y:  1 }:
                    orientation = Orientation.No;
                    break;
                case { x: 1, y:  1 }:
                    orientation = Orientation.N;
                    break;
                case { x: 1, y:  0 }:
                    orientation = Orientation.Ne;
                    break;
                case { x: 1, y:  -1 }:
                    orientation = Orientation.E;
                    break;
                case { x: 0, y:  -1 }:
                    orientation = Orientation.Se;
                    break;
            }
            var spriteIndex = (int)orientation;
            spriteIndex = spriteIndex > 4 ? 5 - (spriteIndex - 3) : spriteIndex;
            renderer.flipX = orientation is Orientation.E or Orientation.Ne or Orientation.Se;
            renderer.sprite = spriteSheet.Length > spriteIndex ? spriteSheet[spriteIndex] : renderer.sprite;
        }
        
        private void SetTargetCoordinate(Cell dest) {
            var gamepos = dest.gameObject.transform.position;
            currentTarget = new Vector3(gamepos.x, gamepos.y + map.cellSize.y/2, gamepos.z + 1);
        }

        private void Update()
        {
            UpdatePosition(Time.deltaTime);
        }

        private void UpdatePosition(float deltaTime)
        {
            if (path == null) return;
            var newPos = Vector3.MoveTowards(gameObject.transform.position, currentTarget, deltaTime * speed);
            gameObject.transform.position = newPos;
            if (newPos != currentTarget) return;
            if (step < 0 || step >= path.Count) return;

            currentCell = path[step];
            position = new Vector2Int(currentCell.tilePosition.x, currentCell.tilePosition.y);
            if (step < path.Count - 1)
            {
                isMoving = true;
                step++;
                SetTargetCoordinate(path[step]);
                UpdateCharacterOrientation(new Vector2Int(path[step].tilePosition.x, path[step].tilePosition.y));
            }
            else
            {
                path = null;
                isMoving = false;
                if (targetEntity == null) return;
                targetEntity.Interact();
                GameManager.Instance.playerInstance.hpChanged.Invoke(hp -= 10);
                targetEntity = null;
            }
        }

        public void InteractWithEntity(Vector2Int pos, Interactable entity)
        {
            var cell = map.GetCellFromPos(pos);
            if (cell)
            {
                var pathToEntity = (new PathFinder(map)).getPath(currentCell, cell, true, true);
                pathToEntity.RemoveAt(pathToEntity.Count - 1);
                targetEntity = entity;
                SetNextPath(pathToEntity);
            }
        }
    }
}