using Godot;
using System;
using System.Collections.Generic;


interface iTaskList
{
	void Next(); //Перейти к следующей задаче
	void Act(); //Выполнение процесса (должно использоваться в _PhysicsProgress)
	void Add(Task task); //Добавить задачу в конец списка
	void Interception(Task task); //перехватить задачу
	void Start(); //Запуск задач
	void Delete(int pos); //удалить задачу в позиции
	void Insert(Task task, int pos); //вставка задачи в определённую позицию
	//int AddGroupTask(Task task, int IdGroup = 0); //вставка зависимой задачи
}

	/// <summary>
  ///  Список задач
  /// </summary>
public class TaskList : iTaskList
{
	public List<Task> list = new List<Task>();
	/// <summary>
  ///  Ссылка на моба, выполнящего задачи
  /// </summary>
	private Mob mob;
	
	/// <summary>
  ///  Количество задач
  /// </summary>
	public int Count
	{
		get
		{
			return list.Count;
		}
	} 

	
		
	/// <summary>
  /// Удаление текущей задачи
	/// и переход к
  /// </summary>
	public void Next()
	{
		//завершаем предыдущую задачу
		list[0].End();
		
		//удаляем текущую задачу
		list.RemoveAt(0);
		
		
		//если задачи остались, то распаковываем следующую
		//иначе останавливаем процесс
		if (Count > 0)
		{
			list[0].Start();
		}
		else 
		{
			mob.SetPhysicsProcess(false);
			mob.QueueFree();
		}
	}
	
	/// <summary>
  ///  Выполнение процесса (должно использоваться в _PhysicsProgress)
  /// </summary>
	public void Act()
	{
		list[0].Act();
			
		if (list[0].Trigger())
			Next();
	}
	
	/// <summary>
  /// Добавить задачу
  /// </summary>
	public void Add(Task task)
	{
		if (task == null) return;
		Insert(task, Count);
		
		//если это первая задача и есть привязка листа к мобу то стартуем
		if (Count == 1)
			Start();
		
	}
	/// <summary>
	///Перехват задачи - 
	///мгновенно начать выполнять поставленную задачу,
	///а предыдущую удалить из списка
	/// </summary>
	/// <param name="task">Задача Например: MoveTask, WaitTask</param>
	public void Interception(Task task)
	{
		Insert(task, 1);
		Next();
	}
	/// <summary>
	///Перехват задачи - 
	///мгновенно начать выполнять поставленную задачу,
	///а предыдущую оставить на потом
	/// </summary>
	/// <param name="task">Задача Например: MoveTask, WaitTask</param>
	public void InterceptionWithSaveTask(Task task)
	{
		Insert(list[0],1);
		Interception(task);
	}
	
	/// <summary>
	///Установка ссылок на моба. 
	///Не рекомендуется использовать как интерфейс!
	///метод необходим для сеттера
	/// </summary>
	/// <param name="settedMob">Ссылка на моба, с которым должны происходить задачи</param>
	public void _setMobLink(Mob settedMob)
	{
		this.mob = settedMob;
		for (int i = 0; i < Count; i++)
			list[i].mob = settedMob;
	}
	
	/// <summary>
	/// Запуск выполнения задач. 
	/// Задачи не запустятся если:
	/// 1) их нет;
	/// 2) нет ссылки на моба;
	/// 3) моб не инстансирован (не помещён в активную зону).
	/// </summary>
	public void Start()
	{
		if ((mob != null) && (Count > 0) && (mob.IsInsideTree()))
		{
			list[0].Start();
			mob.SetPhysicsProcess(true);
		}
	}

	/// <summary>
	/// Удалить задачу
	/// </summary>
	/// <param name="pos">Позиция удаляемой задачи</param>
	public void Delete(int pos)
	{
		if (pos < Count)
		{
			if (pos == 0)
			{
				Next();
			}
			else 
			{
				list.RemoveAt(pos);
			}
		}
		else
		{
			GD.PrintErr("Попытка удалить несуществующую задачу с позицией: " + pos);
		}
	}


	/// <summary>
	/// Удаление всех задач по заданному привязанному ID
	/// </summary>
	/// <param name="searchBoundId">привязанный идентификатор задачи</param>
	//НЕЗАКОНЧЕННЫЙ МЕТОД
	public void DeleteById(int searchBoundId)
	{
		for(int i = 0; i < Count; i++)
		{
			if (list[i].boundId == searchBoundId)
				Delete(i);
		}
	}
	public void Insert(Task task, int pos)
	{
		task.mob = mob;
		list.Insert(pos, task);	
	}

	public static TaskList operator + (TaskList firstTL, TaskList secondTL)
	{
		if (firstTL == null) firstTL = new TaskList();
		foreach(Task oneTask in secondTL.list)
		{
			firstTL.Add(oneTask);
		}
		return firstTL;
	}
}
