using Godot;
using System;

public class AttractionPoint : Area2D
{
	[Export]
	public int Id;
	[Export]
	public int Solvency = 200;

	[Export]
	public double _taskChancer_ReductorFactor = 100;
	[Export]
	public double _taskChancer_MaxChance = 1; 
	protected Random _rand = new Random();
	protected virtual void OnAreaEntered(Mob mob)	
	{
	}
	protected virtual void OnAreaEntered(CashWalker cashWalker)
	{
	}

	protected virtual void Executed(CashWalker cashWalker) { }
	protected virtual void Executed(Mob mob) { }
	
	//ёбанный костыль
	protected virtual void _Executed(Mob mob)
	{
		if (mob.GetType() == typeof(Mob))
    {
      Executed((Mob)mob);
    }
    if (mob.GetType() == typeof(CashWalker))
    {
      Executed((CashWalker)mob);
    }
	}

	protected virtual TaskList _GetTasks()
	{
		TaskList taskList = new TaskList();
		taskList.Add(new MoveTask(this));
		return taskList;
	}

	protected virtual double _GetChanceTask(int mobSolvency)
	{
		double distanceFactor = Math.Abs((double)mobSolvency - (double)Solvency);
		distanceFactor = distanceFactor / _taskChancer_ReductorFactor + 1;
		return (1/distanceFactor) * _taskChancer_MaxChance;

	}
	/// <summary>
  /// С вероятностью определённой в _GetChanceTask() возвращает список задач, определённый в _GetTasks()
  /// </summary>
  /// <param name="mobSolvency">Состоятельность моба</param>
	public TaskList GetTaskList(int mobSolvency)
	{
		if (_rand.NextDouble() < _GetChanceTask(mobSolvency))
		{
			return _GetTasks();
		} 
		return new TaskList();
	}

}



