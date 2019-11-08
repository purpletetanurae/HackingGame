using System;
using System.Collections.Generic;
using Godot;


/*
* множество SolvencyTexts должен быть отсортирован от меньшего к большему по ключу
*/
[Serializable]
public class Phrase
{
  public Dictionary<int, double> PossiblePhrases { get; set; } = new Dictionary<int, double>();
  public Dictionary<int, string> SolvencyText { get; set; } = new Dictionary<int, string>();
  public bool NeedAnswer { get; set; } = false;

  public Phrase()
  {

  }
  public string GetPhraseBySolvency(int solvency)
  {
    string OutPhrase = "Non_phrases";
    
    foreach (KeyValuePair<int, string> Pair in SolvencyText)
    {
      if (Pair.Key < solvency) OutPhrase = Pair.Value;
    }
    return OutPhrase;
  }
  
}
