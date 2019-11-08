using Godot;
using System;
using System.Collections.Generic;

public class CashWalker : Mob
{
  private const string _pathToMobsDialogs = "../MobsDialogs";
  private PackedScene _loadMoneyLabel;
  private int _money = 100;
  private Random _rand = new Random();
  ///<summary>
  /// Состоятельность моба.
  /// От данного свойства зависят основные характеристики моба: Деньги, Задачи с AttractionPoint 
  ///(выбор тех мест, которое схожее по состоятельности), внешний вид, желание разговаривать
  ///(с теми кто близок к нему по состоянию).
  ///</summary>
  [Export]
  public int Solvency = 50;

  ///<summary>
  /// Шанс встретить друга при визуальном контакте моба
  ///</summary>
  [Export]
  public double ChanceMeetFriend = 1;
  public DialogData DialogData;

  ///<summary>
  /// Информация, которую говорил моб в диалогах. В ключе - имя информации.
  ///</summary>
  
  ///<summary>
  /// Деньги моба.
  /// При изменении вылетает анимация.
  ///</summary>
  public int Money
  {
    set
    {
      ViewMoneyChanges(value - _money);
      _money = value;
      EmitSignal(nameof(PropertyChanged), nameof(Money), _money.ToString());
    }
    get
    {
      return _money;
    }
  }

  public override void _Ready()
  {
    base._Ready();
    DialogData = new DialogData(this);
    if (!HasNode(_pathToMobsDialogs))
    {
      GetNode<Area2D>("Vision").Disconnect("area_entered", this, nameof(OnVisionAreaEntered));
      GD.Print("ПРЕДУПРЕЖДЕНИЕ: не найден MobsDialogs по пути:" + _pathToMobsDialogs);
    }
    _loadMoneyLabel = (PackedScene)GD.Load("res://Mob/Modules/MoneyLabel.tscn");
  }
  private void ViewMoneyChanges(int moneyChanges)
  {
    if (!IsInsideTree()) return;
    if (moneyChanges == 0) return;

    MoneyLabel moneyLabel = (MoneyLabel)_loadMoneyLabel.Instance();
    moneyLabel.MoneyChanges = moneyChanges;
    //следующая строчка тестовая, необходимо подставить формулу расчёта позиции бошки моба
    moneyLabel.SetPosition(new Vector2(-50, -150));
    AddChild(moneyLabel);
  }
  //с некой вероятностью прохожий оказывается другом и вступаем в диалог
  protected override void OnVisionAreaEntered(CashWalker cashWalker)
  {
    if (DialogData.IsIgnore(cashWalker)) return;
    if ((_rand.NextDouble() - ChanceMeetFriend) < 0)
    {
      GetNode<MobsDialogs>(_pathToMobsDialogs).Connect(this, cashWalker);
    }
  }
}
