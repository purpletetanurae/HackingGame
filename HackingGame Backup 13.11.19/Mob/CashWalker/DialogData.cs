using Godot;
using System;
using System.Collections.Generic;

public struct DialogData
{
  private Dictionary<string, string> _answers;
  private List<CashWalker> _ignore;

  public DialogData(CashWalker connectedMob)
  {
    _answers = new Dictionary<string, string>();
    _ignore = new List<CashWalker>();
    AddIgnore(connectedMob);
  }
  public void AddIgnore(CashWalker mob)
  {
    _ignore.Add(mob);
  }
  public bool IsIgnore(CashWalker mob)
  {
    return _ignore.Contains(mob);
  }
  public void AddAnswer(string name, string value)
  {
    _answers.Add(name, value);
  }
  public string GetAnswer(string name)
  {
    if (_answers.ContainsKey(name))
    {
      return _answers[name];
    }
    else
    {
      return "none";
    }
  }
}
