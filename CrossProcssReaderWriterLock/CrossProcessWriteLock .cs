namespace CrossProcssReaderWriterLock
{
    /// <summary>
    /// 表示跨进程写操作锁的封装类。
    /// Represents an implementation of a cross-process write lock.
    /// </summary>
    public sealed class CrossProcessWriteLock : IDisposable
    {
        private readonly Mutex _mutex;
        private volatile bool _isDisposed;

        /// <summary>
        /// 初始化一个新的 <see cref="CrossProcessWriteLock" /> 实例。
        /// Initializes a new instance of the <see cref="CrossProcessWriteLock" /> class.
        /// </summary>
        /// <param name="name">互斥体的名称。 The name of the mutex.</param>
        /// <exception cref="ArgumentException">当 <paramref name="name" /> 为 null 或空白字符串时抛出。 Thrown when <paramref name="name" /> is null or empty.</exception>
        public CrossProcessWriteLock(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Mutex name cannot be null or whitespace.", nameof(name));
            }

            _mutex = new Mutex(false, name);
        }

        /// <summary>
        /// 获取互斥体的拥有权。
        /// Acquires the ownership of the mutex.
        /// </summary>
        /// <exception cref="ObjectDisposedException">当实例已被释放时抛出。 Thrown when the instance has been disposed.</exception>
        /// <exception cref="AbandonedMutexException">当持有互斥体的进程终止时抛出。 Thrown when the process holding the mutex has terminated.</exception>
        public void Acquire()
        {
            ThrowIfDisposed();
            _mutex.WaitOne();
        }

        /// <summary>
        /// 尝试在指定的超时时间内获取互斥体的拥有权。
        /// Attempts to acquire the ownership of the mutex within the specified timeout.
        /// </summary>
        /// <param name="millisecondsTimeout">超时时间（以毫秒为单位）。 The time-out interval in milliseconds.</param>
        /// <returns>如果成功获取互斥体的拥有权，则为 <see langword="true" />；否则为 <see langword="false" />。 True if the current instance successfully acquires the ownership of the mutex; otherwise, false.</returns>
        /// <exception cref="ObjectDisposedException">当实例已被释放时抛出。 Thrown when the instance has been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="millisecondsTimeout" /> 小于零且不等于 <see cref="Timeout.Infinite" /> 时抛出。 Thrown when <paramref name="millisecondsTimeout" /> is negative and not equal to <see cref="Timeout.Infinite" />.</exception>
        public bool TryAcquire(int millisecondsTimeout)
        {
            ThrowIfDisposed();

            if (millisecondsTimeout < 0 && millisecondsTimeout != Timeout.Infinite)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout), "Timeout must be non-negative or Timeout.Infinite.");
            }

            return _mutex.WaitOne(millisecondsTimeout);
        }

        /// <summary>
        /// 释放互斥体的拥有权。
        /// Releases the ownership of the mutex.
        /// </summary>
        /// <exception cref="ObjectDisposedException">当实例已被释放时抛出。 Thrown when the instance has been disposed.</exception>
        /// <exception cref="ApplicationException">当当前实例不拥有互斥体时抛出。 Thrown when the current instance does not own the mutex.</exception>
        public void Release()
        {
            ThrowIfDisposed();
            _mutex.ReleaseMutex();
        }

        /// <summary>
        /// 释放互斥体的拥有权并释放资源。
        /// Releases the ownership of the mutex and disposes the resources.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _mutex.Dispose();
                _isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(CrossProcessWriteLock));
            }
        }
    }
}