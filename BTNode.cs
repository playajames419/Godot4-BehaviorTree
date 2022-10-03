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

	// (Optional) Emitted after a Tick() call. True is success, false is failure.
	[Signal]
	public delegate void TickedBehaviorEventHandler(); //TODO Implement these using global event manager
	
	// Emitted if AbortTree is set to true
	[Signal]
	public delegate void AbortedBehaviorEventHandler();

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
			GD.PushWarning("Deactivated BTNode '" + Name + "', path: '" + GetPath() + "'");
			Fail();
		}
	}

	/**
	 * IMPLEMENT THE FOLLOWING FUNCTIONS
	 * You just need to implement them. DON'T CALL THEM MANUALLY.
	 */

	// Public: Called before the tick happens used for preperation for action or conditional
	public abstract void _preTick(Node agent, Blackboard blackboard);

	// This is where the core behavior goes and where the node state is changed.
	// You must return either succeed() or fail() (check below), not just set the state.
	public abstract bool _tick(Node agent, Blackboard blackboard);

	// Called after the tick for anything that needs to happen after the action
	public abstract void _postTick(Node agent, Blackboard blackboard, bool result);

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
			GD.Print(Name);
		
		// Do stuff before core behavior
		_preTick(agent, blackboard);
		
		Run();

		bool result = _tick(agent, blackboard);

		// if (result is GDScriptFunctionState) //TODO Implement these need to figure out how this converts to C# GDScriptFunctionState is depreciated v 2.1
		// {
		// 	Debug.Assert(Running(), "BTNode execution was suspended. Did you succeed() or fail() before await?");
		// 	result = await ;
		// }
		
		Debug.Assert(!Running(), "BTNode executed but it's still running. Did you forget to return succeed() or fail()?");
		
		// Do stuff after core behavior depending on the result
		_postTick(agent, blackboard, result);
		
		// Notify completion and new state (i.e. the result of the execution)
		EmitSignal(nameof(TickedBehavior));
		
		// Queue tree abortion at the end of current tick
		if (AbortTree)
			EmitSignal(nameof(AbortedBehavior));

		return result;
	}
}