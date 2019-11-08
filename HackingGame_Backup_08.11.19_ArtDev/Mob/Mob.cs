using Godot;
using System;


public class Mob : Node2D
{
	[Signal]
	public delegate void PropertyChanged(string propName, string propValue);
	[Export]
	public float Speed = 150;
	public float _delta;
	
	public TaskList _taskList;
	public TaskList taskList
	{
		set
		{
			//присваиваем и запускаем метод установки ссылок
			_taskList = value;
			_taskList._setMobLink(this);
			_taskList.Start();
		}
		
		get
		{
			return _taskList;
		}
	}
	
	
	
	public override void _Ready()
	{
		SetPhysicsProcess(false);

		if (taskList != null)
		{
			taskList.Start();
		}
			
	}
	public override void _PhysicsProcess(float delta)
	{
		_delta = delta;
		taskList.Act();
	}
	public Timer Say(string text)
	{
		MobMessage mobMessage = GetNode<MobMessage>("MobMessage");
		mobMessage.Text = text;
		mobMessage.View();
		return mobMessage.TimerClose;
	}
	protected virtual void OnVisionAreaEntered(CashWalker cashWalker)
	{
		GD.Print("i see, you, cashWalker");
	}
		protected virtual void OnVisionAreaEntered(Mob mob)
	{
		GD.Print("i see, you, mob");
	}

}
