using Godot;
using System;

public class CheckBoxSignal : CheckBox
{
  public override void _Ready()
  {
    Connect("pressed",this, "OnValueChanged");
  }

  private void OnValueChanged()
  {
    GetTree().CallGroup("Save", "PrepareForSave");
  }
}
