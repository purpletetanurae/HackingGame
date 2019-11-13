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
  public Dictionary<int, int> SolvencyText { get; set; } = new Dictionary<int, int>();
  public bool NeedAnswer { get; set; } = false;

  public Phrase()
  {
    
  }
  public string GetPhraseBySolvency(int solvency)
  {
    string OutPhrase = "Non_phrases";
    
    foreach (KeyValuePair<int, int> Pair in SolvencyText)
    {
      if (Pair.Key <= solvency) OutPhrase = PhraseLocale.GetText(Pair.Value);
    }
    return OutPhrase;
  }
  
}
