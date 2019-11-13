using Godot;
using System;
using System.Collections.Generic;

public class ChangerPossiblePhrases : SolvencyTexts
{
  public override void _Ready()
  {
    _pathToModule = "res://DialogCreator/DCHeartStone/PossiblePhraseButton.tscn";
    base._Ready();
  }
  public void Add(int id, string text)
  {
    PossiblePhraseButton possiblePhraseButton = (PossiblePhraseButton)_loadModule.Instance();
    possiblePhraseButton.Text = text;
    possiblePhraseButton.idPhrase = id;
    AddChild(possiblePhraseButton);
  }
}
