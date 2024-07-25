using System;
using UnityEngine.Playables;

namespace MirrorDemo.Client.Common
{
    public sealed partial class Container
    {
        public readonly struct SwitchContainerAnimation_Timeline : ISwitchContainerAnimation
        {
            public readonly PlayableDirector playableDirector;

            public SwitchContainerAnimation_Timeline(PlayableDirector playableDirector) => this.playableDirector = playableDirector;
            public bool IsPlaying() => playableDirector.state == PlayState.Playing;
            public void Play() => playableDirector.Play();
            public void Stop() => playableDirector.Stop();

            public void AddCompletedCallback(Action callback)
            {
                var director = playableDirector;
                director.paused += _ =>
                {
                    if (director.time == director.duration)
                    {
                        callback();
                    }
                };
            }
        }
    }
}