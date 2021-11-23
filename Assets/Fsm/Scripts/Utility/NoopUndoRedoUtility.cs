using UnityEngine;

namespace Fsm.Utility
{
    public class NoopUndoRedoUtility : IUndoRedoUtility
    {
        public void RecordObject(Object objectToUndo, string name) {}
        public void SetDirty(Object dirtyObjectame) {}
    }
}
