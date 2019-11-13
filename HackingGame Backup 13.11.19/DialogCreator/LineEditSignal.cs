using Godot;
using System;

public class LineEditSignal : LineEdit
{
  public int IdLocale;
  public override void _Ready()
  {
    Connect("text_changed",this, "OnValueChanged");
  }

  private void OnValueChanged(string value)
  {
    GetTree().CallGroup("Save", "PrepareForSave");
  }
}
