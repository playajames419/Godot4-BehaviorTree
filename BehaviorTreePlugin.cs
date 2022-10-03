#if TOOLS
using Godot;

namespace ProjectLife.addons.behaviortree
{
	[Tool]
	public partial class BehaviorTreePlugin : EditorPlugin
	{
		public override void _EnterTree()
		{
			var behaviorTreeScript = GD.Load<Script>("res://addons/behaviortree/BehaviorTree.cs");
			var leafScript = GD.Load<Script>("res://addons/behaviortree/leafs/Leaf.cs");
			var parallelScript = GD.Load<Script>("res://addons/behaviortree/composites/Parallel.cs");
			var randomSelectorScript = GD.Load<Script>("res://addons/behaviortree/composites/RandomSelector.cs");
			var randomSequenceScript = GD.Load<Script>("res://addons/behaviortree/composites/RandomSequence.cs");
			var selectorScript = GD.Load<Script>("res://addons/behaviortree/composites/Selector.cs");
			var sequenceScript = GD.Load<Script>("res://addons/behaviortree/composites/Sequence.cs");
			var alwaysFailScript = GD.Load<Script>("res://addons/behaviortree/decorators/AlwaysFail.cs");
			var alwaysSucceedScript = GD.Load<Script>("res://addons/behaviortree/decorators/AlwaysSucceed.cs");
			var invertScript = GD.Load<Script>("res://addons/behaviortree/decorators/Invert.cs");
			var limitScript = GD.Load<Script>("res://addons/behaviortree/decorators/Limit.cs");
			var repeatScript = GD.Load<Script>("res://addons/behaviortree/decorators/Repeat.cs");
			var untilFailScript = GD.Load<Script>("res://addons/behaviortree/decorators/UntilFail.cs");
			var untilSuccessScript = GD.Load<Script>("res://addons/behaviortree/decorators/UntilSuccess.cs");
			AddCustomType("BehaviorTree", "Node", behaviorTreeScript, null);
			AddCustomType("Leaf", "Node", leafScript, null);
			AddCustomType("Parallel", "Node", parallelScript, null);
			AddCustomType("RandomSelector", "Node", randomSelectorScript, null);
			AddCustomType("RandomSequence", "Node", randomSequenceScript, null);
			AddCustomType("Selector", "Node", selectorScript, null);
			AddCustomType("Sequence", "Node", sequenceScript, null);
			AddCustomType("AlwaysFail", "Node", alwaysFailScript, null);
			AddCustomType("AlwaysSucceed", "Node", alwaysSucceedScript, null);
			AddCustomType("Invert", "Node", invertScript, null);
			AddCustomType("Limit", "Node", limitScript, null);
			AddCustomType("Repeat", "Node", repeatScript, null);
			AddCustomType("UntilFail", "Node", untilFailScript, null);
			AddCustomType("UntilSuccess", "Node", untilSuccessScript, null);
		}

		public override void _ExitTree()
		{
			RemoveCustomType("BehaviorTree");
			RemoveCustomType("Leaf");
			RemoveCustomType("Parallel");
			RemoveCustomType("RandomSelector");
			RemoveCustomType("RandomSequence");
			RemoveCustomType("Selector");
			RemoveCustomType("Sequence");
			RemoveCustomType("AlwaysFail");
			RemoveCustomType("AlwaysSucceed");
			RemoveCustomType("Invert");
			RemoveCustomType("Limit");
			RemoveCustomType("Repeat");
			RemoveCustomType("UntilFail");
			RemoveCustomType("UntilSuccess");
		}
	}
}
#endif
