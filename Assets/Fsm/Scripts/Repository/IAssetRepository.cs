
namespace Fsm.Repository
{
    public interface IAssetRepository
    {
        void AddObjectToAsset(UnityEngine.Object objectToAdd, UnityEngine.Object assetObject);
        void RemoveObjectFromAsset(UnityEngine.Object objectToRemove);
        void SaveAssets();
    }
}