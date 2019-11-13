using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class DCHeartStone : DialogCreator
{
  private Dictionary <int, Phrase> _changenedPossiblePhrases;
  private ChangerPossiblePhrases _changerPossiblePhrases;
  private Random _rand = new Random();
  public override void _Ready()
  {
    LoadPhrases();
    LoadScenes();
    PhraseLocale.Load();
    PhraseLocaleDev.Load(PhraseLocale.GetDevData());
    RandomView();
  }
  protected override void Clear()
  {
    _changerPossiblePhrases.RemoveAll();
  }

  protected override void LoadScenes()
  {
    _changerPossiblePhrases = GetNode<ChangerPossiblePhrases>("ScrollContainer/ChangerPossiblePhrases");
  }
  protected override void View(int id)
  {
    _nowViewId = id;
    _changenedPossiblePhrases = GetChangenedPossiblePhrases();
    ReView();
  }
  public override void ReView()
  {
    Clear();
    ViewNowPhrase();
    ViewChangedPossiblePhrases();
  }
  private void RandomView()
  {
    if (Phrases.Count < 2)
    {
      GetNode<MessageToUser>("MessageToUser").ShowError("Колличество фраз < 2! Установка связей невозможна!");
    } else
    {
      View(Phrases.ElementAt(_rand.Next(Phrases.Count)).Key);
    }
    
  }
  private void ViewNowPhrase()
  {
    GetNode<Label>("NowPhrase/Label").Text = Phrases[_nowViewId].GetPhraseBySolvency(_possiblePhraseSolvency);
    GetNode<Label>("NowPhrase/Panel/Label").Text = _nowViewId.ToString();
    GetNode<CheckBox>("NowPhrase/CheckBox").Pressed = Phrases[_nowViewId].NeedAnswer;
  }
  private void ViewChangedPossiblePhrases()
  {
    foreach (KeyValuePair<int, Phrase> kvp in _changenedPossiblePhrases)
    {
      _changerPossiblePhrases.Add(kvp.Key, kvp.Value.GetPhraseBySolvency(_possiblePhraseSolvency));
    }
  }

  private Dictionary<int, Phrase> GetChangenedPossiblePhrases(int limit = 4)
  {
    IEnumerable<KeyValuePair <int, Phrase>> query = from phrase in Phrases
                                                    where !Phrases[_nowViewId].PossiblePhrases.ContainsKey(phrase.Key)
                                                    where phrase.Key != _nowViewId
                                                    orderby _rand.Next()                                                
                                                    select phrase;
    return query.Take(limit).ToDictionary(x => x.Key, x => x.Value);
  }
  public void Change(int id)
  {
    Phrases[_nowViewId].PossiblePhrases.Add(id, 1);
    Phrases[_nowViewId].PossiblePhrases = PhraseChainDev.NormalizePossiblePhrase(Phrases[_nowViewId].PossiblePhrases);
    GetNode<ControlPanel>("ControlPanel").SetSaveDisable(false);
    RandomView();
  }

  protected override void ChangeScene()
  {
    GetTree().ChangeScene("res://DialogCreator/DialogCreator.tscn");
  }

  
}
