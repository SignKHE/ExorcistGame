using System;

namespace ExorcistGame.UI
{
    /// <summary>
    /// UI View가 바인딩하는 ViewModel
    /// </summary>
    public abstract class ViewModelBase : IDisposable
    {
        public void Dispose()
        {
            DisposeReactiveProperty();
        }

        /// <summary>
        /// ViewModel이 Dispose될때 리액트 프로퍼티를 함께 Dispose 하기 위한 기능
        /// </summary>
        protected abstract void DisposeReactiveProperty();
    }
}