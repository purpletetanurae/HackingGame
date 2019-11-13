using Godot;
using System;
using System.Collections.Generic;

public class PossiblePhraseButton : Button
{
  public int idPhrase;

  private void OnClick()
  {
    GetNode<DCHeartStone>("../../..").Change(idPhrase);
  }
}
