using Godot;

namespace ProjectLife.addons.behaviortree.composites;

[Tool]
public partial class Selector : Task
{
    
    // One of the children must run successfully

    private int _currentChild;
    
    protected internal override void _run()
    {
        var child = GetChild(_currentChild) as Task;
        child._run();
        _running();
    }

    protected internal override void _childSuccess()
    {
        _currentChild = 0;
        _success();
    }

    protected override void _childFail()
    {
        _currentChild += 1;
        if (_currentChild < GetChildCount()) return;
        _currentChild = 0;
        _fail();
    }

    protected override void _childRunning()
    {
    }

    protected override void _cancel()
    {
        _currentChild = 0;
        base._cancel();
    }

    protected override void _start()
    {
        _currentChild = 0;
        base._start();
    }
}