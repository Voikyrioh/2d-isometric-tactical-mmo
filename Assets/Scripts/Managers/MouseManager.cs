using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.Managers
{
    public class MouseManager : MonoBehaviour
    {
        private Dictionary<string, double> EventTimeBuffer = new();
        private InputSystem inputSystem;
        private Vector2 mousePos;
        [CanBeNull] private Interactable hoveredInteractable;
        
        private void Awake()
        {
            inputSystem = new InputSystem();
        }

        private void OnEnable()
        {
            inputSystem.Enable();
        }

        private void OnDisable()
        {
            inputSystem.Disable();
        }

        private void Start()
        {
            inputSystem.UI.Point.performed += GetHover;
            inputSystem.UI.Click.performed += Click;
        }

        private bool EventIsTriggerable(string eventName, double eventTime, double triggerRequirement)
        {
            if (EventTimeBuffer.ContainsKey(eventName) && !(eventTime - EventTimeBuffer[eventName] > triggerRequirement)) return false;
            EventTimeBuffer[eventName] = eventTime;
            
            return true;
        }

        private void GetHover(InputAction.CallbackContext ctx)
        {
            if (!EventIsTriggerable(ctx.action.name, ctx.time, 0.01)) return;
            hoveredInteractable?.RemoveHover();
            GameManager.Instance.cursorInstance.Hide(); 
            mousePos = ctx.action.ReadValue<Vector2>();
            if (GetTClassAtPos<Interactable>(mousePos, LayerMask.GetMask("Entities"), out var interactable))
            {
                interactable.Hover();
                hoveredInteractable = interactable;
            }
            else if (GetTClassAtPos<Cell>(mousePos, LayerMask.GetMask("Ground"), out var cell))
            {
                if (cell.Walkable)
                {
                    GameManager.Instance.cursorInstance.Show(cell.transform.position);
                }
            }
        }
        
        private void Click(InputAction.CallbackContext ctx)
        {
            if (!EventIsTriggerable(ctx.action.name, ctx.time, 0.2)) return;
            if (GetTClassAtPos<Interactable>(mousePos, LayerMask.GetMask("Entities"), out var interactable))
                interactable.Click();
            else if (GetTClassAtPos<Cell>(mousePos, LayerMask.GetMask("Ground"), out var cell))
            {
                if (cell.Walkable)
                {
                    GameManager.Instance.playerInstance.MovePlayer(new Vector2Int(cell.tilePosition.x, cell.tilePosition.y));
                }
            }
        }

        private bool GetTClassAtPos<T>(Vector2 pos, LayerMask mask, out T instance)
        {
            var cameraPoints = GameManager.Instance.mainCameraInstance.ScreenToWorldPoint(pos);
            var hit = Physics2D.Raycast(cameraPoints, Vector2.zero, 30f, mask);
            instance = default(T);
            return hit && (instance = hit.collider.GetComponent<T>()) != null;
        }
        
    }
}