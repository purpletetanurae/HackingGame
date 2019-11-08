using Godot;
using System;

public class Main3 : Node2D
{
  public override void _Ready()
  {
    TaskList tasks = new TaskList();

    TaskList tasks2 = new TaskList();
    tasks2.Add(null);
    tasks2.Add(new WaitTask());

    tasks += tasks;
  }
}
