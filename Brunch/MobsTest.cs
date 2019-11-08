using Godot;
using System;

public class MobsTest : Node2D
{
  public override void _Ready()
  {
    var scene = GD.Load<PackedScene>("res://Mob/Mob.tscn");
    var node = scene.Instance();
    AddChild(node);
  }
}
