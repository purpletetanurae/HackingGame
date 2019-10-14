using Godot;
using System;

public class Task : Node
{
	public Mob mob;
	
	//срабатывает при анбоксинга из тасклиста
	public virtual void Start() { }	
	
	//срабатывает в процессе
	public virtual void Act() { }	
	
	//триггер, если тру, то переходим к следующей задаче
	public virtual bool Trigger() { return true; }
}
