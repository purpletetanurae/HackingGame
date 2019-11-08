using Godot;
using System;
using System.Collections.Generic;
/*
  неприятная работа mouse_entered и mouse_exited
 */
public class MobSelector : Control
{
  [Signal]
  public delegate void Closed();
  public List<PropertyLabel> Propeties = new List<PropertyLabel>();
  private int _maxCountProperties = 4;
  private int _nowAnimation = 0;
  private bool _isClosingState = false;
  public bool Focused {get; private set;} = false;
  private PackedScene _loadPropertyLabel;
  public Mob FollowMob
  {
    set
    {
      _followMob = value;
      _followMob.Connect("tree_exiting", this, nameof(OnMobTreeExiting));
      _followMob.Connect("PropertyChanged", this, nameof(OnMobPropertyChanged));
      SetPhysicsProcess(true);
    }
    get
    {
      return _followMob;
    }
  }
  private Mob _followMob = null;
  //расстояние от центра моба до того где должна быть метка
  //сделано на отъебись
  private Vector2 _correctPosition = new Vector2(-89, -255);

  

  public override void _Ready()
  {
    _loadPropertyLabel = (PackedScene)GD.Load("res://GUI/MobSelector/PropertyLabel.tscn");
    InstanceProperties();
    GetNode<AnimationPlayer>("Anim").Play("Show");
  }

  public override void _PhysicsProcess(float delta)
  {
    if (_followMob == null)
    {
      SetPhysicsProcess(false);
      return;
    } 
    SetGlobalPosition(_followMob.GetGlobalPosition() + _correctPosition);
  }
  public void NextAnimation()
  {
    //если анимация идёт в обратную сторону
    if (_isClosingState)
    {
      CloseAnimation();
      return;
    }


    if (_nowAnimation < Propeties.Count)
    {
      Propeties[_nowAnimation].GetNode<AnimationPlayer>("Anim").Play("Show");
      _nowAnimation++;
    }
    else
    {
      GetNode<AnimationPlayer>("Button/Anim").Play("Show");
    }
  }
  private void CloseAnimation()
  {
    if (_nowAnimation >= 0)
    {
      Propeties[_nowAnimation].GetNode<AnimationPlayer>("Anim").Play("Hide");
      _nowAnimation--;
    }
    if (_nowAnimation == -1)
    {
      GetNode<AnimationPlayer>("Anim").Play("Hide");
      _nowAnimation--;
    }
  }

  //неприятная реализация
  private void InstanceProperties()
  {
    for (int i = 0; i < Propeties.Count; i++)
    {
      InstanceProperty(i);
    }
  }
  public void AddProperty(string propName, string propValue)
  {
    if (Propeties.Count == _maxCountProperties)
    {
      GD.Print("Максимальное кол-во свойств в Mob.Selector.Propeties = " + _maxCountProperties);
      return;
    }

    PropertyLabel propLabel = new PropertyLabel();
    propLabel.SetValues(propName, propValue, new Vector2(7,10) + new Vector2(0, 30) * Propeties.Count);
    Propeties.Add(propLabel);

    if (IsInsideTree())
    {
      InstanceProperty(Propeties.Count - 1);
    }
  }

  private void InstanceProperty(int idx)
  {
    PropertyLabel propLabel = (PropertyLabel)_loadPropertyLabel.Instance();
    propLabel.SetValues(Propeties[idx]);
    GetNode<Control>("Propeties").AddChild(propLabel);
    Propeties[idx] = propLabel;
  }
  public void RefreshPropeties()
  {
    if (!HasNode(".."))
    {
      GD.Print("Не найден MobController");
      return;
    }
    GetParent<MobController>().SetPropertyMobSelector(FollowMob);
  }
  //использовать после инстансирования
  private void SetSpeedAnimation(float speed)
  {
    GetNode<AnimationPlayer>("Anim").SetSpeedScale(speed);
    GetNode<AnimationPlayer>("Button/Anim").SetSpeedScale(speed);
    foreach (PropertyLabel propLabel in GetNode<Node>("Propeties").GetChildren())
    {
      propLabel.GetNode<AnimationPlayer>("Anim").SetSpeedScale(speed);
    }
  }
  public void Close()
  {
    EmitSignal("Closed");
    _isClosingState = true;
    _nowAnimation--;
    SetSpeedAnimation((float)1.5);
    GetNode<AnimationPlayer>("Button/Anim").Play("Hide");
  }
  public void OnAnimFinish(string animName)
  {
    if (_isClosingState)
    {
      QueueFree();
    }
  }
  public void OnMouseFocusEntered()
  {
    Focused = true;
  }
  public void OnMouseFocusExited()
  {
    Focused = false;
  }
  public void _on_Button_pressed()
  {
    if (FollowMob.GetType() == typeof(CashWalker))
    {
      CashWalker LohMob = (CashWalker)FollowMob;
      LohMob.Money = 0;
      Close();
    }
  }
  private void OnMobTreeExiting()
  {
    SetPhysicsProcess(false);
    Close();
  }
  private void OnMobPropertyChanged(string propName, string propValue)
  {
    GD.Print("OnMobPropertyChanged");
    PropertyLabel propLabel = Propeties.Find(prop => prop.PropName == propName);
    if (propLabel != null)
    {
      propLabel.PropValue = propValue;
    }
    
  }
}
