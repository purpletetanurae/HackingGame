using Godot;
using System;

public class MoveTask : Task
{
	protected Vector2 EndPosition; 
	protected Navigation2D _navigation;
	protected Vector2[] _path;
	protected int _idx = 0;
	protected float _stoppedDistance = 5;

	/// <summary>
	///Двигаться к мобу
	/// </summary>
	public MoveTask(Mob mob)
	{
		EndPosition = mob.Position;
	}
	/// <summary>
	///Двигаться к точке притяжения
	/// </summary>
	public MoveTask(AttractionPoint attractionPoint)
	{
		EndPosition = attractionPoint.Position;
		boundId = attractionPoint.Id;
	}
	/// <summary>
	///Двигаться к точке
	/// </summary>
	public MoveTask(Vector2 moveVec)
	{	
		EndPosition = moveVec;
	}	
	/// <summary>
	/// Двигаться в точку moveVec, недоходя на расстояние stoppedDistance
	/// </summary>
	public MoveTask(Vector2 moveVec, float stoppedDistance)
	{	
		EndPosition = moveVec;
		_stoppedDistance = stoppedDistance;
	}	
	/// <summary>
	///Двигаться к точке по координатам
	/// </summary>
	public MoveTask(float x, float y)
	{
		EndPosition = new Vector2(x,y);
	}
	
	
	public override void Start() 
	{
		_navigation = mob.GetNode<Navigation2D>("/root/Main/Navigation2D");
		_path = _navigation.GetSimplePath(mob.Position, EndPosition, true);
	}	
	public override void Act() 
	{
		float distance = mob.Position.DistanceTo(_path[_idx + 1]);
		RotateByMoving();
		if (distance > _stoppedDistance)
		{
			mob.Position = mob.Position.LinearInterpolate(_path[_idx + 1], (mob.Speed * mob._delta) / distance);
			mob.ZIndex = (int)mob.Position.y;
		}
		else 
		{
			_idx++;
		}
		
	}
	protected void RotateByMoving()
	{
		AnimatedSprite animSprite =	mob.GetNode<AnimatedSprite>("AnimatedSprite");
		Area2D vision = mob.GetNode<Area2D>("Vision");

		if ((_path[_idx + 1].x - mob.Position.x) < 0)
		{
			animSprite.FlipH = true;
			vision.Scale = new Vector2(-1, 1);
		}
		else
		{
			animSprite.FlipH = false;
			vision.Scale = new Vector2(1, 1);
		}
	}
	public override bool Trigger() 
	{
		//если путь больше единицы то тригеру сообщаем тру
		return (_idx + 2) > _path.Length;
	}
	
	public override void End()
	{
	} 
}
