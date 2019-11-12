using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;

namespace OEPCommon
{
    public class StateMachine<T>
    {
        #region internal
        internal class State
        {
            event Action<object> onEnter;
            event Func<IFuture> onLeave;
            event Action<object> onUpdate;

            public State(Action<object> onEnter, Func<IFuture> onLeave, Action<object> onUpdate)
            {
                this.onEnter += onEnter;
                this.onLeave += onLeave;
                this.onUpdate += onUpdate;
            }
            
            public IFuture CallOnLeave()
            {
                if (onLeave != null)
                    return onLeave();

                return null;
            }

            public void CallOnEnter(object obj = null)
            {
                if (onEnter != null)
                    onEnter(obj);
            }

            public void CallOnUpdate(object obj = null)
            {
                if (onUpdate != null)
                    onUpdate(obj);
            }

            public void Clear()
            {
                onEnter = null;
                onLeave = null;
                onUpdate = null;
            }
        }
        #endregion

        public T currentState { get; private set; }
        public event Action<T, T, Object> onStateChange;
        public event Action<T, Object> onStateUpdate;
        private readonly Dictionary<T, State> _states = new Dictionary<T, State>();
        private IFuture _leaveFuture;

        public StateMachine(T startState)
        {
            currentState = startState;
        }
        
        public bool Check(T state)
        {
            return currentState.Equals(state);
        }

        public void Add(T state, Action<object> onEnter = null, Func<IFuture> onLeave = null, Action<object> onUpdate = null)
        {
            var newState = new State(onEnter, onLeave, onUpdate);
            _states.Add(state, newState);
        }

        State Get(T state)
        {
            if (!_states.ContainsKey(state))
                throw new Exception("State machine don't have " + state + " condition");
            return _states[state];
        }

        public void Remove(T state)
        {
            State removeState = Get(state);
            if (currentState.Equals(state))
            {
                removeState.CallOnLeave();
                removeState.Clear();
            }
            _states.Remove(state);
        }

        public void Clear(bool leaveSignal = false)
        {
            if (_states.Count == 0) return;

            if (_states.ContainsKey(currentState) && leaveSignal)
                _states[currentState].CallOnLeave();
            
            foreach (var state in _states.Values)
                state.Clear();
            
            _states.Clear();
        }
        
        public void SetState(T state, object obj = null)
        {
            if (_leaveFuture != null)
                return;

            if (_states.ContainsKey(currentState))
            {
                if (currentState.Equals(state))
                {
                    onStateUpdate?.Invoke(state, obj);
                    _states[currentState].CallOnUpdate(obj);
                    return;
                }
                _leaveFuture = _states[currentState].CallOnLeave();
            }

            var prevState = currentState; 
            currentState = state;

            if (_states.ContainsKey(currentState))
            {
                if (_leaveFuture != null)
                {
                    _leaveFuture.AddListener(future =>
                    {
                        _leaveFuture = null;
                        onStateChange?.Invoke(prevState, state, obj);
                        _states[currentState].CallOnEnter(obj);
                    });
                }
                else
                {
                    onStateChange?.Invoke(prevState, state, obj);
                    _states[currentState].CallOnEnter(obj);
                }
            }
        }
    }
}
