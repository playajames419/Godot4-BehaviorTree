using Godot;

namespace ProjectLife.addons.behaviortree.composites;

[Tool]
public partial class Parallel : Task
{
    
    //TODO implement from https://github.com/andrew-wilkes/godot-behaviour-tree/blob/master/BehaviorTree/Composites/Parallel.gd
    
    protected internal override void _run()
    {
        throw new System.NotImplementedException();
    }

    protected internal override void _childSuccess()
    {
        throw new System.NotImplementedException();
    }

    protected override void _childFail()
    {
        throw new System.NotImplementedException();
    }

    protected override void _childRunning()
    {
        throw new System.NotImplementedException();
    }
}