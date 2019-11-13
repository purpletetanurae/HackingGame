using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public static class PhraseLocale
{
  private static int _maxId = 0;
  private const string _pathToPhrasesLocalizationsFile = @"Mob\Dialogs\PhrasesLocalTest.dat";
  private static string _locale = "ru";
  private static List<string> _supportedLocals = new List<string>() {"ru", "en"};
  private static Dictionary<int, Dictionary<string ,string>> _data = new Dictionary<int, Dictionary<string ,string>>(); 

  public static void Load()
  {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream fs = new FileStream(_pathToPhrasesLocalizationsFile, FileMode.OpenOrCreate);
    _data = (Dictionary<int, Dictionary<string ,string>>)formatter.Deserialize(fs);
    fs.Close();
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

  public static string GetText(int idx)
  {
    return _data[idx][_locale];
  }
  public static void SetLocale(string lang)
  {
    _locale = lang;
  }

  public static List<string> GetSupportedLocals()
  {
    return _supportedLocals;
  }
  public static KeyValuePair<Dictionary<int, Dictionary<string ,string>>, List<string>> GetDevData()
  {
    return new KeyValuePair<Dictionary<int, Dictionary<string, string>>, List<string>>(_data, _supportedLocals);
  }
}
