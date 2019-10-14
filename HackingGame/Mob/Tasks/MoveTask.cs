using Godot;
using System;

public class MoveTask : Task
{
	private Vector2 EndPosition; 
	private Path2D Path;
	private PathFollow2D PathFollow;
	
	public MoveTask(float x, float y)
	{
		EndPosition = new Vector2(x,y);
	}
	
	public override void Start() 
	{
		//создаем ссылки на необходимые свойства
		Path = mob.GetNode<Path2D>("Path2D");
		PathFollow = mob.GetNode<PathFollow2D>("Path2D/PathFollow2D");
		
		//устанавливаем начальную и конечную точку
		Path.GetCurve().AddPoint(mob.GetNode<KinematicBody2D>("KinematicBody2D").Position);
		Path.GetCurve().AddPoint(EndPosition);
	}	
	public override void Act() 
	{
		//передвигаемся
		PathFollow.Offset += mob.Speed * mob._delta;
	}
	
	public override bool Trigger() 
	{
		//если путь больше единицы то тригеру сообщаем тру
		return PathFollow.UnitOffset >= 1;
	}
}
