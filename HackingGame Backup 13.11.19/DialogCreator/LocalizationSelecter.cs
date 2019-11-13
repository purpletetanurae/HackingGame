using Godot;
using System;
using System.Collections.Generic;

public class LocalizationSelecter : Control
{
  private PopupMenu _popupMenu;
  private List<string> _locals;
  public override void _Ready()
  {
    MenuButton menuButton = GetNode<MenuButton>("MenuButton");
    _popupMenu = menuButton.GetPopup();
    _popupMenu.Connect("id_pressed", this, nameof(OnItemPressed));
    _locals = PhraseLocale.GetSupportedLocals();
    CreateSelectList();

  }
  private void CreateSelectList()
  {
    foreach(string lang in _locals)
    {
      _popupMenu.AddItem(lang);
    }
  }
  private void AddSelect(string lang)
  {
    Button button = new Button();
    button.Text = lang;
    GetNode<VBoxContainer>("Control/VBoxContainer").AddChild(button);
  }
  public void OnItemPressed(int id)
  {
    GD.Print("OnItemPressed");
    PhraseLocaleDev.SetLocale(_locals[id]);
    GetNode<DialogCreator>("..").ReView();
  }
}
