using Godot;
using System;
using System.Collections.Generic;
public class Main : Node2D
{
  private AttractionPoint Qap;
  private AttractionPoint Qap2;
  private AttractionPoint Sap;
  private Random _rand = new Random();
  private Node2D _mobsPack;
  private PackedScene _load;
  private Dictionary <int, string> _idColors = new Dictionary <int, string>();
  private Timer _timer;
  private Label _label;
  public override void _Ready()
  {

    _timer = GetNode<Timer>("AttractionPoints/AttractionPoint/QueueTimer");
    _mobsPack = GetNode<Node2D>("Mobs");


    Sap = GetNode<SpaceAttractionPoint>("AttractionPoints/SpaceAttractionPoint");
    Qap = GetNode<QueueAttractionPoint>("AttractionPoints/AttractionPoint");
    Qap2 = GetNode<QueueAttractionPoint>("AttractionPoints/QueueAttractionPoint");


    _load = (PackedScene)GD.Load("res://Mob/Mob.tscn");
    _label = GetNode<Label>("fps");



 
  }
  public void _on_MobTimer_timeout()
  {

    InstanceMob(_rand.Next(120));
  }
  public override void _PhysicsProcess(float delta)
  {
    _label.SetText(Engine.GetFramesPerSecond().ToString());
  }

  private void InstanceMob(int randValue)
  {
    Mob _instanceMob = (Mob)_load.Instance();
    Vector2 home = new Vector2((float)(1024*_rand.Next(2)), _rand.Next(50,500));

    _instanceMob.Position = home;

    home = new Vector2((float)(1024*_rand.Next(2)), _rand.Next(50,500));
    TaskList taskList = new TaskList();

    if (randValue > 100)
    {
      _instanceMob.SetModulate(new Color("ff2cd8"));
      taskList.Add(new MoveTask(Sap));
    }
    else if (randValue > 70)
    {
      taskList.Add(new MoveTask(Qap));
    } 
    else if (randValue > 40)
    {
      taskList.Add(new MoveTask(Qap2));
      _instanceMob.SetModulate(new Color("e6009c"));
    }
    else if (randValue > 20)
    {
      taskList.Add(new MoveTask(Qap));
      taskList.Add(new MoveTask(Qap2));
      _instanceMob.SetModulate(new Color("000000"));
    }
    else
    {
      
      _instanceMob.SetModulate(new Color("dc0000"));
      
    }
    taskList.Add(new MoveTask(home));
    _instanceMob.taskList = taskList;
    _mobsPack.AddChild(_instanceMob);
  }
}
