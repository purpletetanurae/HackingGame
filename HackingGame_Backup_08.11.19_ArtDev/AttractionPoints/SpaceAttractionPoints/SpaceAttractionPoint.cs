using Godot;
using System;


public class SpaceAttractionPoint : AttractionPoint
{
  [Export]
  public int SpawnRadious = 200;
  protected Position2D _spawnCenter;
  public override void _Ready()
  {
    _spawnCenter = GetNode<Position2D>("Position2D");
  }
  protected override void OnAreaEntered(Mob mob)
	{
    if (mob.taskList.list[0].boundId == Id)
    {
      mob.taskList.Interception(new MoveTask(GetRandomPosition()));
      mob.taskList.Insert(new WaitTask(_rand.Next(3,7)), 1);
    }
	}
  
  
  protected Vector2 GetRandomPosition()
  {
    return _spawnCenter.GetGlobalPosition() + GetRandomNormalizeVector() * (float)_rand.Next(0,SpawnRadious);
  }
  protected Vector2 GetRandomNormalizeVector()
  {
    Vector2 ranVec = new Vector2();
    ranVec.x = (float)_rand.Next(-50,50);
    ranVec.y = (float)_rand.Next(-50,50);
    return ranVec.Normalized();
  }

  
}
