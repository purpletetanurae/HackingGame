using Godot;
using System;
using System.Collections.Generic;

public class MobGenerator : Node2D
{
  const string pathToAttractionPoints = "/root/Main/AttractionPoints";
  /// <summary>
	/// Минимальная состоятельность моба (не деньги)
	/// </summary>
  [Export]
  public int MinSolvencyCashWalkers = 100;

  /// <summary>
	/// Максимальная состоятельность моба (не деньги)
	/// </summary>
  [Export]
  public int MaxSolvencyCashWalkers = 150;
  
  /// <summary>
	/// Список увеличенной нагрузки спавна мобов
	/// </summary>
  public List<ChanceEnhancer> PeakHours = new List<ChanceEnhancer>();


  /// <summary>
	/// Стартовая вероятность спавна моба каждый тик
	/// </summary>
  [Export]
  public double StartChancePerTick = 0.15;
  private Random _rand = new Random();




  private Node2D _spawnPaths;
  private int _countPaths;
  private Node2D _cashWalkers;
  private PackedScene _loadCashWalker;
  private double _nowChance;
  private List<AttractionPoint> _attractionPoints;
  private Timer _levelTimer;
  private Vector2 _startPositionCashWalker;

  public override void _Ready()
  {
    SetVars();
    if (GetNode<Node2D>("SpawnPaths").GetChildCount() == 0)
    {
      GD.Print("ОШИБКА! Нет отрезков спавна мобов. Необходимо добавить path2D и PathFollow2D в SpawnPath. минимум 2 точки спавна.");
    }
    //test Generate
    //PeakHours.Add(new ChanceEnhancer(5));
  }
  

  public void OnSpawnTimerTimeOut()
  {
    if (_rand.NextDouble() < GetNowChance())
    {
      Instance(100);
    }
  }


  protected Vector2 GetSpawnPosition(int idxSpawn)
  {

    Path2D path = GetNode<Node2D>("SpawnPaths").GetChild<Path2D>(idxSpawn);
    PathFollow2D pathFollow = path.GetNode<PathFollow2D>("PathFollow2D");
    pathFollow.SetUnitOffset((float)_rand.NextDouble());

    return pathFollow.GetGlobalPosition();
  }
  protected void Instance(int solvency)
  {
    int idxSpawn = GetRandomIdxSpawnPath();
    int idxExit = GetRandomIdxSpawnPath();

    TaskList tasks = GetTasks(solvency);

    CashWalker instanceCashWalker = (CashWalker)_loadCashWalker.Instance();

    _startPositionCashWalker = GetSpawnPosition(idxSpawn);
    instanceCashWalker.Position = _startPositionCashWalker;
    instanceCashWalker.Solvency = solvency;
    instanceCashWalker.Money = _rand.Next(solvency - 50, solvency + 50);


    //желательно это поправить
    //если начальная и конечная сторона совпадает и нет никаких задач,
    //то нужно хотябы сделать так чтобы моб проходил улицу 
    //и не было вероятности что он откуда выйдет сразу туда и зайдёт
    while((idxExit == idxSpawn)&&(tasks.Count == 0))
    {
      idxExit = GetRandomIdxSpawnPath();
    }

    

    tasks.Add(new MoveTask(GetSpawnPosition(idxExit)));
    instanceCashWalker.taskList = tasks;

    _cashWalkers.AddChild(instanceCashWalker);
  }
  protected void GetLook()
  {

  }
  protected TaskList GetTasks(int solvency)
  {
    TaskList tasks = new TaskList(); 
    foreach (AttractionPoint attractionPoint in _attractionPoints)
    {
      tasks += attractionPoint.GetTaskList(solvency);
    }
    return tasks;
  }

  //получаем случайное место спавна
  private int GetRandomIdxSpawnPath()
  {
    int check = _rand.Next(_countPaths);
    return check;
  }

  //возвращает бонусную вероятность в конкретное время
  private double GetTotalBonusChance(float time)
  {
    double total = 1;
    foreach (ChanceEnhancer chance in PeakHours)
    {
      total *= chance.GetChance(time);
    }
    
    return total;
  }

  //возвращает вероятность спавна моба в конкретное время
  private double GetNowChance()
  {
    double nowChance = GetTotalBonusChance(_levelTimer.WaitTime - _levelTimer.TimeLeft);
    nowChance *= StartChancePerTick;

    nowChance = (nowChance > 1) ? 1 : nowChance; 
    nowChance = (nowChance < 0) ? 0 : nowChance; 

    return nowChance;
  }

  //получение точек притяжения и сортировка по расстоянию от моба до точки
  private List<AttractionPoint> GetAttractionPointsList()
  {
    List<AttractionPoint> attractionPoints = new List<AttractionPoint>();
    Node2D node = GetNode<Node2D>(pathToAttractionPoints);

    foreach(AttractionPoint attractionPoint in node.GetChildren())
    {
      attractionPoints.Add(attractionPoint);
    }

    attractionPoints.Sort(
      delegate(AttractionPoint ap1, AttractionPoint ap2)
      {
        float firstDistance = _startPositionCashWalker.DistanceTo(ap1.Position);
        float secondDistance = _startPositionCashWalker.DistanceTo(ap2.Position);
        return firstDistance > secondDistance ? 1 : 0;
      }
    );

    return attractionPoints;
  }

  private void SetVars()
  {
    _spawnPaths = GetNode<Node2D>("SpawnPaths");
    _loadCashWalker = (PackedScene)GD.Load("res://Mob/CashWalker/CashWalker.tscn");
    _cashWalkers = GetNode<Node2D>("CashWalkers");
    _countPaths = _spawnPaths.GetChildCount();
    _levelTimer = GetNode<Timer>("LevelTimer");
    _attractionPoints = GetAttractionPointsList();
  }

}
