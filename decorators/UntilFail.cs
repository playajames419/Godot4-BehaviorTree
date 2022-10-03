using Godot;

namespace ProjectLife.addons.behaviortree.decorators;

[Tool]
public partial class UntilFail : Task
{
    // Only reports a failure
    protected internal override void _run()
    {
        var child = GetChild(0) as Task;
        child._run();
        _running();
    }

    protected internal override void _childSuccess()
    {
    }

    protected override void _childFail()
    {
        _success();
    }

    protected override void _childRunning()
    {
    }
}