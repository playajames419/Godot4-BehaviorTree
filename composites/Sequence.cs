using System.Diagnostics;
using Godot;

namespace ProjectLife.addons.behaviortree.composites;

[Tool]
public partial class Sequence : Task
{
    // All children must run successfully

    private int _currentChild;

    public override void _Ready()
    {
        if (GetChildCount() < 1) return; //TODO Maybe assert this? Possibly not if children are dynamic and can be added at runtime?
        foreach (var c in GetChildren())
            Debug.Assert(c is Task, "Children of " + Name + " must be of type Task.cs");
        
    }
    
    protected internal override void _run()
    {
        var child = GetChild(_currentChild) as Task;
        if (child == null)
            Control.
        child!._run();
        _running();
    }

    protected internal override void _childSuccess()
    {
        _currentChild += 1;
        if (_currentChild < GetChildCount()) return;
        _currentChild = 0;
        _success();
    }

    protected override void _childFail()
    {
        _currentChild = 0;
        _fail();
    }

    protected override void _childRunning()
    {
        // return
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