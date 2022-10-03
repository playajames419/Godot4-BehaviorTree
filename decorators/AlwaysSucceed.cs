using Godot;

namespace ProjectLife.addons.behaviortree.decorators;

[Tool]
public partial class AlwaysSucceed : Task
{
    protected internal override void _run()
    {
        if (GetChildCount() > 0)
        {
            var child = GetChild(0) as addons.behaviortree.Task;
            child._run();
        }
        _success();
    }

    protected internal override void _childSuccess()
    {
    }

    protected override void _childFail()
    {
    }

    protected override void _childRunning()
    {
    }
}