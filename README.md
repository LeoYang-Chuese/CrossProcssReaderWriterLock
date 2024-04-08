### [中文](https://github.com/LeoYang-Chuese/CrossProcssReaderWriterLock/blob/master/README.CN.md)


# CrossProcessWriteLock: A Simple Implementation of Cross-Process Write Lock

## Core Idea

In distributed systems or multi-process environments, it is often necessary to control concurrent access to shared resources to ensure data consistency and integrity. `CrossProcessWriteLock` is a simple and practical implementation of a cross-process write lock, which is based on the `Mutex` class in .NET and provides basic operations such as acquiring, releasing, and timeout.

The characteristic of a write lock is that only one process is allowed to have write permission at a time, and other processes can only wait. This mechanism can effectively avoid data corruption caused by concurrent writes.

## Usage

Here is a simple example:

```c#
// Create a write lock with the mutex name "my-mutex"
using (var writeLock = new CrossProcessWriteLock("my-mutex"))
{
    // Acquire the write lock, an exception will be thrown if the acquisition fails
    writeLock.Acquire();

    // Write your logic that requires the write lock here
    DoSomeWriteOperations();

    // Release the write lock
    writeLock.Release();
}
```

You can also use the `TryAcquire` method to attempt to acquire the write lock within a specified timeout:

```c#
// Try to acquire the write lock within 1 second
if (writeLock.TryAcquire(1000))
{
    try
    {
        // Perform write operations
        DoSomeWriteOperations();
    }
    finally
    {
        // Ensure to release the write lock
        writeLock.Release();
    }
}
else
{
    // Failed to acquire the write lock due to timeout
    Console.WriteLine("Failed to acquire the write lock.");
}
```

Note that if the current process already owns the write lock but tries to acquire it again, an `ApplicationException` exception will be thrown. Also, if the process holding the write lock terminates unexpectedly, other processes acquiring the write lock will throw an `AbandonedMutexException` exception.

In summary, `CrossProcessWriteLock` provides a simple and reliable implementation of a cross-process write lock, which is suitable for scenarios where shared resources need to be protected.



### todo:

ReadWriteLock
