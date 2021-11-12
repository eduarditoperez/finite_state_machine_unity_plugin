using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Fsm.Core;
using Fsm.State;
using Fsm.Repository;
using System.Collections.Generic;
using System.Linq;

// TODO: add namespace
public class FiniteStateMachineView : GraphView
{
    public new class UxmlFactory : UxmlFactory<FiniteStateMachineView, GraphView.UxmlTraits> { }

    private FiniteStateMachine _fsm;
    private IAssetRepository _assetRepository;

    public FiniteStateMachineView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Fsm/Editor/FiniteStateMachineEditor.uss");
        styleSheets.Add(styleSheet);

        _assetRepository = new UnityAssetRepository();
    }

    internal void PopulateView(FiniteStateMachine fsm)
    {
        _fsm = fsm;
        _fsm.AssetRepository = _assetRepository;

        graphViewChanged -= OnGraphViewChange;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChange;
        if (_fsm.IsNotEmpty)
        {
            _fsm.States.ForEach(state => CreateStateView(state));
        }
    }

    private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
    { 
        // Here we remove all elements in the view
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                StateView stateView = element as StateView;
                if (stateView != null)
                {
                    _fsm.RemoveState(stateView.State);
                }

                Edge edge = element as Edge;
                if (edge != null)
                {
                    StateView outputStateView = edge.output.node as StateView;
                    StateView inputStateView = edge.input.node as StateView;
                    _fsm.TryRemoveTransition(outputStateView.State, inputStateView.State);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                StateView outputStateView = edge.output.node as StateView;
                StateView inputStateView = edge.input.node as StateView;
                _fsm.TryAddTransition(outputStateView.State, inputStateView.State);
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

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort 
            => endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
    }


}
