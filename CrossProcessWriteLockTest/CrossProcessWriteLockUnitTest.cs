using CrossProcssReaderWriterLock;

namespace CrossProcessWriteLockTest
{
    public class CrossProcessWriteLockUnitTest
    {
        [Fact]
        public void Acquire_UnnamedMutex_ShouldSucceed()
        {
            using (var writeLock = new CrossProcessWriteLock(Guid.NewGuid().ToString()))
            {
                writeLock.Acquire();
                // 在这里进行其他测试断言
            }
        }

        [Fact]
        public void Acquire_NamedMutex_ShouldSucceed()
        {
            var mutexName = "test-mutex";
            using (var writeLock = new CrossProcessWriteLock(mutexName))
            {
                writeLock.Acquire();
                // 在这里进行其他测试断言
            }
        }

        [Fact]
        public void TryAcquire_WithTimeout_ShouldSucceed()
        {
            var mutexName = "test-mutex";
            using (var writeLock = new CrossProcessWriteLock(mutexName))
            {
                Assert.True(writeLock.TryAcquire(1000));
                // 在这里进行其他测试断言
            }
        }

        [Fact]
        public void TryAcquire_WithNegativeTimeout_ShouldThrowArgumentOutOfRangeException()
        {
            var mutexName = "test-mutex";
            using (var writeLock = new CrossProcessWriteLock(mutexName))
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => writeLock.TryAcquire(-2));
            }
        }

        [Fact]
        public void Release_UnownedMutex_ShouldThrowApplicationException()
        {
            var mutexName = "test-mutex";
            using (var writeLock = new CrossProcessWriteLock(mutexName))
            {
                Assert.Throws<ApplicationException>(() => writeLock.Release());
            }
        }

        [Fact]
        public void Dispose_ReleasesResource()
        {
            var mutexName = "test-mutex";
            var writeLock = new CrossProcessWriteLock(mutexName);
            writeLock.Acquire();
            writeLock.Dispose();

            using (var newWriteLock = new CrossProcessWriteLock(mutexName))
            {
                newWriteLock.Acquire();
                // 如果资源未释放,这里会抛出异常
            }
        }
    }
}