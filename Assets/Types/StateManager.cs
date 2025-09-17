using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public Dictionary<string, State> stateMap = new Dictionary<string, State>();
    public State current_state;

    public void AddState(string name, State state)
    {
        stateMap.Add(name, state);
    }

    public void ChangeState(string new_state, Dictionary<string, object> args = null)
    {
        if (stateMap.ContainsKey(new_state))
        {
            current_state.ExitState(args);
            current_state = stateMap[new_state];
            current_state.EnterState(args);

        }
        else print("invalid state name");
    }
}
