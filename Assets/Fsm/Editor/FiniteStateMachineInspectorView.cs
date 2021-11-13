using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;

public class FiniteStateMachineInspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<FiniteStateMachineInspectorView, VisualElement.UxmlTraits> { }

    private Editor _editor;

    public FiniteStateMachineInspectorView() {}

    internal void UpdateSelection(StateView stateView)
    {
        Clear();

        UnityEngine.GameObject.DestroyImmediate(_editor);

        _editor = Editor.CreateEditor(stateView.State);

        IMGUIContainer container = new IMGUIContainer(() => { _editor.OnInspectorGUI(); } );
        Add(container);

    }
}
