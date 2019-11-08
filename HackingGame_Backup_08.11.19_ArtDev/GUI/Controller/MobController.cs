using Godot;
using System;

/*
  тестовый контроллер
  нужно прописать патч с мобами


  Замечал багло, что появляется 2 селектора, крайне редко
 */
public class MobController : Node2D
{ 
  const string _pathToNodeMobs = "../MobGenerator/CashWalkers";
  const float _clickRegion = 60;

  private Vector2 _mousePosition;
  
  private PackedScene _mobSelectorLoad;
  private MobSelector _activeSelector;
  private Mob SelectedMob
  {
    set
    {
      if ((_activeSelector != null)&&(_activeSelector.Focused)) 
      {
        return;
      }

      _activeSelector?.Close();
      if (value != null)
      {
        InstanceMobSelector(value);
      }
    }
  }

  public override void _Ready()
  {
    if (!HasNode(_pathToNodeMobs))
    {
      SetProcessInput(false);
      GD.Print("Не могу найти путь до мобов, которых хочу обрабатывать. Кстате я контролер, у меня есть жена, дети, мне хорошо платят, правда, не в биткоинах, но меня устраивает. Хочу вот памяти себе купить, чувствуется подводит. Бывает иногда double переменные забываю, только начальницу не говори. Ах! Да! Про оброботку мобов. Поработай с константой _pathToNodeMobs и пропиши туда путь, где мне обрабатывать мобов. Целую :) ");
    }
    else
    {
      _mobSelectorLoad = (PackedScene)GD.Load("res://GUI/MobSelector/MobSelector.tscn");
    }
      
  }
  public override void _Input(InputEvent @event)
  {
    if (!@event.IsActionPressed("Tap")) return;
    if (@event is InputEventMouseButton mouseEvent)
      TouchController(mouseEvent);
  }

  //обработка клика мыши
  private void TouchController(InputEventMouseButton mouseEvent)
  {
    _mousePosition = mouseEvent.Position;
    SelectedMob = GetNearestMobInRegion();
  }


  private void InstanceMobSelector(Mob mob)
  {
    _activeSelector = (MobSelector)_mobSelectorLoad.Instance();
    _activeSelector.FollowMob = mob;
  
    SetPropertyMobSelector(mob);

    _activeSelector.Connect("Closed", this, nameof(OnSelectorClosed));
    AddChild(_activeSelector);
  }
  public void SetPropertyMobSelector(Mob mob)
  {
    if (mob.GetType() == typeof(Mob))
    {
      _SetPropertyMobSelector((Mob)mob);
    }
    if (mob.GetType() == typeof(CashWalker))
    {
      _SetPropertyMobSelector((CashWalker)mob);
    }
  }

  private void _SetPropertyMobSelector(Mob mob)
  {
  }

 
  private void _SetPropertyMobSelector(CashWalker cashWalker)
  {
    _activeSelector.AddProperty("Money", cashWalker.Money.ToString());
    _activeSelector.AddProperty("Solvency", cashWalker.Solvency.ToString());
  }
  //поиск и возврат ближайшего моба к клику
  private Mob GetNearestMobInRegion()
  {
    Mob nearestMob = null;
    foreach(Mob mob in GetNode<Node2D>(_pathToNodeMobs).GetChildren())
    {
      if (nearestMob == null)
      {
        if (_mousePosition.DistanceTo(mob.Position) < _clickRegion)
        {
          nearestMob = mob;
        }
      } 
      else
      {
        if (_mousePosition.DistanceTo(mob.Position) < _mousePosition.DistanceTo(nearestMob.Position))
        {
          nearestMob = mob;
        }
      }
    }
    return nearestMob;
  }
  private void OnSelectorClosed()
  {
    _activeSelector = null;
  }


}
