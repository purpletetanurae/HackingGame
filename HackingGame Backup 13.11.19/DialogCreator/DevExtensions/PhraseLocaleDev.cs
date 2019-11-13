using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public static class PhraseLocaleDev
{
  private static int _maxId = 0;
  private const string _pathToPhrasesLocalizationsFile = @"Mob\Dialogs\PhrasesLocalTest.dat";
  private static string _locale;
  private static List<string> _supportedLocals;
  private static Dictionary<int, Dictionary<string ,string>> _data; 

  public static void Load(KeyValuePair<Dictionary<int, Dictionary<string ,string>>, List<string>> phraseLocaleData)
  {
    _supportedLocals = phraseLocaleData.Value;
    _locale = _supportedLocals[0];
    _data = phraseLocaleData.Key;
    SetMaxKey();
  }
  private static void SetMaxKey()
  {
    if (_data.Count == 0)
    {
      _maxId = 0;  
    }
    else
    {
      _maxId = _data.Aggregate((l, r) => l.Key > r.Key ? l : r).Key;
    }
  }
  public static void Save()
  {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream fs = new FileStream(_pathToPhrasesLocalizationsFile, FileMode.OpenOrCreate);
    formatter.Serialize(fs, _data);
    fs.Close();
  }
  // for DialogCreator
  public static void SetText(int idx, string newText)
  {
    _data[idx][_locale] = newText;
  }
  public static string GetText(int idx)
  {
    return _data[idx][_locale];
  }
  public static void SetLocale(string lang)
  {
    _locale = lang;
    PhraseLocale.SetLocale(lang);
  }
  // for DialogCreator
  public static int CreateNewText()
  {
    
    _data.Add(++_maxId, new Dictionary<string, string>());
    foreach (string lang in _supportedLocals)
    {
      _data[_maxId].Add(lang, $"New solvency text in {lang} Language");
    }
    return _maxId;
  }
  //отладка
  public static string PrintData()
  {
    string str = "PrintData: \n";
    foreach (KeyValuePair <int, Dictionary<string ,string>> id in _data)
    {
      str += $"id : {id.Key} \n";
      foreach (KeyValuePair <string, string> lang in id.Value)
      {
        str += $" - lang : {lang.Key}, text: {lang.Value} \n";
      }
    }
    str += "end PrintData \n";
    return str;
  }
  public static List<string> GetSupportedLocals()
  {
    return _supportedLocals;
  }
}