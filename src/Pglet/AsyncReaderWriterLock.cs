using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet
{
    // Source: https://github.com/copenhagenatomics/CA_DataUploader/blob/master/CA_DataUploaderLib/AsyncReaderWriterLock.cs
    // async compatible reader / writer lock based on https://stackoverflow.com/a/64757462/66372
    // it doesn't have a lot of protection, so keep its usage simple i.e. blocks where try/finally can be used and no recursion can happen
    // How it works:
    //  - in general, a reader/writer lock allows any amount of readers to enter the lock while only a single writer can do so. While the writer holds the lock, no reader can hold the lock
    //  - 2 semaphores + a count of readers in the lock are used to provide the above guarantees
    //  - to guarantee no new readers or writers can enter the lock while a writer is active, a write semaphore is used
    //    - both readers and writers acquire this semaphore first when trying to take the lock
    //    - readers release the semaphore just after acquiring the read lock, so more readers can enter the lock (so technically acquiring of readers locks do not happens in parallel)
    //  - to guarantee the writer does not enter the lock while there are still readers in the lock, a read semaphore is used
    //    - both the writer and the first reader acquire this semaphore when trying to take the lock. They do it *after* they hold the write semaphore
    //    - the last active reader holding the lock, releases the read semaphore. Note it does not need to be the reader that acquired it first.
    //    - to track a reader acquiring/releasing a lock is the first/last one, a reader count is tracked when acquiring/releasing the read lock.
    //  - cancellation tokens are supported so that readers/writers can abort while waiting for an active writer to finish its job.
    public sealed class AsyncReaderWriterLock : IDisposable
    {
        readonly SemaphoreSlim _readSemaphore = new SemaphoreSlim(1, 1);
        readonly SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1, 1);
        int _readerCount;

        public class WriterLock : IDisposable
        {
            AsyncReaderWriterLock _asyncLock;
            public WriterLock(AsyncReaderWriterLock asyncLock)
            {
                _asyncLock = asyncLock;
            }

            public void Release()
            {
                _asyncLock.ReleaseWriterLock();
            }

            public void Dispose()
            {
                Release();
            }
        }

        public class ReaderLock : IDisposable
        {
            AsyncReaderWriterLock _asyncLock;
            public ReaderLock(AsyncReaderWriterLock asyncLock)
            {
                _asyncLock = asyncLock;
            }

            public void Release()
            {
                _asyncLock.ReleaseReaderLock();
            }

            public void Dispose()
            {
                Release();
            }
        }

        public WriterLock AcquireWriterLock(CancellationToken token = default)
        {
            _writeSemaphore.Wait(token);
            try
            {
                _readSemaphore.Wait(token);
                return new WriterLock(this);
            }
            catch
            {
                _writeSemaphore.Release();
                throw;
            }
        }

        public async Task<WriterLock> AcquireWriterLockAsync(CancellationToken token = default)
        {
            await _writeSemaphore.WaitAsync(token).ConfigureAwait(false);
            try
            {
                await _readSemaphore.WaitAsync(token).ConfigureAwait(false);
                return new WriterLock(this);
            }
            catch
            {
                _writeSemaphore.Release();
                throw;
            }
        }

        public void ReleaseWriterLock()
        {
            _readSemaphore.Release();
            _writeSemaphore.Release();
        }

        public ReaderLock AcquireReaderLock(CancellationToken token = default)
        {
            _writeSemaphore.Wait(token);

            if (Interlocked.Increment(ref _readerCount) == 1)
            {
                try
                {
                    _readSemaphore.Wait(token);
                }
                catch
                {
                    Interlocked.Decrement(ref _readerCount);
                    _writeSemaphore.Release();
                    throw;
                }
            }

            _writeSemaphore.Release();
            return new ReaderLock(this);
        }

        public async Task<ReaderLock> AcquireReaderLockAsync(CancellationToken token = default)
        {
            await _writeSemaphore.WaitAsync(token).ConfigureAwait(false);

            if (Interlocked.Increment(ref _readerCount) == 1)
            {
                try
                {
                    await _readSemaphore.WaitAsync(token).ConfigureAwait(false);
                }
                catch
                {
                    Interlocked.Decrement(ref _readerCount);
                    _writeSemaphore.Release();
                    throw;
                }
            }

            _writeSemaphore.Release();
            return new ReaderLock(this);
        }

        public void ReleaseReaderLock()
        {
            if (Interlocked.Decrement(ref _readerCount) == 0)
            {
                _readSemaphore.Release();
            }
        }

        public void Dispose()
        {
            _writeSemaphore.Dispose();
            _readSemaphore.Dispose();
        }

        public override string ToString()
        {
            return GetHashCode().ToString();
        }
    }
}
