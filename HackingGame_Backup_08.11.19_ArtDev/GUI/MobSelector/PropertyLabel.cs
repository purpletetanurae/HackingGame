using Godot;
using System;

public class PropertyLabel : Control
{
  private string _propViewName;
  public string PropViewName
  {
    set
    {
      _propViewName = value;
      if (IsInsideTree())
      {
        GetNode<Label>("Name").Text = _propViewName;
      }
    }
    get
    {
      return _propViewName;
    }
  }
  private string _propName;
  public string PropName
  {
    set
    {
      _propName = value;
      
    }
    get
    {
      return _propName;
    }
  }
  private string _propValue;
  public string PropValue
  {
    set
    {
      _propValue = value;
      if (IsInsideTree())
      {
        GetNode<Label>("Value").Text = _propValue;
      }
    }
    get
    {
      return _propValue;
    }
  }
  public override void _Ready()
  {
    GetNode<Label>("Name").Text = _propViewName;
    GetNode<Label>("Value").Text = _propValue;
  }
  public void NextAnimation()
  {
    if (HasNode("../.."))
    {
      GetNode<MobSelector>("../..").NextAnimation();
    }

  }
  public void SetValues(string propName, string propViewName, string propValue, Vector2 position)
  {
    PropName = propName;
    PropViewName = propViewName;
    PropValue = propValue;
    SetPosition(position);
  }
  public void SetValues(string propName, string propValue, Vector2 position)
  {
    SetValues(propName, propName, propValue, position);
  }
  public void SetValues(PropertyLabel propLabel)
  {
    SetValues(propLabel.PropName, propLabel.PropViewName, propLabel.PropValue, propLabel.GetPosition());
  }

  
}
