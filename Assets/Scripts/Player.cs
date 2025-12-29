using System;
using System.Collections.Generic;
using DefaultNamespace;
using SpriteManager;
using UnityEngine;
using Utils;

public class Player : MonoBehaviour
{
    [SerializeField] public Sprite playerSprite;
    [SerializeField] public float speed = 1f;
    private MapManager map;
    private PlayerCursor cursor;
    private Vector2Int position = Vector2Int.zero;
    private Cell currentCell;
    
    private List<Cell> path;
    private int step;
    private Vector3 currentTarget;
    private CharacterSprite spriteManager;
    private bool playerInitialized = false;
    private Interactable targetEntity;
    
    private void Awake()
    {
        var renderer = GetComponent<SpriteRenderer>();
        spriteManager = new CharacterSprite(playerSprite, renderer);
    }

    public void Init(MapManager mapRef, PlayerCursor cursorRef)
    {
        map = mapRef;
        cursor = cursorRef;
        playerInitialized = true;
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
        var newList = new List<Cell>();
        if (path?.Count > 0)
        {
            newList.Add(path[step]);    
        }
        
        newList.AddRange(newPath);
        path = newList;
        step = 0;
        SetTargetCoordinate(path[step]);
        UpdateCharacterOrientation(new Vector2Int(path[step].tilePosition.x, path[step].tilePosition.y));
    }
    
    public void MovePlayer(Vector2Int dest)
    {
        SetNextPath(new PathFinder(map).getPath(currentCell, map.GetCellFromPos(dest)));
        targetEntity = null;
    }

    private void UpdateCharacterOrientation(Vector2Int nextStep)
    {
        var direction = new Vector2Int(Math.Clamp(nextStep.x - position.x, -1, 1), Math.Clamp(nextStep.y - position.y, -1, 1));
        var orientation = CharacterSprite.Orientation.So;
        switch (direction)
        {
            case { x: -1, y: 0 }:
                orientation = CharacterSprite.Orientation.S;
                break;
            case { x: -1, y:  1 }:
                orientation = CharacterSprite.Orientation.Se;
                break;
            case { x: 0, y:  1 }:
                orientation = CharacterSprite.Orientation.E;
                break;
            case { x: 1, y:  1 }:
                orientation = CharacterSprite.Orientation.Ne;
                break;
            case { x: 1, y:  0 }:
                orientation = CharacterSprite.Orientation.N;
                break;
            case { x: 1, y:  -1 }:
                orientation = CharacterSprite.Orientation.No;
                break;
            case { x: 0, y:  -1 }:
                orientation = CharacterSprite.Orientation.O;
                break;
        }
        
        spriteManager.updateOrientation(orientation);
    }
    
    private void SetTargetCoordinate(Cell dest) {
        var gamepos = dest.gameObject.transform.position;
        currentTarget = new Vector3(gamepos.x, gamepos.y + map.cellSize.y/2, gamepos.z + 1);
    }

    private void FixedUpdate()
    {
        UpdatePosition(Time.deltaTime);
    }

    private void UpdatePosition(float deltaTime)
    {
        if (path == null) return;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, currentTarget, deltaTime * speed);

        if (gameObject.transform.position != currentTarget) return;
        if (step < path.Count - 1)
        {
            currentCell = path[step];
            position = new Vector2Int(currentCell.tilePosition.x, currentCell.tilePosition.y);
            step++;
            SetTargetCoordinate(path[step]);
            UpdateCharacterOrientation(new Vector2Int(path[step].tilePosition.x, path[step].tilePosition.y));
        }
        else
        {
            path = null;
            if (targetEntity != null)
            {
                targetEntity.Interact();
                targetEntity = null;
            }
        }
    }

    public void InteractWithEntity(Vector2Int pos, Interactable entity)
    {
        var cell = map.GetCellFromPos(pos);
        if (cell)
        {
            var pathToEntity = (new PathFinder(map)).getPath(currentCell, cell, true);
            pathToEntity.RemoveAt(pathToEntity.Count - 1);
            targetEntity = entity;
            SetNextPath(pathToEntity);
        }
    }
}
