using Godot;
using GodotAnalysers;

[SceneReference("CountingLabel.tscn")]
[Tool]
public partial class CountingLabel
{
    private float passedTime;
    private int startValue;
    private int endValue;
    private SceneTreeTween tween;
    private bool isReady = false;

    public int CurrentValue
    {
        get
        {
            if (passedTime > this.AnimationTime || this.tween == null)
            {
                return endValue;
            }

            return (int)this.tween.InterpolateValue(
                startValue,
                endValue - startValue,
                passedTime,
                this.AnimationTime,
                Tween.TransitionType.Linear,
                Tween.EaseType.In);
        }
    }

    [Export]
    public int Value
    {
        get
        {
            return endValue;
        }
        set
        {
            startValue = this.CurrentValue;
            endValue = value;
            passedTime = 0;
        }
    }

    [Export]
    public int AnimationTime { get; set; } = 2;

    public override void _Ready()
    {
        if (!isReady)
        {
            isReady = true;
            base._Ready();
            this.FillMembers();
            this.tween = this.CreateTween();
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        this._Ready();
        passedTime += delta;
        this.label.Text = this.CurrentValue.ToString();
    }
}
