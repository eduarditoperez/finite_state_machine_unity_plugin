using Fsm.State;
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

// TODO: add namespace
public class StateView : Node
{
    public Action<StateView> OnStateSelected;
    public FsmState State;
    public Port InputPort;
    public Port OutputPort;

    public StateView() {}

    public StateView(FsmState state) : base("Assets/Fsm/Editor/StateViewTemplate.uxml")
    {
        this.State = state;
        this.title = state.name;
        this.viewDataKey = state.Guid;

        style.left = state.Position.x;
        style.top = state.Position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupStates();
    }

    private void CreateInputPorts()
    {
        if (State is RootState)
        {
            return;
        }

        InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, null);
        if (InputPort != null)
        {
            InputPort.name = string.Empty;
            InputPort.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(InputPort);
        }
    }

    private void CreateOutputPorts()
    {
        if (State is EndState)
        {
            return;
        }

        OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, null);
        if (OutputPort != null)
        {
            OutputPort.name = string.Empty;
            OutputPort.style.flexDirection = FlexDirection.ColumnReverse;
            inputContainer.Add(OutputPort);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        // TODO: undo/redo for the position. Use an interface
        Undo.RecordObject(State, "FiniteStateMachine (Set Position)");
        State.Position.x = newPos.xMin;
        State.Position.y = newPos.yMin;
        EditorUtility.SetDirty(State);
    }

    private void SetupStates()
    {
        if (State is RootState)
        {
            AddToClassList("root");
            return;
        }

        if (State is EndState)
        {
            AddToClassList("end");
            return;
        }

        AddToClassList("state");
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnStateSelected?.Invoke(this);
    }

}
