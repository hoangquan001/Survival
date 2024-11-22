using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum XStateDefine
{
    
}
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}
public class StateMachine : MonoBehaviour
{
    public IState preState;
    public IState currentState;
    private Dictionary<XStateDefine, IState> stateMap = new Dictionary<XStateDefine, IState>();

    public void ChangeState(XStateDefine state)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = stateMap[state];
        currentState.Enter();
    }
    
    public void RegisterState(XStateDefine state, IState stateImpl)
    {
        if (!stateMap.ContainsKey(state))
        {
            stateMap.Add(state, stateImpl);
        }
    }
    
    public void UnRegisterState(XStateDefine state)
    {
        if (stateMap.ContainsKey(state))
        {
            stateMap.Remove(state);
        }
    }
    
}

