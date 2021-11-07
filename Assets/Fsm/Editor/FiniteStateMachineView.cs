using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Fsm.Core;
using Fsm.State;

// TODO: add namespace
public class FiniteStateMachineView : GraphView
{
    public new class UxmlFactory : UxmlFactory<FiniteStateMachineView, GraphView.UxmlTraits> { }

    private FiniteStateMachine _fsm;

    public FiniteStateMachineView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Fsm/Editor/FiniteStateMachineEditor.uss");
        styleSheets.Add(styleSheet);
    }

    internal void PopulateView(FiniteStateMachine fsm)
    {
        _fsm = fsm;

        graphViewChanged -= OnGraphViewChange;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChange;
        if (_fsm.HasStates)
        {
            _fsm.States.ForEach(state => CreateStateView(state));
        }
    }

    private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                StateView stateView = element as StateView;
                if (stateView != null)
                {
                    _fsm.RemoveState(stateView.State);
                }
            });
        }

        return graphViewChange;
    }

    private void CreateStateView(FsmStateBase fsmStateBase)
    {
        StateView stateView = new StateView(fsmStateBase);
        AddElement(stateView);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<FsmStateBase>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", 
                    (a) => CreateState(type));
            }
        }
    }

    private void CreateState(System.Type stateType)
    {
        if (_fsm.TryCreateState(stateType, out FsmStateBase state))
        {
            CreateStateView(state);
        }
    }
}
