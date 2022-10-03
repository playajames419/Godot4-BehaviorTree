using System;
using Godot;
using Range = Godot.Range;

namespace ProjectLife.addons.behaviortree.composites;

[Tool]
public partial class RandomSequence : Task
{
    
    // All randomly selected children must succeed
    //TODO Implement this from https://github.com/andrew-wilkes/godot-behaviour-tree/blob/master/BehaviorTree/Composites/RandomSequence.gd
    
    private Range _sequence;
    private int _index;

    public override void _Ready()
    {
        throw new NotImplementedException();
        _setSequence();
    }

    private void _setSequence()
    {
        throw new NotImplementedException();
        _index = 0;
        _sequence = new Range(); // TODO Figure out how to use range or use a random number sort of thing
        _sequence.MaxValue = GetChildCount();
    }

    protected internal override void _run()
    {
        //var child = GetChild(_sequence[_index]) as Task;
        //child._run();
        _running();
    }

    protected internal override void _childSuccess()
    {
        _index += 1;
        //if (_index < _sequence.Size()) return;
        _setSequence();
        _success();
    }

    protected override void _childFail()
    {
        _setSequence();
        _fail();
    }

    protected override void _childRunning()
    {
    }

    protected override void _cancel()
    {
        _index = 0;
        base._cancel();
    }

    protected override void _start()
    {
        _index = 0;
        base._start();
    }
}