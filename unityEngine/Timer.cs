using System;
using System.Collections.Generic;
using OEPFramework.unityEngine.loop;
using OEPFramework.unityEngine.utils;
using OEPFramework.unityEngine._base;
using UnityEngine;

namespace OEPFramework.unityEngine
{
    public sealed class Timer
    {
        public static string EVENT_TIMER_PAUSE = GEvent.GetUniqueCategory();

        public delegate void OnTimeUp(object o);
        public delegate void OnTimeUpVoid();
        private static readonly List<Timer> timers = new List<Timer>();

        private float timeStep;
        private float timeElapsed;
        public OnTimeUp onTimeUpEvent;
        public OnTimeUpVoid onTimeUpVoidEvent;
        private object firedObj;
        private bool work;
        private bool once;
        private static DateTime pauseTime;
        private static DateTime lastTime;
        private bool realtime;
        private IDroppableItem dropper;

        static Timer()
        {
            lastTime = DateTime.UtcNow;
        }

        void Call()
        {
            if (onTimeUpEvent != null)
                onTimeUpEvent(firedObj);

            if (onTimeUpVoidEvent != null)
                onTimeUpVoidEvent();
        }

        public void Pause()
        {
            work = false;
        }

        public void Reset()
        {
            timeElapsed = 0;
        }

        public void Resume()
        {
            work = true;
        }

        public static Timer Create(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false)
        {
            return new Timer(sec, func, obj, dropper, once);
        }

        public static Timer Create(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false)
        {
            return new Timer(sec, func, dropper, once);
        }

        public static Timer CreateRealtime(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false)
        {
            return new Timer(sec, func, obj, dropper, once, true);
        }

        public static Timer CreateRealtime(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false)
        {
            return new Timer(sec, func, dropper, once, true);
        }

        private Timer(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false, bool realtime = false)
        {
            Init(sec, null, func, obj, dropper, once, realtime);
        }

        private Timer(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false, bool realtime = false)
        {
            Init(sec, func, null, null, dropper, once, realtime);
        }

        private void Init(float sec, OnTimeUpVoid funcVoid, OnTimeUp func, object obj, IDroppableItem timerDropper, bool onceCall, bool realtimeTimer)
        {
            if (funcVoid != null)
                onTimeUpVoidEvent = funcVoid;

            if (func != null)
                onTimeUpEvent = func;

            once = onceCall;
            realtime = realtimeTimer;
            firedObj = obj;

            if (Mathf.Approximately(sec, 0))
            {
                Call();

                if (once)
                {
                    onTimeUpEvent = null;
                    onTimeUpVoidEvent = null;
                    return;
                }
            }

            timeStep = sec;
            work = true;

            if (realtime)
                GEvent.Attach(EVENT_TIMER_PAUSE, OnPause, null);

            if (timerDropper != null)
            {
                dropper = timerDropper;
                dropper.onDrop += Drop;
            }

            SyncHelper.Add(() => timers.Add(this), Loops.TIMER);
        }

        private void OnPause(object pause)
        {
            if ((bool)pause)
                pauseTime = DateTime.UtcNow;
            else
                timeElapsed += (float)(DateTime.UtcNow - pauseTime).TotalSeconds;
        }

        public static void Process()
        {
            SyncHelper.Process(Loops.TIMER);
            var now = DateTime.UtcNow;
            var dt = Time.deltaTime;
            var dtReal = (float)(now - lastTime).TotalSeconds;
            lastTime = now;

            foreach (var task in timers)
                task.TimerProcess(dt, dtReal);
        }

        private void TimerProcess(float dt, float dtReal)
        {
            if (!work) return;
            timeElapsed += !realtime ? dt : dtReal;
            if (timeElapsed >= timeStep)
            {
                timeElapsed -= timeStep;

                Call();

                if (once)
                    Drop();
            }
        }

        public void Drop()
        {
            Pause();

            onTimeUpEvent = null;
            onTimeUpVoidEvent = null;

            if (realtime)
                GEvent.Detach(EVENT_TIMER_PAUSE, OnPause);

            if (dropper != null)
                dropper.onDrop -= Drop;
            dropper = null;

            SyncHelper.Add(() => timers.Remove(this), Loops.TIMER);
        }
    }
}
