using Godot;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class DialogTesting : Node2D
{
  public override void _Ready()
  {

    CashWalker CW2 = GetNode<CashWalker>("CashWalker2");
    CashWalker CW1 = GetNode<CashWalker>("CashWalker");
    CashWalker CW3 = GetNode<CashWalker>("CashWalker3");

    TaskList TL = new TaskList();
    CW1.taskList = new TaskList();
    CW2.taskList = new TaskList();
    CW3.taskList = new TaskList();

    CW1.taskList.Add(new MoveTask(CW3));
    CW3.taskList.Add(new MoveTask(CW1));

    
    CW2.taskList.Add(new MoveTask(300, 300));
  }
}
