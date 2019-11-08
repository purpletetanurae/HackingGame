using Godot;
using System;


  /// <summary>
	/// Класс описывает бонусную вероятность спавна моба, например в час пик
	/// </summary>
public class ChanceEnhancer
{

  /// <summary>
	/// Максимальная вероятность
	/// </summary>

  public double MaxChance = 4;

  /// <summary>
	/// Длительность максимальной бонусной вероятности
	/// </summary>
  public double DurationMaxChance = 10;

  /// <summary>
	/// Длительность бонусной вероятности
	/// </summary>
  public double Duration = 20;
  /// <summary>
	/// Время старта увеличения вероятности
	/// </summary>
  public double StartTimePosition;

  /// <param name="startTimePosition">Время старта увеличения вероятности</param>
  public ChanceEnhancer(double startTimePosition)
  {
    StartTimePosition = startTimePosition;
  }

  /// <summary>
	/// Дает вероятность по заданному временни
	/// </summary>
	/// <param name="time">время вероятности</param>
  public double GetChance(float time)
  {

    double funcTime = (double)time - StartTimePosition;

    //если не в области бонуса шанса, сразу ретурним
    if ((funcTime <= 0)||(funcTime >= Duration)) return 1;

    double durationCurve = (Duration - DurationMaxChance) / 2;
    double chance = MaxChance - 1;

    //различная обработка в начале и конце графика
    if (funcTime < durationCurve)
    {
      chance *= GetChanceFunction((Math.PI/2)*(funcTime/durationCurve));
    }
    else if(funcTime > (durationCurve + DurationMaxChance))
    {
      chance *= GetChanceFunction(Math.PI/2 + (Math.PI/2)*((funcTime - durationCurve - DurationMaxChance)/durationCurve));
    }

    return chance + 1;
  }

  private double GetChanceFunction(double x)
  {
    return Math.Sin(x);
  }

}
