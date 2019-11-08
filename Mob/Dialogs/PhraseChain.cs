using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Godot;

[Serializable]
public class PhraseChain
{
  private Random _rand = new Random();
  public Dictionary<int, Phrase> Phrases { get; set; }
  

  public string GetLast(CashWalker mob)
  {
    return "lastPhrase";
  }
  public string GetNext(Dialog dialog)
  {
    CashWalker answeredMob = dialog.AnsweredMobs[0];
    int idNextPhrase = GetRandomPossiblePhraseId(Phrases[dialog.IdPhrase]);
    string textPhrase = Phrases[idNextPhrase].GetPhraseBySolvency(answeredMob.Solvency);
    
    if (dialog.AnsweredMobs.Count > 1)
    {
      dialog.AnsweredMobs.Remove(answeredMob);
    }
    else
    {
      dialog.IdPhrase = idNextPhrase;
      dialog.AnsweredMobs = GetAnswersMobs(dialog, dialog.GetRandomMember());
    }
    
    return SetCurrentVariablesInText(textPhrase, answeredMob);
  }
  public string GetNext2(Dialog dialog)
  {
    CashWalker answeredMob = dialog.AnsweredMobs[0];
    string textPhrase = Phrases[dialog.IdPhrase].GetPhraseBySolvency(answeredMob.Solvency);
    
    if (dialog.AnsweredMobs.Count > 0)
    {
      dialog.AnsweredMobs.Remove(answeredMob);
    }
    else
    {
      dialog.IdPhrase = GetRandomPossiblePhraseId(Phrases[dialog.IdPhrase]);
      dialog.AnsweredMobs = GetAnswersMobs(dialog, dialog.GetRandomMember(), answeredMob);
      return GetNext2(dialog);
    }
    return SetCurrentVariablesInText(textPhrase, answeredMob);
  }
  public string GetNext3(Dialog dialog)
  {
    CashWalker answeredMob = dialog.AnsweredMobs[0];
    string textPhrase = Phrases[dialog.IdPhrase].GetPhraseBySolvency(answeredMob.Solvency);
    dialog.AnsweredMobs.Remove(answeredMob);

    if (dialog.AnsweredMobs.Count == 0)
    {
      dialog.AnsweredMobs = GetAnswersMobs(dialog, dialog.GetRandomMember(), answeredMob);
      dialog.IdPhrase = GetRandomPossiblePhraseId(Phrases[dialog.IdPhrase]);
    }

    return SetCurrentVariablesInText(textPhrase, answeredMob);
  }
  private List<CashWalker> GetAnswersMobs(Dialog dialog, CashWalker mob, CashWalker answeredMob = null)
  {

    List<CashWalker> answers = new List<CashWalker>();
    if (Phrases[dialog.IdPhrase].NeedAnswer)
    {
      foreach(KeyValuePair<CashWalker, float> member in dialog.MembersTime)
      {
        if (member.Key != answeredMob)
        {
          answers.Add(member.Key);
        }
      }
    }
    else 
    {
      answers.Add(mob);
    }
    return answers;
  }

  private int GetRandomPossiblePhraseId(Phrase phrase)
  {
    double roll = new Random().NextDouble();
    foreach(KeyValuePair<int, double> posPhrase in phrase.PossiblePhrases)
    {
      
      roll -= posPhrase.Value;
      if (roll <= 0) return posPhrase.Key;
    }
    return 0;
  }
  private string SetCurrentVariablesInText(string text, CashWalker mob)
  {
    Regex pattern = new Regex(@"(?<=\[).*?(?=\])");
    string newText = pattern.Replace(text, delegate (Match match)
    {
      return GetVar(match.Value, mob);
    });
    return newText;
  }
  
  private string GetVar(string name, CashWalker mob)
  {
    string outVar = mob.DialogData.GetAnswer(name);
    if (outVar != "none")
    {
      return outVar;
    }

    switch (name)
    {
      case "MobName":
        outVar = "...";
        mob.DialogData.AddAnswer(name, outVar);
        break;

      case "CompanionName":
        outVar = "...";
        break;

      default:
        outVar = "!not found name!";
        break;
    }
    
    return outVar;
  }
}