using Godot;

namespace ProjectLife.addons.behaviortree.decorators;

[Tool]
public partial class UntilSuccess : Task
{
    // Only reports a success
    protected internal override void _run()
    {
        var child = GetChild(0) as Task;
        child._run();
        _running();
    }

    protected internal override void _childSuccess()
    {
        _success();
    }

    protected override void _childFail()
    {
    }

    protected override void _childRunning()
    {
    }
}