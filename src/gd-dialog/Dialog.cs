using Godot;
using System;
using GodotAnalysers;
using DodgeTheCreeps.Utils;

[SceneReference("Dialog.tscn")]
public partial class Dialog
{
    [Export]
    public int Speed;
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.timer.Connect(CommonSignals.Timeout, this, nameof(Timer_Timeout));
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && ((ButtonList)mouseEvent.ButtonMask).HasFlag(ButtonList.Left))
        {
            ForceFinish();
        }

        if (@event is InputEventKey keyEvent && keyEvent.Pressed && (((KeyList)keyEvent.Scancode).HasFlag(KeyList.Space) || ((KeyList)keyEvent.Scancode).HasFlag(KeyList.Escape)))
        {
            ForceFinish();
        }
    }

    protected void Timer_Timeout()
    {
        this.dialogTestLabel.VisibleCharacters++;
        if (this.dialogTestLabel.VisibleCharacters > this.dialogTestLabel.Text.Length)
        {
            this.timer.Stop();
        }
    }

    public void Show(string text, bool left)
    {
        this.Show();
        this.dialogTestLabel.Text = text;
        this.dialogTestLabel.VisibleCharacters = 0;
        if (left)
        {
            this.rightFace.Hide();
            this.leftFace.Show();
        }
        else
        {
            this.rightFace.Show();
            this.leftFace.Hide();
        }
        this.timer.Start();
    }

    public void ForceFinish()
    {
        if (this.dialogTestLabel.VisibleCharacters < this.dialogTestLabel.Text.Length)
        {
            this.dialogTestLabel.VisibleCharacters = this.dialogTestLabel.Text.Length;
            this.timer.Stop();
        }
        else
        {
            this.Hide();
        }
    }
}
