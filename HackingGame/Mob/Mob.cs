using Godot;
using System;


public class Mob : Node2D
{
	public float Speed = 100;
	public float _delta;
	
	public TaskList _taskList;
	public TaskList taskList
	{
		set
		{
			//присваиваем и запускаем метод установки ссылок
			_taskList = value;
			_taskList._setMobLink(this);
		}
		
		get
		{
			return _taskList;
		}
	}
	
	
	
	public override void _Ready()
	{
		SetPhysicsProcess(false);
		
		
		//generator
		taskList = new TaskList();
		taskList.Add(new WaitTask(5));
		taskList.Add(new MoveTask(500,500));
		
	}
	public override void _PhysicsProcess(float delta)
	{
		_delta = delta;
		taskList.Act();
	}
}
