using System.Diagnostics;
using Godot;

namespace ProjectLife.addons.behaviortree;

public abstract partial class BTNode : Node
{
	/**
	 * Base class from which every node in the behavior tree inherits.
	 * You don't usually need to instance this node directly.
	 * To define your behaviors, use and extend BTLeaf instead.
	 */

	// (Optional) Emitted after a tick() call. True is success, false is failure.
	[Signal]
	public delegate void Ticked(); //TODO Implement these using global event manager
	// Emitted if abort_tree is set to true
	[Signal]
	public delegate void AbortedTree();

	public enum BTNodeState
	{
		Failure,
		Success,
		Running
	}

	// Turn this off to make the node fail at each tick.
	[Export] public bool IsActive = true;
	
	// Turn this on to print the name of the node at each tick.
	[Export] public bool IsDebug;
	
	// Turn this on to abort the tree after completion.
	[Export] public bool AbortTree;

	public BTNodeState State;

	public override void _Ready()
	{
		if (IsActive)
			Succeed();
		else
		{
			// push_warning("Deactivated BTNode '" + name + "', path: '" + get_path() + "'")
			Fail();
		}
	}

	/**
	 * IMPLEMENT THE FOLLOWING FUNCTIONS
	 * You just need to implement them. DON'T CALL THEM MANUALLY.
	 */

	// Public: Called before the tick happens used for preperation for action or conditional
	public abstract void PreTick(Node agent, Blackboard blackboard);

	// This is where the core behavior goes and where the node state is changed.
	// You must return either succeed() or fail() (check below), not just set the state.
	public abstract void Tick(Node agent, Blackboard blackboard);

	// Called after the tick for anything that needs to happen after the action
	public abstract void PostTick(Node agent, Blackboard blackboard, bool result);

	// DO NOT OVERRIDE ANYTHING FROM HERE ON

	/**
	 * BEGIN: RETURN VALUES
	 * Your _tick() must return one of the following functions.
	 */

	// Return this to set the state to success.
	public bool Succeed()
	{
		State = BTNodeState.Success;
		return true;
	}

	public bool Fail()
	{
		State = BTNodeState.Failure;
		return false;
	}
	
	public bool SetState(BTNodeState value)
	{
		switch (value)
		{
			case BTNodeState.Success:
				return Succeed();
			case BTNodeState.Failure:
				return Fail();
		}
		Debug.Assert(false, "Invalid BTNodeState assignment. Can only set to success or failure.");
		return false;
	}
	
	// END: RETURN VALUES
	
	// Dont call these

	public void Run()
	{
		State = BTNodeState.Running;
	}

	public bool Succeeded()
	{
		return State == BTNodeState.Success;
	}

	public bool Failed()
	{
		return State == BTNodeState.Failure;
	}

	public bool Running()
	{
		return State == BTNodeState.Running;
	}

	public BTNodeState GetState()
	{
		return State;
	}
	
	// DO NOT OVERRIDE THIS!
	public bool Tick(Node agent, Blackboard blackboard)
	{
		if (!IsActive)
			return Fail();

		if (Running())
			return false;

		if (IsDebug)
			Print(Name);
		
		// Do stuff before core behavior
		PreTick(agent, blackboard);
		
		Run();

		bool result = Tick(agent, blackboard);

		if (result is GDScriptFunctionState) //TODO Implement these need to figure out how this converts to C#
		{
			Debug.Assert(Running(), "BTNode execution was suspended. Did you succeed() or fail() before yield?");
			result = await (result, "completed");
		}
		
		Debug.Assert(!Running(), "BTNode executed but it's still running. Did you forget to return succeed() or fail()?");
		
		// Do stuff after core behavior depending on the result
		PostTick(agent, blackboard, result);
		
		// Notify completion and new state (i.e. the result of the execution)
		//EmitSignal("tick", result); //TODO implement signals here
		
		// Queue tree abortion at the end of current tick
		if (AbortTree)
			EmitSignal("abort_tree");

		return result;
	}
}