using UnityEngine;
using Assets.Tilemaps;

namespace Assets.ScriptableObjects
{
    public abstract class AlgorithmBase : ScriptableObject
    {
        public abstract void Apply(TilemapStructure tilemap);
    }
}
