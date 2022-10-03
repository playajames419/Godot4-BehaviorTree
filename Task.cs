using System.Diagnostics;
using Godot;

namespace ProjectLife.addons.behaviortree;

internal enum State {
	FRESH,
	RUNNING,
	FAILED,
	SUCCEEDED,
	CANCELED
}

/**
 * This Task class will provide all of the essential functionality for the Leaf and Branch Nodes
 * that may be extended upon or adopted as need be.
 */

[Tool]
public abstract partial class Task : Node
{

	[Export] public BehaviorTree Control = null;
	private Task _tree = null;
	private State _status = State.FRESH;

	public override void _Ready()
	{
		Debug.Assert(Control != null, "Behavior Tree Error: " + Name + " can not have a null control property.");
	}

	protected void _running()
	{
		_status = State.RUNNING;
		if (Control != null)
		{
			Control._childRunning();
		}
	}

	protected void _success()
	{
		_status = State.SUCCEEDED;
		if (Control != null)
		{
			Control._childFail();
		}
	}

	protected void _fail()
	{
		_status = State.FAILED;
		if (Control != null)
			Control._childFail();
	}

	protected virtual void _cancel()
	{
		if (_status != State.RUNNING) return;
		
		_status = State.CANCELED;
		foreach (var node in GetChildren())
		{
			if (node is Task child)
				child._cancel();
		}
	}

	protected internal abstract void _run(); // Process the task and call running(), success(), or fail()

	protected internal abstract void _childSuccess();

	protected abstract void _childFail();

	protected abstract void _childRunning();

	protected virtual void _start()
	{
		_status = State.FRESH;
		foreach (var node in GetChildren())
		{
			if (node is not Task child) continue;
			child.Control = this;
			child._tree = _tree;
			child._start();
		}
	}

	protected void _reset()
	{
		_cancel();
		_status = State.FRESH;
	}
	
}
