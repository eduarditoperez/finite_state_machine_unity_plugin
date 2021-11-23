using UnityEngine;

namespace Fsm.Utility
{
    public interface IUndoRedoUtility
    {
        void RecordObject(Object objectToUndo, string name);
        void SetDirty(Object dirtyObjectame);
    }
}
