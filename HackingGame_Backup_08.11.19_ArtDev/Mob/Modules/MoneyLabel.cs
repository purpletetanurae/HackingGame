using Godot;
using System;

public class MoneyLabel : Label
{
  public int MoneyChanges = 0;

  public override void _Ready()
  {
    SetText(ConvertChangesToString());
    GetNode<AnimationPlayer>("Anim").Play("Hide");
  }
  private string ConvertChangesToString()
  {
    string message;

    if (MoneyChanges > 0)
    {
      message = "+" + MoneyChanges.ToString() + "$";
    }
    else
    {
      message = "-" + (Math.Abs(MoneyChanges)).ToString() + "$";
    }

    return message;
  }
}
