using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

///<summary>
/// Расширение класса для разработки.
/// Тут содержаться методы для корректировки БД фраз.
///</summary>
[Serializable]
public class PhraseChainDev : PhraseChain
{
  
  public string CheckPossiblePhrase(int idPastedPhrase, int id)
  {
    if(idPastedPhrase == id) return "AddPossiblePhrase: Error: Циклы делать нельзя! Добавление ссылки самого на себя";
    if (!Phrases.ContainsKey(idPastedPhrase)) return "AddPossiblePhrase : ERROR : Не существует вставляемой фразы";
    if (Phrases[id].PossiblePhrases.ContainsKey(idPastedPhrase)) return "AddPossiblePhrase : ERROR : Данная возможная фраза уже добавлена";
    return "Good";
  }

  ///<summary>
  /// Удаление фразы
  /// Так же подчищает у других фраз связи с этой фразой.
  /// Так же подчищает БД с локализацией.
  ///</summary>
  public void RemovePhrase(int id)
  {
    foreach (KeyValuePair<int, Phrase> kvp in Phrases)
    {
      if (kvp.Value.PossiblePhrases.ContainsKey(id))
      {
        kvp.Value.PossiblePhrases.Remove(id);
      }
    }
    Phrases.Remove(id);
  }
  ///<summary>
  /// Нормализация вероятностей возможных фраз
  ///</summary>
  public static Dictionary<int, double> NormalizePossiblePhrase(Dictionary<int, double> posPhrase)
  {
    if (posPhrase.Count == 0) return posPhrase;

    double sum = 0;
    foreach (KeyValuePair<int, double> kvp in posPhrase)
    {
      sum += kvp.Value;
    }
    double multiplier = 1/sum;
    Dictionary <int, double> outPosPhrases = new Dictionary<int, double>();

    foreach (KeyValuePair<int, double> kvp in posPhrase)
    {
      outPosPhrases.Add(kvp.Key, kvp.Value * multiplier);
    }

    return outPosPhrases;
  }
  public List<int> GetIdPhrasesWhereNullSolvencyText()
  {
    IEnumerable <int> query = from phrase in Phrases
                              where phrase.Value.SolvencyText.Count == 0
                              select phrase.Key;
    return query.ToList();
  }
}
