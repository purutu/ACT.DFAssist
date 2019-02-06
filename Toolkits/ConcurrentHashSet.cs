using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace ACT.DFAssist.Toolkits
{
    public class ConcurrentHashSet<T> : IDisposable, ICollection<T>, ISet<T>, ISerializable, IDeserializationCallback
    {
        private readonly HashSet<T> _hs = new HashSet<T>();
        private readonly ReaderWriterLockSlim _l = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public ConcurrentHashSet()
        {
        }

        public ConcurrentHashSet(IEqualityComparer<T> comparer)
        {
            _hs = new HashSet<T>(comparer);
        }

        public ConcurrentHashSet(IEnumerable<T> collection)
        {
            _hs = new HashSet<T>(collection);
        }

        public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            _hs = new HashSet<T>(collection, comparer);
        }

        protected ConcurrentHashSet(SerializationInfo info, StreamingContext context)
        {
            _hs = new HashSet<T>();

            var iSerializable = _hs as ISerializable;
            iSerializable.GetObjectData(info, context);
        }

        ~ConcurrentHashSet()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool d)
        {
            if (d)
            {
                if (_l != null)
                    _l.Dispose();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _hs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void OnDeserialization(object sender)
        {
            _hs.OnDeserialization(sender);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            _hs.GetObjectData(info, context);
        }

        public int Count
        {
            get
            {
                _l.EnterReadLock();
                try
                {
                    return _hs.Count;
                }
                finally
                {
                    if (_l.IsReadLockHeld)
                        _l.ExitReadLock();
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Contains(T i)
        {
            _l.EnterReadLock();
            try
            {
                return _hs.Contains(i);
            }
            finally
            {
                if (_l.IsReadLockHeld)
                    _l.ExitReadLock();
            }
        }

        public bool Add(T i)
        {
            _l.EnterWriteLock();
            try
            {
                return _hs.Add(i);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        void ICollection<T>.Add(T item)
        {
            _l.EnterWriteLock();
            try
            {
                _hs.Add(item);
            }
            finally
            {
                if (_l.IsWriteLockHeld) _l.ExitWriteLock();
            }
        }

        bool ISet<T>.Add(T item)
        {
            _l.EnterWriteLock();
            try
            {
                return _hs.Add(item);
            }
            finally
            {
                if (_l.IsWriteLockHeld) _l.ExitWriteLock();
            }
        }

        public bool Remove(T i)
        {
            _l.EnterWriteLock();
            try
            {
                return _hs.Remove(i);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        public void Clear()
        {
            _l.EnterWriteLock();
            try
            {
                _hs.Clear();
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _l.EnterWriteLock();
            try
            {
                _hs.CopyTo(array, arrayIndex);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            _l.EnterWriteLock();
            _l.EnterReadLock();
            try
            {
                _hs.UnionWith(other);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
                if (_l.IsReadLockHeld)
                    _l.ExitReadLock();
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            _l.EnterWriteLock();
            _l.EnterReadLock();
            try
            {
                _hs.IntersectWith(other);
            }
            finally
            {
                if (_l.IsWriteLockHeld) _l.ExitWriteLock();
                if (_l.IsReadLockHeld) _l.ExitReadLock();
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            _l.EnterWriteLock();
            _l.EnterReadLock();
            try
            {
                _hs.ExceptWith(other);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
                if (_l.IsReadLockHeld)
                    _l.ExitReadLock();
            }
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            _l.EnterWriteLock();
            try
            {
                _hs.SymmetricExceptWith(other);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            _l.EnterWriteLock();
            try
            {
                return _hs.IsSubsetOf(other);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            _l.EnterWriteLock();
            try
            {
                return _hs.IsSupersetOf(other);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            _l.EnterWriteLock();
            try
            {
                return _hs.IsProperSupersetOf(other);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            _l.EnterWriteLock();
            try
            {
                return _hs.IsProperSubsetOf(other);
            }
            finally
            {
                if (_l.IsWriteLockHeld)
                    _l.ExitWriteLock();
            }
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            _l.EnterReadLock();
            try
            {
                return _hs.Overlaps(other);
            }
            finally
            {
                if (_l.IsReadLockHeld)
                    _l.ExitReadLock();
            }
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            _l.EnterReadLock();
            try
            {
                return _hs.SetEquals(other);
            }
            finally
            {
                if (_l.IsReadLockHeld)
                    _l.ExitReadLock();
            }
        }
    }
}
