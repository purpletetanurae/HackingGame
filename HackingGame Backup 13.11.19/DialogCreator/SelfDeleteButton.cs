using Godot;
using System;

public class SelfDeleteButton : Button
{
  private void OnPressed()
  {
    GetTree().CallGroup("Save", "PrepareForSave");
    GetNode<SolvencyTexts>("../..").Remove(GetNode<Control>("..")); 
  }
}
