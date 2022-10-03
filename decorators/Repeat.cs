using Godot;
namespace ProjectLife.addons.behaviortree.decorators;

[Tool]
public partial class Repeat : Task
{
    // Repeats the child Task and reports Success after repeating unless it fails
    // This code counts successful repetitions of the child Node until it stops repeating and reports success.
    
    // Number of times to run or zero for infinite
    [Export] private int _limit = 5;

    private int _count;
    private bool _repeating;
    
    protected internal override void _run()
    {
        if (!_repeating)
        {
            _repeating = true;
            var child = GetChild(0) as addons.behaviortree.Task;
            child._run();
        }
        _running();
    }

    protected internal override void _childSuccess()
    {
        if (_limit > 0)
        {
            _count += 1;
            if (_count >= _limit)
            {
                _count = 0;
                _repeating = false;
                _success();
            }
        }

        if (_repeating)
        {
            var child = GetChild(0) as addons.behaviortree.Task;
            child._run();
        }
    }

    protected override void _childFail()
    {
        _repeating = false;
        _fail();
    }

    protected override void _childRunning()
    {
    }
    
    protected override void _cancel()
    {
        _count = 0;
        _repeating = false;
        base._cancel();
    }

    protected override void _start()
    {
        _count = 0;
        _repeating = false;
        base._start();
    }
    
}