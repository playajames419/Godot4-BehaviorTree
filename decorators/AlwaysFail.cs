using Godot;

namespace ProjectLife.addons.behaviortree.decorators;

[Tool]
public partial class AlwaysFail : Task
{
    // A child Node is optional for this Node, but if it exists, we call it’s run() method.
    protected internal override void _run()
    {
        if (GetChildCount() <= 0) return;
        
        var node = GetChild(0);
        if (node is addons.behaviortree.Task child)
            child._run();
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