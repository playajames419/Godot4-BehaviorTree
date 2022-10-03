using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace ProjectLife.addons.behaviortree;

public partial class Blackboard : Node
{
	/**
	 * This is the database where all your variables go. Here you keep track of
	 * whether the player is nearby, your health is low, there is a cover available,
	 * or whatever.
	 *
	 * You just store the data here and use it for condition checks in BTCondition scripts,
	 * or as arguments for your function calls in BTAction.
	 *
	 * This is a good way to separate data from behavior, which is essential
	 * to avoid nasty bugs.
	 */

	[Export] private Dictionary _data;

	public override void _EnterTree()
	{
		_data = _data.Duplicate();
	}

	public override void _Ready()
	{
		foreach (var key in _data.Keys) //TODO Key is never a string need to check this another way
			Debug.Assert(key is string, "Blackboard keys must be stored as strings.");
	}

	/**
	 * Updates or adds the blackboard with the provided key and value
	 *
	 * key = Contains the name the data will be stored upder in the blackboard
	 * value = Contains the data that will be associated with the key. (Variant)
	 * 
	 * Example:
	 *	set_data("health", 15)
	 */
	public void SetData(string key, Variant value)
	{
		_data[key] = value;
	}

	/**
	 * # Returns the blackboard data for the provided key
	 *
	 * key = Contains the name the data will be stored under in the blackboard
	 *
	 * Example
	 *	get("health")
	 *		=> 15
	 *
	 * Returns Variant
	 */

	public Variant GetData(string key)
	{
		if (!HasData(key))
			return null; //TODO cant return null Variant cant be null I guess

		var value = _data[key];

		if (value.IsEmpty() || GetTree().Root.HasNode(value))
		{
			_data[key] = null;
			return null;
		}
		else
		{
			return GetNode(value);
		}
	}

	/**
	 * # Public: Returns true if the key is present in the blackboard
	 *
	 * key = Contains the name the data will be stored upder in the blackboard
	 *
	 * Example
	 *	had_data("health")
	 *		
	 */

	public bool HasData(string key)
	{
		return _data.ContainsKey(key);
	}
}