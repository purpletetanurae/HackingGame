using Godot;
using System;

public class SpinBoxSignal : SpinBox
{
  public override void _Ready()
  {
    Connect("value_changed",this, "OnValueChanged");
  }

  private void OnValueChanged(float value)
  {
    GetTree().CallGroup("Save", "PrepareForSave");
  }
}
