using UnityEngine;

namespace Fsm.Repository
{
    /// <summary>
    /// Used for testing purposes
    /// </summary>
    public class NoopAssetRepository : IAssetRepository
    {
        public void AddObjectToAsset(Object objectToAdd, Object assetObject) {}
        public void RemoveObjectFromAsset(Object objectToRemove) {}
        public void SaveAssets() {}
    }
}
