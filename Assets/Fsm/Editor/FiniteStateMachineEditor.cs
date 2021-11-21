using Fsm.Core;
using Fsm.RunTime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

// TODO: add namespace
public class FiniteStateMachineEditor : EditorWindow
{
    private FiniteStateMachineView _fsmView;
    private FiniteStateMachineInspectorView _inspectorView;

    [MenuItem("Fsm/Editor")]
    public static void OpenWindow()
    {
        FiniteStateMachineEditor wnd = GetWindow<FiniteStateMachineEditor>();
        if (wnd != null)
        {
            wnd.titleContent = new GUIContent("FiniteStateMachineEditor");
            wnd.minSize = new Vector2(800, 600);
        }
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
        _fsmView.OnStateSelected = OnStateSelectionChanged;
        _inspectorView = root.Q<FiniteStateMachineInspectorView>();

        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        FiniteStateMachine fsm = Selection.activeObject as FiniteStateMachine;
        if (fsm == null && Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject
                .TryGetComponent<FiniteStateMachineRunner>(out FiniteStateMachineRunner runner))
            {
                fsm = runner.FiniteStateMachine;
            }
        }

        if (Application.isPlaying)
        {
            if (fsm)
            {
                _fsmView.PopulateView(fsm);
            }
            return;
        }
        
        if (fsm && AssetDatabase.CanOpenForEdit(fsm))
        {
            _fsmView.PopulateView(fsm);
        }
    }

    private void OnStateSelectionChanged(StateView stateView)
    {
        _inspectorView.UpdateSelection(stateView);
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanteId, int line)
    {
        if (Selection.activeObject is FiniteStateMachine)
        {
            OpenWindow();
            return true;
        }
        return false;
    }
}
