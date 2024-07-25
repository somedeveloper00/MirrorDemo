using System;

namespace MirrorDemo.Client.Common
{
    /// <summary>
    /// An interface for playing animations for <see cref="Container"/>
    /// </summary>
    public interface ISwitchContainerAnimation
    {
        void AddCompletedCallback(Action callback);
        bool IsPlaying();
        void Play();
        void Stop();
    }
}   