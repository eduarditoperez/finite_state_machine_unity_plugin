using UnityEditor;
using UnityEngine;

namespace Fsm.Utility
{
    public class UnityUndoRedoUtility : IUndoRedoUtility
    {
        public void RecordObject(Object objectToUndo, string name)
        {
            Undo.RecordObject(objectToUndo, name);
        }

        public void SetDirty(Object target)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
