using Godot;
using System;
using System.Collections.Generic;


interface iQueue
{
  void Add(Mob mob); //добавление моба в очередь
  void Remove(int idx = 0); //Удаление моба из очереди
  Mob GetMob(int idx); //Получить ссылку на моба по заданной позиции
  Vector2 GetQueuePosition(int idx);
  
}
  /// <summary>
  ///  Очередь. Описывает поведение живой очереди мобов.
  /// </summary>
public class Queue : iQueue
{
  /// <summary>
  ///  Настоящее количество человек в очереди
  /// </summary>
  public int Count 
  {
    get 
    {
      return _list.Count;
    }
  }

  private List <Mob> _list = new List<Mob>();
  private int _capacity;
  private Path2D _path;
  private QueueAttractionPoint _Qap;
  
  private Vector2 _parentPosition;
  private Timer _timer;
  public Queue (QueueAttractionPoint queueAttractionPoint)
  {
    _Qap = queueAttractionPoint;
    _path = queueAttractionPoint.GetNode<Path2D>("Path2D");
    _capacity = _path.GetCurve().GetPointCount();
    _parentPosition = queueAttractionPoint.Position;
    _timer = queueAttractionPoint.GetNode<Timer>("QueueTimer");
  }


  /// <summary>
  /// Добавление моба в очередь
  /// или его отправка на следующую задачу
  /// </summary>
  /// <param name="mob">Моб, который встанет в очередь, а Иван - петух)))</param>
  public void Add(Mob mob)
  {
    if (_list.Contains(mob)) return;

    if (Count < _capacity)
    {
      _list.Add(mob);
      PutMobInPlace(mob, Count - 1);
    }
    else
    {
      mob.taskList.Next();
    }
    if (Count == 1)
    {
      _Qap.SetPhysicsProcess(true);
    }
  }
  
  /// <summary>
  /// Удаление моба из очереди
  /// </summary>
  /// <param name="idx">Индекс позиции получаемого моба</param>
  public void Remove(int idx = 0)
  {
    _list[idx].taskList.Delete(0);
    _list.RemoveAt(idx);
    
    if (Count > 0)
    {
      Shift();
      _Qap.SetPhysicsProcess(true);
    }
    
  }

  /// <summary>
  /// Получить ссылку на моба
  /// </summary>
  /// <param name="idx">Индекс позиции получаемого моба</param>
  public Mob GetMob(int idx)
  {
    return _list[idx];
  }

  //получить координаты очереди idx
  public Vector2 GetQueuePosition(int idx)
  {
    return _path.GetCurve().GetPointPosition(idx) + _parentPosition;
  }

  //сдвиг всех в очереди
  private void Shift()
  {
    for (int i = 0; i < Count; i++)
    {
      PutMobInPlace(_list[i], i);
    }
  }

  //поставить моба в определённое место в очереди и приказать ждать
  private void PutMobInPlace(Mob mob, int position)
  {
      
    if (position >= _capacity)
      GD.PrintErr("Queue->PutMobInPlace-> попытка поставить моба в положение сверх вместимости очереди!");
    
    Vector2 queuePosition = GetQueuePosition(position);
    mob.taskList.Interception(new MoveTask(queuePosition){boundId = _Qap.Id});

    //в данном случае проверка "if" исправляет следующее:
    // если моб ещё не дошёл до своей точки и перехватиться может
    // moveTask и старый waitTask останется
    // возникает проблема забытых waitTask'oв
    if ((mob.taskList.Count > 1)&&(_Qap.Id != mob.taskList.list[1].boundId))
    {
      
      mob.taskList.Insert(new WaitTask(){boundId = _Qap.Id}, 1);
    }
    
  }
  public bool Contains(Mob mob)
  {
    return _list.Contains(mob);
  }

}
