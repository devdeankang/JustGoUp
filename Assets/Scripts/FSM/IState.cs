public interface IState<T>
{
    void Enter(T sender);
    void Update(T sender);
    void FixedUpdate(T sender);
    void Exit(T sender);
    string GetName();
}

public abstract class State<T> : IState<T> where T : class
{
    public abstract void Enter(T player);
    public abstract void Update(T player);
    public abstract void FixedUpdate(T player);
    public abstract void Exit(T player);

    public string GetName()
    {
        return this.GetType().Name;
    }
}