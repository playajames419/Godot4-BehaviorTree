using Godot;

namespace ProjectLife.addons.behaviortree.leafs;

[Tool]
public partial class Leaf : Task
{
    
    /**
     * Note:
     * The Leaf Node is the action object - it does stuff according to it’s current state.
     * Each Leaf Node will extend the Task class and call running(), success(), or fail().
     * It’s run() method will be called on every tick. A State Machine may be a good fit for
     * it’s run code structure unless it is doing something trivial. Here is code for a most
     * basic Leaf Node that simply reports success.
     */
    
    protected internal override void _run()
    {
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