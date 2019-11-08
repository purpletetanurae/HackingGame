using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
///<summary>
///Центровой мозг
///</summary>
public class DialogCreator : Control
{
  private PossiblePhrases _possiblePhrases;
  private SolvencyTexts _solvencyTexts;
  ///<summary>
  ///путь к файлу сохранения фраз
  ///</summary>
  private const string _pathToPhrasesFile = @"Mob\Dialogs\Phrases.dat";
  private PhraseChain _phrasePack;
  ///<summary>
  ///Все фразы
  ///</summary>
  public Dictionary<int, Phrase> Phrases;
  ///<summary>
  ///Активный ID
  ///</summary>
  private int _nowViewId;

  public override void _Ready()
  {
    LoadPhrases();  
    LoadScenes();
    View(0);  

  }
  ///<summary>
  ///Загрузка фраз из файла
  ///</summary>
  private void LoadPhrases()
  {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream fs = new FileStream(_pathToPhrasesFile, FileMode.OpenOrCreate);
    _phrasePack = (PhraseChain)formatter.Deserialize(fs);
    Phrases = _phrasePack.Phrases;
    fs.Close();
  }
  ///<summary>
  ///Подгрузка сцен (элементов solvencytext и possiblePhrases)
  ///</summary>
  private void LoadScenes()
  {
    _possiblePhrases = GetNode<PossiblePhrases>("PossiblePhrases");
    _solvencyTexts = GetNode<SolvencyTexts>("SolvencyTexts");
  }
  ///<summary>
  ///Смена показываемой фразы по ID
  ///</summary>
  private void View(int id)
  {    
    Clear();
    _nowViewId = id;
    if (!Phrases.ContainsKey(id))
    {
      GetNode<ControlPanel>("ControlPanel").SetDisableds(false);
      
      return;
    }
    GetNode<ControlPanel>("ControlPanel").SetDisableds(true);
    
    ViewSolvencyTexts(Phrases[id]);
    ViewPossiblePhrases(Phrases[id]);
    GetNode<CheckBox>("NeedAnswer").Pressed = Phrases[id].NeedAnswer;
  }
  ///<summary>
  ///Очистка всех полей
  ///</summary>
  private void Clear()
  {
    _possiblePhrases.RemoveAll();
    _solvencyTexts.RemoveAll();
    GetNode<CheckBox>("NeedAnswer").Pressed = false;
  }

  private void ViewPossiblePhrases(Phrase phrase)
  {
    foreach (KeyValuePair <int , double> posPhrase in phrase.PossiblePhrases)
    {
      _possiblePhrases.Add(posPhrase.Key, Phrases[posPhrase.Key].SolvencyText[0], posPhrase.Value);
    }
  }
  private void ViewSolvencyTexts(Phrase phrase)
  {
    foreach(KeyValuePair<int, string> text in phrase.SolvencyText)
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
  }
  ///<summary>
  ///Создание пустой строки SolvencyText
  ///Внимание! Нельзя добавлять с одинаковым solvency
  ///Сигнал одноименной кнопки кидает сигнал на этот метод.
  ///</summary>
  private void AddSolvencyText()
  {
    _solvencyTexts.Add(0, "");
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
  ///Обработка сигнала при нажатии кнопки вперёд
  ///</summary>
  private void OnNextPressed()
  {
      GetNode<LineEdit>("IdNavigation/Id").Text = (_nowViewId + 1).ToString();
      View(_nowViewId + 1);
  }
}
