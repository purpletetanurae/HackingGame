using Godot;
using System;

public class WaitTask : Task
{
	//время ожидания
	private float time;
	//нода в годоте
	private Timer timer;
	
	//бессконечно ждать
	public WaitTask()
	{
		time = 300;
	}
	//ждать, time в секундах
	public WaitTask(int time)
	{
		this.time = time;
	}
	
	public override void Start()
	{
		//создаем экземпляр ноды
		//устанавливаем, что срабатывать нужно только раз
		//и ставим время
		timer = new Timer();
		timer.OneShot = true;
		timer.WaitTime = time;
		
		//прикрепляем ноду к Мобу и стартуем
		mob.AddChild(timer);
		timer.Start();
	}
	//помимо условия остановки необходимо
	//удалять таймер, чтобы незасорился моб
	public override bool Trigger()
	{
		return timer.IsStopped();
	}
	
	public override void End()
	{
		mob.RemoveChild(timer);
		timer.QueueFree();
		
	}
}
