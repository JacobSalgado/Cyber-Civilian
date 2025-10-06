using System.Collections.Generic;

/// <summary>
/// <para>Pre-defined action for an entity.</para>
/// <para>NOTE: all classes inheriting this class do not need to override all the functions (i.e. use what's needed)</para>
/// </summary>
public abstract class State
{
    // constructors
    public State(Entity new_entity) { }
    public State(GameManager gameManager) { }

    /// <summary>
    /// <para>Startup behavior for the given State.</para>
    /// 
    /// <para>Example uses: initialize variables, reset counters, assertion checks, etc.</para>
    /// </summary>
    /// <param name="args">
    /// <para>Optional parameters for the State structured as a dictionary.</para>
    /// <para>Should be used if the state requires data from other classes</para>
    /// <para>
    /// Example Use: 
    /// EnterState({
    ///     "new_time": 4f,
    ///     "counter_start": 4});
    /// </para>
    /// </param>
    public virtual void EnterState(Dictionary<string, object> args = null) { }

    /// <summary>
    /// Execution of the desired state behavior
    /// </summary>
    public virtual void UpdateState() { }

    /// <summary>
    /// <para>End behavior for the given State.</para>
    /// 
    /// <para>Example uses: resetting any variables/behaviors, setting up new data points</para>
    /// </summary>
    /// <param name="args">
    /// <para>Optional parameters for the State structured as a dictionary.</para>
    /// <para>Should be used if the state requires data from other classes</para>
    /// <para>
    /// Example Use: 
    /// ExitState({
    ///     "new_time": 4f,
    ///     "counter_start": 4});
    /// </para>
    /// </param>
    public virtual void ExitState(Dictionary<string, object> args = null) { }
}
