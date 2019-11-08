using Godot;
using System;
  ///<summary>
  ///Область с PossiblePhrase'ами
  ///</summary>
public class PossiblePhrases : SolvencyTexts
{
  public override void _Ready()
  {
    _pathToModule = "res://DialogCreator/PossiblePhrase.tscn";
    base._Ready();
  }
  ///<summary>
  ///Добавление ноды PossiblePhrase
  ///</summary>
  public void Add(int id, string text, double chance)
  {
    Control SolvencyText = (Control)_loadModule.Instance();
    SolvencyText.GetNode<Label>("Id").Text = id.ToString();
    SolvencyText.GetNode<Label>("Text").Text = text;
    SolvencyText.GetNode<SpinBox>("Chance").Value = (float)chance;
    SolvencyText.SetPosition(new Vector2(0, Count * (_indentations + 40)));
    AddChild(SolvencyText);
  }

}
