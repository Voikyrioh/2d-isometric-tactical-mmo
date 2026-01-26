using System.Collections.Generic;
using UnityEngine;

namespace Database.Models
{
    public struct MobLevelData
    {
        public int Level;
        public int Health;
        public int Damage;
        public int Movement;
        public int Actions;
    }
    
    [CreateAssetMenu(fileName = "Monstre", menuName = "Entities/Mob")]
    public class MobData : ScriptableObject
    {
        public string Name;
        public MobLevelData[] Levels;
        public Texture2D SpriteSheet;
    }
}