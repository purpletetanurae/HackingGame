using Godot;
using System;

/*
  Точка притяжения с очередью: Магазин
  Использование снижает бабло моба
 */
public class Shop : QueueAttractionPoint
{
  [Export]
  public int MinPrice = 50;
  [Export]
  public int MaxPrice = 100;
  

  private PackedScene _moneyAnimScene;
  public override void _Ready()
  {
    base._Ready();
    _moneyAnimScene = (PackedScene)GD.Load("res://Mob/Modules/TimeLabel.tscn");
  }

  protected override void Executed(CashWalker cashWalker)
	{
    if (cashWalker.Money >= MinPrice)
    {
      int price = GetPrice(cashWalker);
      cashWalker.Money -= price;
    }
	}
  //в будущем желательно переписать рандом под состояние моба
  protected int GetPrice(CashWalker cashWalker)
  {
    int Max = (MaxPrice > cashWalker.Money) ? cashWalker.Money : MaxPrice;
    return _rand.Next(MinPrice, Max); 
  }

}
