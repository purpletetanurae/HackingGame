using Godot;
using System;

public class Task : Node
{
	public Mob mob;
	public int boundId = 0;
	
	//срабатывает при анбоксинга из тасклиста
	public virtual void Start() { }	
	
	//срабатывает в процессе
	public virtual void Act() { }	
	
	//срабатывает в завершении процесса
	public virtual void End() { }
	
	//триггер, если тру, то переходим к следующей задаче
	public virtual bool Trigger() { return true; }
}
