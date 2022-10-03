using Godot;
namespace ProjectLife.addons.behaviortree.decorators;

[Tool]
public partial class Limit : Task
{

    // Will fail if the child task succeeds and gets called again too many times
    
    // Maximum number of times to run
    [Export] private int _limit = 4;

    private int _count;
    
    protected internal override void _run()
    {
        var child = GetChild(0) as addons.behaviortree.Task;
        child._run();
        _running();
    }

    protected internal override void _childSuccess()
    {
        _count += 1;
        if (_count >= _limit)
        {
            _count = 0;
            _fail();
        }
        else
        {
            _success();
        }
    }

    protected override void _childFail()
    {
        _count = 0;
        _fail();
    }

    protected override void _childRunning()
    {
    }
    
    protected override void _cancel()
    {
        _count = 0;
        base._cancel();
    }

    protected override void _start()
    {
        _count = 0;
        base._start();
    }
}