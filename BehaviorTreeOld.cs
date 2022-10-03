using System.Diagnostics;
using Godot;
using Godot.Collections;


[Tool]
public partial class BehaviorTree : Node
{

	[Export] private bool _isEnabled;
	public Dictionary<Variant, Variant> Data;
	
	public override void _Ready()
	{
		Debug.Assert(GetChildCount() == 1 && GetChild(0) is Task, "Behavior Tree Error: "+ Name + " should have one child.");
		if (GetChildCount() == 1 && GetChild(0) is Task)
			Init();
	}

	private void Init()
	{
		Data = new Dictionary<Variant, Variant>();
	}

	public override void _Process(double delta)
	{
		if (!_isEnabled) return;
		var child = GetChild(0) as Task;
		child._run();
	}

	public void Enable()
	{
		_isEnabled = true;
	}

	public void Disable()
	{
		_isEnabled = false;
	}
}