using System;
using DefaultNamespace;
using DefaultNamespace.Managers;
using Unity.VisualScripting;
using UnityEngine;

public class Ressource : CellEntity, Interactable
{
    [SerializeField] private float respawnTime;
    [SerializeField] private int levelRequirement;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Sprite depletedSprite;
    [SerializeField] public Vector2[] colliderDefault;
    [SerializeField] public Vector2[] colliderDepleted;
    
    public bool depleted;
    private SpriteRenderer spriteRenderer;
    private float respawnTimeRemaining;
    private PolygonCollider2D collider;
    
    private void Awake()
    {
        IsInteractable = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_Outline_thickness", 0f);
        this.AddComponent<PolygonCollider2D>();
        collider = GetComponent<PolygonCollider2D>();
        collider.layerOverridePriority = 1;
        collider.points = colliderDefault;
    }

    private void Deplete()
    {
        spriteRenderer.sprite = depletedSprite;
        depleted = true;
        respawnTimeRemaining = respawnTime;
        collider.points = colliderDepleted;
    }
    
    private void Regenerate()
    {
        spriteRenderer.sprite = sprite;
        depleted = false;
        collider.points = colliderDefault;
    }

    public void Hover()
    {
        spriteRenderer.material.SetFloat("_Outline_thickness", 0.1f);
    }

    public void RemoveHover()
    {
        spriteRenderer.material.SetFloat("_Outline_thickness", 0f);
    }
    
    public void Interact()
    {
        if (depleted) return;
        Deplete();
    }

    public void Click()
    {
        GameManager.Instance.playerInstance.InteractWithEntity(new Vector2Int(posX, posY), this);
    }

    private void Update()
    {
        if (depleted)
        {
            respawnTimeRemaining -= Time.deltaTime;
            if (respawnTimeRemaining <= 0)
            {
                Regenerate();
            }
        }
    }
}
