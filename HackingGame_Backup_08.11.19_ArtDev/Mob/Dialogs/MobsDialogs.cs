using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

  ///<summary>
  /// Управляющая система диалогов.
  /// Необходима для координации мобов и создании новых диалогов.
  ///</summary>
public class MobsDialogs : Node
{
  private const string _pathToPhrasesFile = @"Mob\Dialogs\Phrases.dat";
  public List<Dialog> Dialogs = new List<Dialog>();
  private int _dialogsIds = 0;
  public PhraseChain Phrases;
  
  public override void _Ready()
  {
    //SavePhrases();
    LoadPhrases();
  }
  private void LoadPhrases()
  {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream fs = new FileStream(_pathToPhrasesFile, FileMode.OpenOrCreate);
    Phrases = (PhraseChain)formatter.Deserialize(fs);
    fs.Close();
  }
  public void Connect(CashWalker invoker, CashWalker to)
  {
    Dialog invokerDialog = GetDialogByMob(invoker);
    Dialog friendDialog = GetDialogByMob(to);

    if ((friendDialog == null)&&(invokerDialog == null))
    {
      invoker.Say("CreateNewChanel");
      Dialogs.Add(new Dialog(this, invoker, to){Id = _dialogsIds++});
    }
    else if (friendDialog == null)
    {
      invoker.Say("Connect friend to invoker dialog");
      invokerDialog.Join(to);
    }
    else if (invokerDialog == null)
    {
      invoker.Say("Connect invoker to friend dialog");
      friendDialog.Join(invoker);
    }
  }

  private Dialog GetDialogByMob(CashWalker mob)
  {
    return Dialogs.Find(dialog => dialog.MembersTime.ContainsKey(mob));
  }
  //for test, need delete
  private void SavePhrases()
  {
    PhraseChain PC = new PhraseChain();
      Phrase phrase0 = new Phrase();
      Phrase phrase1 = new Phrase();
      Phrase phrase2 = new Phrase();
      Phrase phrase3 = new Phrase();
      Phrase phrase4 = new Phrase();


      phrase0.SolvencyText.Add(0, "Здарова[MemberNames]");
      phrase0.SolvencyText.Add(100, "Привет[MemberNames]");
      phrase0.SolvencyText.Add(400, "Добрый [DayType][MemberNames]");
      phrase0.NeedAnswer = true;
      phrase0.PossiblePhrases.Add(1, 1);

      phrase1.SolvencyText.Add(0, "Здарова, [MemberNames]");
      phrase1.SolvencyText.Add(100, "Привет, [MemberNames]");
      phrase1.SolvencyText.Add(400, "Добрый [DayType], [MemberNames]");
      phrase1.PossiblePhrases.Add(2, 1);

      phrase2.SolvencyText.Add(0, "Как житуха [Pronoun]?");
      phrase2.SolvencyText.Add(100, "Как дела [Pronoun]?");
      phrase2.SolvencyText.Add(400, "Как Ваше ничего?");
      phrase2.PossiblePhrases.Add(3, 0.5);
      phrase2.PossiblePhrases.Add(4, 0.5);
      phrase2.NeedAnswer = true;

      phrase3.SolvencyText.Add(0, "Заебись!");
      phrase3.SolvencyText.Add(100, "Хорошо!");
      phrase3.SolvencyText.Add(400, "Весьма хорошо!");


      phrase4.SolvencyText.Add(0, "Хуёво!");
      phrase4.SolvencyText.Add(100, "Такое себе");
      phrase4.SolvencyText.Add(400, "Дела весьма не вызывают положительных эмоций");

      PC.Phrases = new Dictionary<int, Phrase>()
      {
        {0, phrase0 },
        {1, phrase1 },
        {2, phrase2 },
        {3, phrase3 },
        {4, phrase4 }
      };

      BinaryFormatter formatter = new BinaryFormatter();

      FileStream fs = new FileStream(_pathToPhrasesFile, FileMode.OpenOrCreate);
      formatter.Serialize(fs, PC);
      fs.Close();
  }

}
