using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
///<summary>
/// Редактор диалогов
///</summary>
public class DialogCreator : Control
{
  ///<summary>
  /// Фразы с каким солвенси показывать в возможных фразах
  ///</summary>
  protected int _possiblePhraseSolvency = 0;
  ///<summary>
  /// Модуль для работой возможных фраз
  ///</summary>
  private PossiblePhrases _possiblePhrases;
  ///<summary>
  /// Модуль для работы с текстами по солвенси
  ///</summary>
  private SolvencyTexts _solvencyTexts;
  ///<summary>
  ///путь к файлу сохранения фраз
  ///</summary>
  protected const string _pathToPhrasesFile = @"Mob\Dialogs\PhrasesTest.dat";
  ///<summary>
  /// Загруженная база фраз
  ///</summary>
  protected PhraseChainDev _phraseChainDev;
  ///<summary>
  ///Ссылка на фразы
  ///</summary>
  public Dictionary<int, Phrase> Phrases;
  ///<summary>
  ///Активный ID
  ///</summary>
  protected int _nowViewId;
  public override void _Input(InputEvent @event)
  {
    if (@event.IsActionPressed("Save")&&(!GetNode<Button>("ControlPanel/Save").IsDisabled()))
    {
      Save();
    }
  }
  public override void _Ready()
  {
    // WriteDatFile(_pathToPhrasesFile, new PhraseChainDev());
    // WriteDatFile(@"Mob\Dialogs\PhrasesLocalTest.dat", new Dictionary<int, Dictionary<string ,string>>());
    LoadPhrases(); 
    // _phraseChainDev.MobNames = new Dictionary<int, Dictionary<string, string>>();
    // _phraseChainDev.MobNames.Add(0, new Dictionary<string, string>{{"ru", "Саша"}, {"en", "Sasha"}});
    // _phraseChainDev.MobNames.Add(1, new Dictionary<string, string>{{"ru", "Максим"}, {"en", "Maxim"}});
    // _phraseChainDev.MobNames.Add(2, new Dictionary<string, string>{{"ru", "Николай"}, {"en", "Nikolas"}});
    // _phraseChainDev.MobNames.Add(3, new Dictionary<string, string>{{"ru", "Степан"}, {"en", "Steave"}});
    // _phraseChainDev.MobNames.Add(4, new Dictionary<string, string>{{"ru", "Дарья"}, {"en", "Dolly"}});
    // _phraseChainDev.MobNames.Add(5, new Dictionary<string, string>{{"ru", "Катя"}, {"en", "Kiti"}});
    // _phraseChainDev.MobNames.Add(6, new Dictionary<string, string>{{"ru", "Пётр"}, {"en", "Piter"}});
    // _phraseChainDev.MobNames.Add(7, new Dictionary<string, string>{{"ru", "Елена"}, {"en", "Elen"}});
    // _phraseChainDev.MobNames.Add(8, new Dictionary<string, string>{{"ru", "Михаил"}, {"en", "Michael"}});
    // _phraseChainDev.MobNames.Add(9, new Dictionary<string, string>{{"ru", "Артём"}, {"en", "Arthur"}});
    // _phraseChainDev.MobNames.Add(10, new Dictionary<string, string>{{"ru", "Иван"}, {"en", "Ivan"}});
    // _phraseChainDev.MobNames.Add(11, new Dictionary<string, string>{{"ru", "Ярослав"}, {"en", "Yaroslav"}});
    // _phraseChainDev.MobNames.Add(12, new Dictionary<string, string>{{"ru", "Маргарита"}, {"en", "Margaret"}});
    // _phraseChainDev.MobNames.Add(13, new Dictionary<string, string>{{"ru", "Денис"}, {"en", "Dennis"}});
    // WriteDatFile(_pathToPhrasesFile, (PhraseChain)_phraseChainDev);
    LoadScenes();
    PhraseLocale.Load();
    PhraseLocaleDev.Load(PhraseLocale.GetDevData());
    View(0);  

  }
  ///<summary>
  /// Перезагрузка объектов. Необходима для отображения корректной инфы
  ///</summary>
  public virtual void ReView()
  {
    View(_nowViewId);
  }
  ///<summary>
  /// Запись в файл
  ///</summary>
  private void WriteDatFile(string path, object data)
  {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
    formatter.Serialize(fs, data);
    fs.Close();
  }
  ///<summary>
  ///Загрузка фраз из файла
  ///</summary>
  protected void LoadPhrases()
  {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream fs = new FileStream(_pathToPhrasesFile, FileMode.OpenOrCreate);
    _phraseChainDev = (PhraseChainDev)formatter.Deserialize(fs);
    Phrases = _phraseChainDev.Phrases;
    
    fs.Close();
  }
  ///<summary>
  ///Подгрузка сцен (элементов solvencytext и possiblePhrases)
  ///</summary>
  protected virtual void LoadScenes()
  {
    _possiblePhrases = GetNode<PossiblePhrases>("PossiblePhrases/VBoxContainer");
    _solvencyTexts = GetNode<SolvencyTexts>("SolvencyTexts/VBoxContainer");
  }
  ///<summary>
  ///Смена показываемой фразы по ID
  ///</summary>
  protected virtual void View(int id)
  {    
    Clear();
    _nowViewId = id;
    if (!Phrases.ContainsKey(_nowViewId))
    {
      GetNode<ControlPanel>("ControlPanel").SetDisableds(false);
      return;
    }
    GetNode<ControlPanel>("ControlPanel").SetDisableds(true);
    
    ViewSolvencyTexts(Phrases[_nowViewId]);
    ViewPossiblePhrases(Phrases[_nowViewId]);
    GetNode<CheckBox>("NeedAnswer").Pressed = Phrases[_nowViewId].NeedAnswer;
  }
  ///<summary>
  ///Очистка всех полей
  ///</summary>
  protected virtual void Clear()
  {
    _possiblePhrases.RemoveAll();
    _solvencyTexts.RemoveAll();
    GetNode<CheckBox>("NeedAnswer").Pressed = false;
  }
  ///<summary>
  /// Отображение возможных фраз
  ///</summary>
  private void ViewPossiblePhrases(Phrase phrase)
  {
    foreach (KeyValuePair <int , double> posPhrase in phrase.PossiblePhrases)
    {
      _possiblePhrases.Add(posPhrase.Key, Phrases[posPhrase.Key].GetPhraseBySolvency(_possiblePhraseSolvency), posPhrase.Value);
    }
  }
  ///<summary>
  /// Отображение вариаций фраз по солвенси
  ///</summary>
  private void ViewSolvencyTexts(Phrase phrase)
  {
    foreach(KeyValuePair<int, int> text in phrase.SolvencyText)
    {
       _solvencyTexts.Add(text.Key, text.Value);
    }
  }
  ///<summary>
  ///Добавление фразы.
  ///Работает так: нужно открыть ID, который не занят и выполнить данный метод.
  ///Сигнал одноименной кнопки кидает сигнал на этот метод.
  ///</summary>
  private void AddPhrase()
  {
    Phrases.Add(_nowViewId, new Phrase());
    View(_nowViewId);
    GetTree().CallGroup("Save", nameof(PrepareForSave));
  }
  ///<summary>
  /// Удаление фразы
  ///</summary>
  private void DeletePhrase()
  {
    _phraseChainDev.RemovePhrase(_nowViewId);
    View(_nowViewId);
    GetTree().CallGroup("Save", nameof(PrepareForSave));
  }
  ///<summary>
  ///Создание пустой строки SolvencyText
  ///Внимание! Нельзя добавлять с одинаковым solvency
  ///Сигнал одноименной кнопки кидает сигнал на этот метод.
  ///</summary>
  private void AddSolvencyText()
  {
    _solvencyTexts.Add(0, PhraseLocaleDev.CreateNewText());
    GetTree().CallGroup("Save", nameof(PrepareForSave));
  }
  ///<summary>
  ///Создание пустой строки PossiblePhrase
  ///Внимание! Нельзя добавлять с одинаковым Id
  ///Сигнал одноименной кнопки кидает цепляется к этому методу.
  ///</summary>
  
  private void AddPossiblePhrase()
  {
    int idPastedPhrase = (int)GetNode<SpinBox>("ControlPanel/BackGround/Panel/SpinBox").Value;
    string error = _phraseChainDev.CheckPossiblePhrase(idPastedPhrase, _nowViewId);
    if (error == "Good")
    {
      _possiblePhrases.Add(idPastedPhrase, Phrases[idPastedPhrase].GetPhraseBySolvency(_possiblePhraseSolvency), 1);
      GetTree().CallGroup("Save", nameof(PrepareForSave));
    }
    else
    {
      GetNode<MessageToUser>("MessageToUser").ShowError(error);
    }
    
  }
  ///<summary>
  ///Обработка сигнала при смене текста
  ///</summary>
  private void OnIdChanged(string newText)
  {
    Regex regexObj = new Regex(@"[^\d]");
    string resultString = regexObj.Replace(newText, "");
    if (resultString.Length > 0)
    {
      View(resultString.ToInt());
    }
  }
  ///<summary>
  ///Обработка сигнала при нажатии кнопки назад
  ///</summary>
  private void OnPreviousPressed()
  {
    if (_nowViewId > 0)
    {
      GetNode<LineEdit>("IdNavigation/Id").Text = (_nowViewId - 1).ToString();
      View(_nowViewId - 1);
    }
  }
  ///<summary>
  ///При любом изменения сохраняется для начала в переменную
  ///</summary>
  public void PrepareForSave()
  {
    if (Phrases.ContainsKey(_nowViewId))
    {
      Phrases[_nowViewId].SolvencyText = _solvencyTexts.GetData();
      Phrases[_nowViewId].PossiblePhrases = _possiblePhrases.GetData();
      Phrases[_nowViewId].NeedAnswer = GetNode<CheckBox>("NeedAnswer").Pressed;
    }
    
    GetNode<ControlPanel>("ControlPanel").SetSaveDisable(false);
  }
  ///<summary>
  ///Сохранение
  ///</summary>
  public void Save()
  {
    if (!IsAllSetsSolvencyTexts())
    {
      return;
    }
    WriteDatFile(_pathToPhrasesFile, (PhraseChain)_phraseChainDev);
    PhraseLocaleDev.Save();
    GetNode<ControlPanel>("ControlPanel").SetSaveDisable(true);
    GetNode<MessageToUser>("MessageToUser").Show("Сохранено");
  }
  ///<summary>
  ///Проверка на фразы с пустыми solvencyTexts
  ///</summary>
  private bool IsAllSetsSolvencyTexts()
  {
    List<int> nullSolvencyTexts = _phraseChainDev.GetIdPhrasesWhereNullSolvencyText(); 
    if (nullSolvencyTexts.Count > 0)
    {
      string error = $"Не добавлен SolvencyText в следующ{(nullSolvencyTexts.Count == 1 ? "eм" : "их")} Id: ";
      foreach (int id in nullSolvencyTexts)
      {
        error += id.ToString() + ", ";
      }
      error = error.Substr(0, error.Length - 2);
      GetNode<MessageToUser>("MessageToUser").ShowError(error);
      return false;
    }
    else
    {
      return true;
    }
  }
  
  ///<summary>
  ///Обработка сигнала при нажатии кнопки вперёд
  ///</summary>
  private void OnNextPressed()
  {
      GetNode<LineEdit>("IdNavigation/Id").Text = (_nowViewId + 1).ToString();
      View(_nowViewId + 1);
  }
  ///<summary>
  ///Обработка сигнала при смене solvency
  ///</summary>
  public void OnSolvencyChanged(float newValue)
  {
    GetNode<Label>("SolvencyChanged/Label").Text = "Solvency: " + newValue.ToString();
    _possiblePhraseSolvency = (int)newValue;
    ReView();
  }
  ///<summary>
  ///Переход к редактированию связей
  ///</summary>
  protected virtual void ChangeScene()
  {
    GetTree().ChangeScene("res://DialogCreator/DCHeartStone/DCHeartStone.tscn");
  }
}
