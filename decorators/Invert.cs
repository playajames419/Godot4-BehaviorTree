using System.Diagnostics;
using Godot;

namespace ProjectLife.addons.behaviortree.decorators;

[Tool]
public partial class Invert : Task
{
    // This must have a child Node that provides status updates.
    // Invert the result
    protected internal override void _run()
    {
        var child = GetChild(0) as addons.behaviortree.Task;
        Debug.Assert(child != null, nameof(child) + " != null");
        child._run();
        _running();
    }

    protected internal override void _childSuccess()
    {
        _fail();
    }

    protected override void _childFail()
    {
        _success();
    }

    protected override void _childRunning()
    {
    }
}