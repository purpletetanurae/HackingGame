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
  private const string _pathToPhrasesFile = @"Mob\Dialogs\PhrasesTest.dat";
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
  // private void SavePhrases()
  // {
  //     BinaryFormatter formatter = new BinaryFormatter();

  //     FileStream fs = new FileStream(_pathToPhrasesFile, FileMode.OpenOrCreate);
  //     formatter.Serialize(fs, PC);
  //     fs.Close();
  // }

}
