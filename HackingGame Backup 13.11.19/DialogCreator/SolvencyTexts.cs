using Godot;
using System;
using System.Collections.Generic;
  ///<summary>
  ///Область с SolvencyText'ами
  ///</summary>
public class SolvencyTexts : VBoxContainer
{
  [Signal]
  public delegate void OnChanged();
  protected string _pathToModule = "res://DialogCreator/SolvencyText.tscn";
  protected PackedScene _loadModule;
  ///<summary>
  ///расстояние между элементами
  ///</summary>
  protected const float _indentations = 5;
  public override void _Ready()
  {
    _loadModule = (PackedScene)GD.Load(_pathToModule);
  }
  public int Count
  {
    get 
    {
      return GetChildCount();
    }
  }
  ///<summary>
  ///Добавление ноды
  ///</summary>
  public virtual void Add(int solvency, int IdLocale)
  {
    Control SolvencyText = (Control)_loadModule.Instance();
    SolvencyText.GetNode<SpinBox>("Solvency").Value = solvency;
    SolvencyText.GetNode<LineEditSignal>("Text").Text = PhraseLocale.GetText(IdLocale);
    SolvencyText.GetNode<LineEditSignal>("Text").IdLocale = IdLocale;
    AddChild(SolvencyText);
  }
  ///<summary>
  ///Удаление всех нод
  ///</summary>
  public void RemoveAll()
  {
    while (Count > 0)
    {
      Remove(0);
    }
  }
  public void Remove(Control child)
  {
    child.QueueFree();
    RemoveChild(child);
  }
  public void Remove(int id)
  {
    GetChild(id).QueueFree();
    RemoveChild(GetChild(id));
  }
  public Dictionary<int, int> GetData()
  {
    Dictionary<int, int> outST = new Dictionary<int, int>();
    foreach(Control solvencyText in GetChildren())
    {
      int Key = (int)solvencyText.GetNode<SpinBox>("Solvency").Value;
      int Value = solvencyText.GetNode<LineEditSignal>("Text").IdLocale;
      PhraseLocaleDev.SetText(Value, solvencyText.GetNode<LineEditSignal>("Text").Text);
      
      //Предотвращение одинаковых ключей
      while(outST.ContainsKey(Key))
      {
        Key++;
      }
      outST.Add(Key, Value);
    }
    return outST;
  }
}
