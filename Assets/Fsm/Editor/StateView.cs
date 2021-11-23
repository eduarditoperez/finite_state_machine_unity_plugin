using Fsm.State;
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// TODO: add namespace and rename to FsmStateView
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
        SetupDataBindings();
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
            outputContainer.Add(OutputPort);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        // Undo/Redo
        Undo.RecordObject(State, "FiniteStateMachine (SetPosition)");
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

    public void UpdateState()
    {
        RemoveFromClassList(FsmStateCondition.Idle.ToString());
        RemoveFromClassList(FsmStateCondition.Running.ToString());

        if (Application.isPlaying)
        {
            switch (State.StateCondition)
            {
                case FsmStateCondition.Idle:
                    AddToClassList(FsmStateCondition.Idle.ToString());
                    break;
                case FsmStateCondition.Running:
                    AddToClassList(FsmStateCondition.Running.ToString());
                    break;
            }
        }
    }

    private void SetupDataBindings()
    {
        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "Description";
        descriptionLabel.Bind(new SerializedObject(State));

        Label stateConditionLabel = this.Q<Label>("state-condition");
        stateConditionLabel.bindingPath = "StateCondition";
        stateConditionLabel.Bind(new SerializedObject(State));
    }
}