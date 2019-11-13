using Godot;
using System;

public class FollowTask : MoveTask
{
  private Mob _followMob;
  private bool _infinityFollow;
  public FollowTask(Mob mob, float stoppedDistance = 100, bool infinityFollow = true):base(mob)
  {
    _followMob = mob;
    _stoppedDistance = stoppedDistance;
    _infinityFollow = infinityFollow;
  }

  public override void Act()
  {
    _path = _navigation.GetSimplePath(mob.Position, _followMob.Position, true);
    _idx = 0;
    base.Act();
  }

  public override bool Trigger()
  {
    if (_infinityFollow) return false;
    return base.Trigger();
  }
}
