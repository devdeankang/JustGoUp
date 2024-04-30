public interface IState<T>
{
    void Enter(T sender);
    void Update(T sender);
    void FixedUpdate(T sender);
    void Exit(T sender);
}