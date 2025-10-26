using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Background script for managing a set of states
/// </summary>
public class StateManager : MonoBehaviour
{
    public Dictionary<string, State> stateMap = new();
    public State current_state;

    /// <summary>
    /// Add a new state to the stateMap
    /// </summary>
    /// <param name="name">The name of the state</param>
    /// <param name="state">The State instance</param>
    public void AddState(string name, State state)
    {
        stateMap.Add(name, state);
    }

    /// <summary>
    /// Change the current_state to the new_state
    /// </summary>
    /// <param name="new_state">The state to begin</param>
    /// <param name="args">Optional parameters for the new state</param>
    public void ChangeState(string new_state, Dictionary<string, object> args = null)
    {
        if (stateMap.ContainsKey(new_state))
        {
            current_state?.ExitState(args);
            current_state = stateMap[new_state];
            current_state.EnterState(args);
        }
        else Debug.LogError(string.Format("State does not exist: {0}", new_state));
    }
}
