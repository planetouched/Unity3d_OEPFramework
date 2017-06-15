using System;
using System.Collections.Generic;
using OEPFramework.common.future;

namespace OEPFramework.common
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
        readonly Dictionary<T, State> states = new Dictionary<T, State>();
        private IFuture leaveFuture;

        public StateMachine(T startState)
        {
            currentState = startState;
        }
        
        public StateMachine()
        {
        }

        public bool Check(T state)
        {
            return currentState.Equals(state);
        }

        public void Add(T state, Action<object> onEnter = null, Func<IFuture> onLeave = null, Action<object> onUpdate = null)
        {
            var newState = new State(onEnter, onLeave, onUpdate);
            states.Add(state, newState);
        }

        State Get(T state)
        {
            if (!states.ContainsKey(state))
                throw new Exception("State machine don't have " + state + " condition");
            return states[state];
        }

        public void Remove(T state)
        {
            State removeState = Get(state);
            if (currentState.Equals(state))
            {
                removeState.CallOnLeave();
                removeState.Clear();
            }
            states.Remove(state);
        }

        public void Clear(bool leaveSignal = false)
        {
            if (states.Count == 0) return;

            if (states.ContainsKey(currentState) && leaveSignal)
                states[currentState].CallOnLeave();
            
            foreach (var state in states.Values)
                state.Clear();
            
            states.Clear();
        }
        
        public void SetState(T state, object obj = null)
        {
            if (leaveFuture != null)
                return;

            if (states.ContainsKey(currentState))
            {
                if (currentState.Equals(state))
                {
                    states[currentState].CallOnUpdate(obj);
                    return;
                }
                leaveFuture = states[currentState].CallOnLeave();
            }

            currentState = state;

            if (states.ContainsKey(currentState))
            {
                if (leaveFuture != null)
                {
                    leaveFuture.AddListener(future =>
                    {
                        leaveFuture = null;
                        states[currentState].CallOnEnter(obj);
                    });
                }
                else
                    states[currentState].CallOnEnter(obj);
            }
        }
    }
}
