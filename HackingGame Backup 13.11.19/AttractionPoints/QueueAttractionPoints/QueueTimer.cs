using Godot;
using System;

public class QueueTimer : Timer
{
  private TextureProgress _textureProgress;
  public override void _Ready()
  {
    SetPhysicsProcess(false);
    _textureProgress = GetNode<TextureProgress>("../TextureProgress");
  }
  public void StartWithTexture()
  {
    Start();
    SetPhysicsProcess(true);
  }
  public override void _PhysicsProcess(float delta)
  {
    _textureProgress.Value = (TimeLeft / WaitTime) * 100;
  }
  public void StopWithTexture()
  {
    Stop();
    SetPhysicsProcess(false);
    _textureProgress.Value = 0;
  }
}
