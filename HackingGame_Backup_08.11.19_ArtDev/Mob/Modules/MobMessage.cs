using Godot;
using System;

public class MobMessage : Control
{
  private const float _timeOneSignText = (float)0.15;
  private string _text;
  public Timer TimerClose;
  public override void _Ready()
  {
    TimerClose = GetNode<Timer>("Timer");
  }
  public string Text
  {
    set
    {
      GetNode<Label>("Panel/Label").Text = value;
      _text = value;
      SetWaitTimeByTextLength();
    }
    get
    {
      return _text;
    }
  }
  public void View()
  {
    Visible = true;
    TimerClose.Start();
  }
  public void UnView()
  {
    Visible = false;
  }
  private void SetWaitTimeByTextLength()
  {
    TimerClose.WaitTime = 5 * _timeOneSignText + (float)_text.Length * _timeOneSignText;
  }
  private void OnTimerTimeout()
  {
    UnView();
  }
}
