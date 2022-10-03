using System.Diagnostics;
using Godot;

namespace ProjectLife.addons.behaviortree;

[Tool]
public partial class BehaviorTree : Node
{
	/**
	 * This is your main node. Put one of these at the root of the scene and start adding BTNodes.
	 * A Behavior Tree only accepts ONE entry point (so one child).
	 */
	
	[Export] public bool IsActive;
	[Export] private NodePath _blackboard;
	[Export] private NodePath _agent;
	[Export(PropertyHint.Enum, "Idle, Physics")] public int SyncMode;
	[Export] public bool IsDebug;

	public Variant TickResult;
	public Node Agent;
	public Blackboard Blackboard;
	public BTNode Controller; //bt_root is renamed to controller

	public override void _Ready()
	{
		Agent = GetNode<Node>(_agent);
		//Blackboard = GetNode<Blackboard>(_blackboard);
		//Controller = GetChild<BTNode>(0);
		
		Debug.Assert(GetChildCount() == 1, "Behavior Tree Error: A Behavior Tree can only have one entry point.");
		//Controller.Connect(nameof(BTNode.AbortedTree), this, Abort());
		Controller.PropagateCall("connect", ["abortTree", this, "abort"]); //TODO connect to signals from BTNode
		//Start();
	}

	public override void _Process(double delta)
	{
		if (!IsActive)
		{
			SetProcess(false);
			return;
		}

		if (IsDebug)
			Print();

		TickResult = Controller.Tick(Agent, Blackboard);

		if (TickResult is GDScriptFunctionState) //TODO Figure how to implement this
		{
			SetProcess(false);
			await (TickResult, "completed"); //TODO update to use new await 
			SetProcess(true);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsActive)
		{
			SetPhysicsProcess(false);
			return;
		}
		
		if (IsDebug)
			Print();

		TickResult = Controller.Tick(Agent, Blackboard);

		if (TickResult is GDScriptFunctionState)
		{
			SetPhysicsProcess(false);
			await (TickResult, "completed");
			SetPhysicsProcess(true);
		}
	}

	// Internal: Set up if we are using process or physics_process for the behavior tree SyncMode: Idle = Process, Physics = PhysicsProcess
	protected void Start()
	{
		if (!IsActive)
			return;

		switch (SyncMode)
		{
			case 0:
				SetPhysicsProcess(false);
				SetProcess(true);
				break;
			case 1:
				SetProcess(false);
				SetPhysicsProcess(true);
				break;
		}
		
		
	}

	// Public: Set the tree to inactive when a abort_tree signal is sent from controller
	public void Abort()
	{
		IsActive = false;
	}
}