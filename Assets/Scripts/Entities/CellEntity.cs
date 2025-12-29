using Items;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class CellEntity: MonoBehaviour
    {
        public bool IsWalkable;
        public bool IsInteractable;
        public int posX;
        public int posY;
    }
}