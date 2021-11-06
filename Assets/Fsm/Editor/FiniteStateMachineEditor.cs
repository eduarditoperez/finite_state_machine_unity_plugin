using Fsm.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// TODO: add namespace
public class FiniteStateMachineEditor : EditorWindow
{
    private FiniteStateMachineView _fsmView;
    private FiniteStateMachineInspectorView _inspectorView;

    [MenuItem("Window/FiniteStateMachineEditor")]
    public static void OpenWindow()
    {
        FiniteStateMachineEditor wnd = GetWindow<FiniteStateMachineEditor>();
        wnd.titleContent = new GUIContent("FiniteStateMachineEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Fsm/Editor/FiniteStateMachineEditor.uxml");
        visualTree.CloneTree(root);
        
        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Fsm/Editor/FiniteStateMachineEditor.uss");
        root.styleSheets.Add(styleSheet);

        // Populate
        _fsmView = root.Q<FiniteStateMachineView>();
        _inspectorView = root.Q<FiniteStateMachineInspectorView>();

        //OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        FiniteStateMachine fsm = Selection.activeObject as FiniteStateMachine;
        if (fsm)
        {
            _fsmView.PopulateView(fsm);
        }
    }
}