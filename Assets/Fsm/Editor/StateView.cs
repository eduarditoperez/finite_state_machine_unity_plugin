using Fsm.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: add namespace
public class StateView : UnityEditor.Experimental.GraphView.Node
{
    public FsmStateBase State;

    public StateView(FsmStateBase state)
    {
        this.State = state;
        this.title = state.name;
        //this.viewDataKey = state.Guid;

        //style.left = state.Position.x;
        //style.top = state.Position.y;
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        //State.Position.x = newPos.xMin;
        //State.Position.y = newPos.yMin;
    }
}
