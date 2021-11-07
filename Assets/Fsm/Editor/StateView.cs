using Fsm.State;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

// TODO: add namespace
public class StateView : UnityEditor.Experimental.GraphView.Node
{
    public FsmStateBase State;
    public Port InputPort;
    public Port OutputPort;

    public StateView(FsmStateBase state)
    {
        this.State = state;
        this.title = state.name;
        this.viewDataKey = state.Guid;

        style.left = state.Position.x;
        style.top = state.Position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    private void CreateInputPorts()
    {
        InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, null);

        if (InputPort != null)
        {
            InputPort.name = "";
            inputContainer.Add(InputPort);
        }
    }

    private void CreateOutputPorts()
    {
        OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, null);

        if (OutputPort != null)
        {
            OutputPort.name = "";
            inputContainer.Add(OutputPort);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        State.Position.x = newPos.xMin;
        State.Position.y = newPos.yMin;
    }
}
