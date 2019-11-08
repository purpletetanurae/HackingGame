using Godot;
using System;

public class SexShop : Shop
{
  private PackedScene _dicks;
  public override void _Ready()
  {
    base._Ready();
    _dicks = (PackedScene)GD.Load("res://AttractionPoints/QueueAttractionPoints/Shops/SexShop/Dick.tscn");
  }
  protected override void Executed(CashWalker cashWalker)
	{
    

    if (cashWalker.Money >= MinPrice)
    {
      base.Executed((CashWalker)cashWalker);
      float DickStats = (float)_rand.Next(100) / 100;
      Color DickColor = new Color(1 - DickStats, 1 - DickStats, 1 - DickStats);
      

      Node2D Dick = (Node2D)_dicks.Instance();
      //GD.Print("color" + DickColor + " " + Dick.GetModulate());
      Dick.SetScale(new Vector2(DickStats + (float)0.2, DickStats + (float)0.2));
      Dick.SetModulate(DickColor);
      //GD.Print("color" + DickColor + " " + Dick.GetModulate());

      cashWalker.AddChild(Dick);

    }
	}
}
