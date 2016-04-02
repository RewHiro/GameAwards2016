public class FadeState
{

    public enum State
    {
        IN,
        OUT,
        WAIT
    }

    public State state
    {
        get; set;
    }

    public bool isFade
    {
        get
        {
            return state != State.WAIT;
        }
    }

    public bool isFadeIn
    {
        get
        {
            return state == State.IN;
        }
    }

    public bool isFadeOut
    {
        get
        {
            return state == State.OUT;
        }
    }

    public FadeState()
    {
        state = State.WAIT;
    }
}