using Godot;
using System;

public class MessageToUser : Control
{
  public void ShowError(string text)
  {
    GetNode<Label>("Label").Text = text;
    GetNode<AnimationPlayer>("AnimationPlayer").Play("ShowError");
  }
  public void Show(string text)
  {
    GetNode<Label>("Label").Text = text;
    GetNode<AnimationPlayer>("AnimationPlayer").Play("ShowGoodMessage");
  }
}
