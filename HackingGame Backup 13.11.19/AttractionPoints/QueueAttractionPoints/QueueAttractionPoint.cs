using Godot;
using System;

public class QueueAttractionPoint : AttractionPoint
{
  protected Queue _queue;
  protected QueueTimer timer;
  
  protected TextureProgress _timerTextureProgress;
  public override void _Ready()
  {
    timer = GetNode<QueueTimer>("QueueTimer");
    _queue = new Queue(this);
    SetPhysicsProcess(false);
  }

  protected override void OnAreaEntered(CashWalker cashWalker)
	{
    if (cashWalker.taskList.list[0].boundId == Id)
    {
      _queue.Add(cashWalker);
    }
      
	}


 
  protected void OnTimeOut()
  {
    _Executed(_queue.GetMob(0));
    _queue.Remove(0);
    timer.StopWithTexture();
  }

   public override void _PhysicsProcess(float delta)
  {
    if ( _queue.GetMob(0).taskList.list[0].GetType().Name == "WaitTask" )
    {
      timer.StartWithTexture();
      SetPhysicsProcess(false);
    }
  }
  
}
