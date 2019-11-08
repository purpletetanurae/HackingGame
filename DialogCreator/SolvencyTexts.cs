using Godot;
using System;
  ///<summary>
  ///Область с SolvencyText'ами
  ///</summary>
public class SolvencyTexts : Control
{
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
  public virtual void Add(int solvency, string text)
  {
    Control SolvencyText = (Control)_loadModule.Instance();
    SolvencyText.GetNode<SpinBox>("Solvency").Value = solvency;
    SolvencyText.GetNode<LineEdit>("Text").Text = text;
    // 40 - сделано криво, потому что не получалось получить размеры ноды(((
    SolvencyText.SetPosition(new Vector2(0, Count * (_indentations + 40)));
    AddChild(SolvencyText);
  }
  ///<summary>
  ///Удаление всех нод
  ///</summary>
  public void RemoveAll()
  {
    while (Count > 0)
    {
      GetChild(0).QueueFree();
      RemoveChild(GetChild(0));
    }
  }
}
