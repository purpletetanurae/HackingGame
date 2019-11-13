using Godot;
using System;
using System.Collections.Generic;
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
    Control PossiblePhrase = (Control)_loadModule.Instance();
    PossiblePhrase.GetNode<Label>("Id").Text = id.ToString();
    PossiblePhrase.GetNode<Label>("Text").Text = text;
    PossiblePhrase.GetNode<SpinBox>("Chance").Value = (float)chance;
    //PossiblePhrase.SetPosition(new Vector2(0, Count * (_indentations + 40)));
    AddChild(PossiblePhrase);
  }
  public new Dictionary <int, double> GetData()
  {
    Dictionary <int, double> outPP = new Dictionary <int, double>();
    foreach(Control PossiblePhrase in GetChildren())
    {
      int Key = PossiblePhrase.GetNode<Label>("Id").Text.ToInt();
      double Value = (double)PossiblePhrase.GetNode<SpinBox>("Chance").Value;
      outPP.Add(Key, Value);
    }
    return PhraseChainDev.NormalizePossiblePhrase(outPP);
  }


}
