namespace GameEngine.engine.game_object.components.animation.composition; 

public class Composition {
    public readonly int lastStateEndStep;
    private readonly Func<GameObject, Component> _retriever;
    private readonly Dictionary<int, State> _states = new();
    private Component? _subject;

    public Composition(Func<GameObject, Component> retriever, IEnumerable<State> states) {
        _retriever = retriever;
        int currentTimeStep = 0;
        foreach (State state in states) {
            _states.Add(currentTimeStep, state);
            currentTimeStep += state.timeUnitWaitAfter;
        }

        lastStateEndStep = currentTimeStep;
    }

    public void Init(GameObject gameObject) {
        _subject = _retriever.Invoke(gameObject);
    }
    
    public void Run(int timeStep) {
        if (_subject != null && _states.TryGetValue(timeStep, out State state)) {
            state.action.Invoke(_subject);
        }
    }
}