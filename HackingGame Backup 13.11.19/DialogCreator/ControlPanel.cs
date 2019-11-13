using Godot;
using System;

public class ControlPanel : Control
{
  private bool _posPhraseOpened = false; 
  ///<summary>
  ///Скрывает кнопки, которые не нужны при занятой ячейке при exist = true.
  ///Скрывает кнопки, которые не нужны при свободной ячейке при exist = false.
  ///</summary>
  public void SetDisableds(bool exist)
  {
    GetNode<Button>("Add").Disabled = exist;
    GetNode<Button>("Delete").Disabled = !exist;
    GetNode<Button>("AddPossiblePhrase").Disabled = !exist;
    GetNode<Button>("AddSolvencyText").Disabled = !exist;
  }
  ///<summary>
  ///Управление св-вом Disabled у кнопки save
  ///</summary>
  public void SetSaveDisable(bool needed)
  {
    GetNode<Button>("Save").Disabled = needed;
  }
  public void OnAddPossiblePhraseMouseEntered()
  {
    if (_posPhraseOpened == true) return;
    GetNode<AnimationPlayer>("AnimationPlayer").Play("Show");
    _posPhraseOpened = true;
  }
  public void OnPanelClouse()
  {
    if (_posPhraseOpened == true)
    {
      GetNode<AnimationPlayer>("AnimationPlayer").Play("Hide");
      _posPhraseOpened = false;
    }
    
  }
}
