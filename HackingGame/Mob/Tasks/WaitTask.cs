using Godot;
using System;

public class WaitTask : Task
{
	//время ожидания
	private float time;
	//нода в годоте
	private Timer timer;
	
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
		if (timer.IsStopped())
		{
			mob.RemoveChild(timer);
			return true;
		} else return false; 
		
	}
}
