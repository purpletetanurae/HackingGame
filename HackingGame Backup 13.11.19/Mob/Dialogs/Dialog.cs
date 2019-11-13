using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
  ///<summary>
  /// Конкретный диалог, содержащий определённых участников
  ///</summary>
public class Dialog : Node
{
  public int Id;
  public int IdPhrase = 0;
  ///<summary>
  /// Участники диалога и их время на разговор
  ///</summary>
  public Dictionary<CashWalker, float> MembersTime = new Dictionary<CashWalker, float>();
  public List<CashWalker> AnsweredMobs = new List<CashWalker>();
  private Vector2 _center;
  private const int _radiousDialog = 50;
  private const float _startMobDialogTime = 200;
  private MobsDialogs _mobsDialogs;
  private Random _rand = new Random();
  private Timer _messageTimerClose;
  private CashWalker _speakingMob;
  ///<summary>
  /// При инициализации необходима ссылка на управляющую диалогами ноду. 
  /// А так же множество стартовых кэшволкеров
  ///</summary>
  public Dialog(MobsDialogs mobsDialogs, params CashWalker[] members)
  {
    _mobsDialogs = mobsDialogs;
    _center = GetCenterMass(members);
    foreach (CashWalker member in members)
    {
      Join(member);
    }
    StartDialog(GetRandomMember());
  }
  ///<summary>
  /// Добавление участника в разговор
  ///</summary>
  public void Join(CashWalker mob)
  {
    //более качественно можно сделать через followTask, но будет затратнее по памяти
    if (MembersTime.Count < 2)
    {
      mob.taskList.InterceptionWithSaveTask(new MoveTask(_center, 50));  
    }
    else
    {
      mob.taskList.InterceptionWithSaveTask(new MoveTask(GetPlacePosition()));
    }
    FillApMobIgnore(mob);
    mob.taskList.Insert(new WaitTask(){boundId = Id}, 1);
    MembersTime.Add(mob, GenerateMobDialogTime(mob));

    if (_speakingMob != null)
    {
      StartDialog(mob);
    }
  }
  private void StartDialog(CashWalker beginingMob)
  {
    AnsweredMobs = new List<CashWalker>();
    AnsweredMobs.Add(beginingMob);
    IdPhrase = 0; 
    if (_messageTimerClose != null)
    {
      _messageTimerClose.Disconnect("timeout", this, nameof(OnCloseTimerTimeOut));
    }
    SayNext();
  }
  ///<summary>
  /// Запустить следующую реплику случайного участника
  ///</summary>
  private void SayNext()
  {
    if (MembersTime.Count < 2) return;

    _speakingMob = AnsweredMobs[0];
    _messageTimerClose = _speakingMob.Say(_mobsDialogs.Phrases.GetNext(this));
    TakeTime(_messageTimerClose.WaitTime);
    _messageTimerClose.Connect("timeout", this, nameof(OnCloseTimerTimeOut));
  }
  private void SayAnswer()
  {

  }
  ///<summary>
  /// Отнять указанное время у всех участников
  ///</summary>
  private void TakeTime(float time)
  {
    for (int i = 0; i < MembersTime.Count; i++)
    {
      MembersTime[MembersTime.ElementAt(i).Key] -= time;
    }
  }
  ///<summary>
  /// Покинуть чат определенного моба
  ///</summary>
  private void Leave(CashWalker mob)
  {
    MembersTime.Remove(mob);
    if (AnsweredMobs.Contains(mob))
    {
      AnsweredMobs.Remove(mob);
    }
      mob.Say("I'am diconnect");
    mob.taskList.DeleteById(Id);
    if (MembersTime.Count == 1)
    {
      Leave(MembersTime.ElementAt(0).Key);
      SelfRemove();
    }
  }
  ///<summary>
  /// Добавить мобу в игнор лист участников конкретного диалога.
  /// Это необходимо, чтобы одни и те же мобы не начинали разговор дважды.
  ///</summary>
  private void FillApMobIgnore(CashWalker mob)
  {
    foreach(KeyValuePair<CashWalker, float> member in MembersTime)
    {
      mob.DialogData.AddIgnore(member.Key);
    }
  }
  ///<summary>
  /// Удаление этого чата из управляющей ноды.
  ///</summary>
  private void SelfRemove()
  {
    _mobsDialogs.Dialogs.Remove(this);
  }
  ///<summary>
  /// Получение центрра масс мобов.
  /// Это необходимо чтобы получить место встречи.
  /// Реализация для меньших затрат памяти.
  /// В будущем лучше использовать FollowTask. Это будет учитывать непроходимые объекты.
  ///</summary>
  private Vector2 GetCenterMass(Mob[] mobs)
  {
    float x = 0;
    float y = 0;
    float sumSpeed = 0;
    for (int i = 0; i < mobs.Length; i++)
    {
      x += mobs[i].Position.x * (1 / mobs[i].Speed);
      y += mobs[i].Position.y * (1 / mobs[i].Speed);
      sumSpeed += 1 / mobs[i].Speed;
    }
    return new Vector2(x / (sumSpeed), y / (sumSpeed));
  }
  ///<summary>
  /// Отслеживание завершения предыдущего сообщения.
  ///</summary>
  private void OnCloseTimerTimeOut()
  {
    _messageTimerClose.Disconnect("timeout", this, nameof(OnCloseTimerTimeOut));
    LeaveMobsWithoutTime();
    SayNext();
  }

  private void LeaveMobsWithoutTime()
  {
    foreach (CashWalker mob in GetMobsWithoutTime())
    {
      if (MembersTime.Count != 1)
      {
        Leave(mob);
      }
    }
  }
  private List<CashWalker> GetMobsWithoutTime()
  {
    List<CashWalker> mobs = new List<CashWalker>();
    foreach (KeyValuePair<CashWalker, float> mob in MembersTime)
    {
      if (mob.Value < 0)
      {
        mobs.Add(mob.Key);
      }
    }
    return mobs;
  }
  ///<summary>
  /// Получение позиции возле диалога.
  ///</summary>
  private Vector2 GetPlacePosition()
  {
    Vector2 place = new Vector2((float)_rand.NextDouble(),(float)_rand.NextDouble()/2);
    place *= _radiousDialog;
    return place + _center;
  }
  ///<summary>
  /// Генерация свободного времени для разговора для конкретного моба.
  /// Требует доработки.
  ///</summary>
  private float GenerateMobDialogTime(CashWalker mob)
  {
    return (float)(_startMobDialogTime - mob.taskList.Count * 10);
  }
  public CashWalker GetRandomMember()
  {
    return MembersTime.ElementAt(_rand.Next(MembersTime.Count)).Key;
  }
}
