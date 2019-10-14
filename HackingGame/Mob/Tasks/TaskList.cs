using Godot;
using System;
using System.Collections.Generic;

public class TaskList
{
	
	private List<Task> list = new List<Task>();
	private int _length = 0;
	//ссылка на моба
	public Mob mob;
	
	//кол-во задач
	public int Length
	{
		get
		{
			return _length;
		}
	} 
	
	//переход к следующей задаче
	public void Next()
	{
		//удаляем текущую задачу
		list.RemoveAt(0);
		
		//если задачи остались, то распаковываем следующую
		//иначе останавливаем процесс
		if (list.Count > 0)
		{
			list[0].Start();
		}
		else 
		{
			mob.SetPhysicsProcess(false);
		}
	}
	
	//что работает в процессе
	//если сработал триггер то переходим к следующей задаче
	public void Act()
	{
		if (list.Count > 0)
			list[0].Act();
			
		if (list[0].Trigger())
			Next();
	}
	
	//добавление новой задачи
	//если задача первая то распаковываем её сразу и запускаем процесс
	//замечу, что метод необходимо дописать, чтобы
	//TaskList собирать вне Mob'a
	public void Add(Task task)
	{
		task.mob = mob;
		list.Add(task);
		
		if (_length == 0)
		{
			list[0].Start();
			mob.SetPhysicsProcess(true);
		}
			
		
		_length++;
	}
	
	//не рекомендуется использовать как интерфейс
	//установка ссылок на моба
	//метод необходим для сеттера
	public void _setMobLink(Mob newMob)
	{
		this.mob = newMob;
		for (int i = 0; i < _length; i++)
			list[i].mob = newMob;
	}
}
