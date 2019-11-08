using Godot;
using System;

public class TimeLabel : Label
{ 
  public string Message = "No_message";

  public override void _Ready()
  {
    SetText(Message);
    GetNode<AnimationPlayer>("Anim").Play("Vanish");
  }
}
