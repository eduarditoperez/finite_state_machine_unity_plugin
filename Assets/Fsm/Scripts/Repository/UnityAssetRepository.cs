using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Fsm.Repository
{
    public class UnityAssetRepository : IAssetRepository
    {
        public void AddObjectToAsset(Object objectToAdd, Object assetObject)
        {
            AssetDatabase.AddObjectToAsset(objectToAdd, assetObject);
            SaveAssets();
        }

        public void RemoveObjectFromAsset(Object objectToRemove)
        {
            AssetDatabase.RemoveObjectFromAsset(objectToRemove);
            SaveAssets();
        }

        public void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }
    }
}
