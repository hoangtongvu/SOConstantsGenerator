// This file is auto-generated, do not change.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Core;

public static class GameBalance
{
    public static readonly Unity.Collections.FixedString64Bytes StringValue =
        @"Bambo";
    public const System.Single UnmanangedPrimitiveValue = 4.5f;
    public static readonly Cores.UserData.Unmanaged UserData =
        new Cores.UserData.UnmanagedConverter().Convert(new()
        {
            NameInfo = new Cores.UserNameInfo()
            {
                FamilyName = @"Doe",
                LastName = @"John",
            },
            Age = 35,
            Balance = 1000,
        });
    public static readonly Cores.UserData.Unmanaged[] UserDataArray = new Cores.UserData.Unmanaged[]
    {
        new Cores.UserData.UnmanagedConverter().Convert(new()
        {
            NameInfo = new Cores.UserNameInfo()
            {
                FamilyName = @"Hap",
                LastName = @"Po",
            },
            Age = 80,
            Balance = 100,
        }),
        new Cores.UserData.UnmanagedConverter().Convert(new()
        {
            NameInfo = new Cores.UserNameInfo()
            {
                FamilyName = @"Li",
                LastName = @"Li",
            },
            Age = 15,
            Balance = 0,
        }),
    };
    public static readonly Cores.UserData.Unmanaged[] UserDataList = new Cores.UserData.Unmanaged[]
    {
        new Cores.UserData.UnmanagedConverter().Convert(new()
        {
            NameInfo = new Cores.UserNameInfo()
            {
                FamilyName = @"Hap",
                LastName = @"Po",
            },
            Age = 80,
            Balance = 100,
        }),
        new Cores.UserData.UnmanagedConverter().Convert(new()
        {
            NameInfo = new Cores.UserNameInfo()
            {
                FamilyName = @"Li",
                LastName = @"Li",
            },
            Age = 15,
            Balance = 0,
        }),
    };
    public struct UserDataMap0
    {
        public const int Count = 2;
        public static readonly Cores.UserKey.Unmanaged[] Keys = new Cores.UserKey.Unmanaged[]
        {
            new Cores.UserKey.UnmanagedConverter().Convert(new()
            {
                Value = 0,
            }),
            new Cores.UserKey.UnmanagedConverter().Convert(new()
            {
                Value = 1,
            }),
        };
        public static readonly Cores.UserData.Unmanaged[] Values = new Cores.UserData.Unmanaged[]
        {
            new Cores.UserData.UnmanagedConverter().Convert(new()
            {
                NameInfo = new Cores.UserNameInfo()
                {
                    FamilyName = @"Hap",
                    LastName = @"Po",
                },
                Age = 80,
                Balance = 100,
            }),
            new Cores.UserData.UnmanagedConverter().Convert(new()
            {
                NameInfo = new Cores.UserNameInfo()
                {
                    FamilyName = @"Li",
                    LastName = @"Li",
                },
                Age = 15,
                Balance = 0,
            }),
        };
        public static readonly t_Enumerable Enumerable;
        public struct t_Enumerable : IEnumerable<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>
        {
            public Enumerator GetEnumerator() => new();
            IEnumerator<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>> IEnumerable<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>.GetEnumerator() { throw new NotImplementedException(); }
            IEnumerator IEnumerable.GetEnumerator() { throw new NotImplementedException(); }
        }
        public struct Enumerator : IEnumerator<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>
        {
            private int index = -1;
            public KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged> Current => new(Keys[this.index], Values[this.index]);
            object IEnumerator.Current => Current;
            public Enumerator() { }
            public void Dispose() { }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() { index++; return index < Count; }
            public void Reset() => this.index = -1;
        }
        public static bool TryGetValue(Cores.UserKey.Unmanaged key, out Cores.UserData.Unmanaged value)
        {
            int keyHash = key.GetHashCode();
            switch (keyHash)
            {
                case 0:
                    value = Values[0];
                    return true;
                case 1:
                    value = Values[1];
                    return true;
            }
            value = default;
            return false;
        }
        public static Cores.UserData.Unmanaged GetValue(Cores.UserKey.Unmanaged key)
        {
            if (TryGetValue(key, out var value)) return value;
            throw new System.Collections.Generic.KeyNotFoundException($"Key {key} Not Found.");
        }
        public static bool ContainsKey(Cores.UserKey.Unmanaged key)
        {
            int keyHash = key.GetHashCode();
            return keyHash switch
            {
                0 => true,
                1 => true,
                _ => false,
            };
        }
        public static bool ContainsValue(Cores.UserData.Unmanaged value)
        {
            foreach (var entry in Values)
            {
                if (value.Equals(entry)) return true;
            }
            return false;
        }
    }
    public struct UserDataMap1
    {
        public const int Count = 2;
        public static readonly Cores.UserKey.Unmanaged[] Keys = new Cores.UserKey.Unmanaged[]
        {
            Unsafe.As<byte, Cores.UserKey.Unmanaged>(ref new byte[] { 15, 0, 0, 0 }[0]),
            Unsafe.As<byte, Cores.UserKey.Unmanaged>(ref new byte[] { 40, 0, 0, 0 }[0]),
        };
        public static readonly Cores.UserData.Unmanaged[] Values = new Cores.UserData.Unmanaged[]
        {
            new Cores.UserData.UnmanagedConverter().Convert(new()
            {
                NameInfo = new Cores.UserNameInfo()
                {
                    FamilyName = @"Hap",
                    LastName = @"Po",
                },
                Age = 80,
                Balance = 100,
            }),
            new Cores.UserData.UnmanagedConverter().Convert(new()
            {
                NameInfo = new Cores.UserNameInfo()
                {
                    FamilyName = @"Li",
                    LastName = @"Li",
                },
                Age = 15,
                Balance = 0,
            }),
        };
        public static readonly t_Enumerable Enumerable;
        public struct t_Enumerable : IEnumerable<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>
        {
            public Enumerator GetEnumerator() => new();
            IEnumerator<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>> IEnumerable<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>.GetEnumerator() { throw new NotImplementedException(); }
            IEnumerator IEnumerable.GetEnumerator() { throw new NotImplementedException(); }
        }
        public struct Enumerator : IEnumerator<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>
        {
            private int index = -1;
            public KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged> Current => new(Keys[this.index], Values[this.index]);
            object IEnumerator.Current => Current;
            public Enumerator() { }
            public void Dispose() { }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() { index++; return index < Count; }
            public void Reset() => this.index = -1;
        }
        public static bool TryGetValue(Cores.UserKey.Unmanaged key, out Cores.UserData.Unmanaged value)
        {
            int keyHash = key.GetHashCode();
            switch (keyHash)
            {
                case 15:
                    value = Values[0];
                    return true;
                case 40:
                    value = Values[1];
                    return true;
            }
            value = default;
            return false;
        }
        public static Cores.UserData.Unmanaged GetValue(Cores.UserKey.Unmanaged key)
        {
            if (TryGetValue(key, out var value)) return value;
            throw new System.Collections.Generic.KeyNotFoundException($"Key {key} Not Found.");
        }
        public static bool ContainsKey(Cores.UserKey.Unmanaged key)
        {
            int keyHash = key.GetHashCode();
            return keyHash switch
            {
                15 => true,
                40 => true,
                _ => false,
            };
        }
        public static bool ContainsValue(Cores.UserData.Unmanaged value)
        {
            foreach (var entry in Values)
            {
                if (value.Equals(entry)) return true;
            }
            return false;
        }
    }
    public struct UserDataMap2
    {
        public const int Count = 2;
        public static readonly Cores.UserKey.Unmanaged[] Keys = new Cores.UserKey.Unmanaged[]
        {
            new Cores.UserKey.UnmanagedConverter().Convert(new()
            {
                Value = 0,
            }),
            new Cores.UserKey.UnmanagedConverter().Convert(new()
            {
                Value = 1,
            }),
        };
        public static readonly Cores.UserData.Unmanaged[] Values = new Cores.UserData.Unmanaged[]
        {
            Unsafe.As<byte, Cores.UserData.Unmanaged>(ref new byte[] { 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 123, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 100, 0, 0, 0 }[0]),
            Unsafe.As<byte, Cores.UserData.Unmanaged>(ref new byte[] { 123, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 56, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0 }[0]),
        };
        public static readonly t_Enumerable Enumerable;
        public struct t_Enumerable : IEnumerable<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>
        {
            public Enumerator GetEnumerator() => new();
            IEnumerator<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>> IEnumerable<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>.GetEnumerator() { throw new NotImplementedException(); }
            IEnumerator IEnumerable.GetEnumerator() { throw new NotImplementedException(); }
        }
        public struct Enumerator : IEnumerator<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>
        {
            private int index = -1;
            public KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged> Current => new(Keys[this.index], Values[this.index]);
            object IEnumerator.Current => Current;
            public Enumerator() { }
            public void Dispose() { }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() { index++; return index < Count; }
            public void Reset() => this.index = -1;
        }
        public static bool TryGetValue(Cores.UserKey.Unmanaged key, out Cores.UserData.Unmanaged value)
        {
            int keyHash = key.GetHashCode();
            switch (keyHash)
            {
                case 0:
                    value = Values[0];
                    return true;
                case 1:
                    value = Values[1];
                    return true;
            }
            value = default;
            return false;
        }
        public static Cores.UserData.Unmanaged GetValue(Cores.UserKey.Unmanaged key)
        {
            if (TryGetValue(key, out var value)) return value;
            throw new System.Collections.Generic.KeyNotFoundException($"Key {key} Not Found.");
        }
        public static bool ContainsKey(Cores.UserKey.Unmanaged key)
        {
            int keyHash = key.GetHashCode();
            return keyHash switch
            {
                0 => true,
                1 => true,
                _ => false,
            };
        }
        public static bool ContainsValue(Cores.UserData.Unmanaged value)
        {
            foreach (var entry in Values)
            {
                if (value.Equals(entry)) return true;
            }
            return false;
        }
    }
    public struct UserDataMap3
    {
        public const int Count = 2;
        public static readonly Cores.UserKey.Unmanaged[] Keys = new Cores.UserKey.Unmanaged[]
        {
            Unsafe.As<byte, Cores.UserKey.Unmanaged>(ref new byte[] { 15, 0, 0, 0 }[0]),
            Unsafe.As<byte, Cores.UserKey.Unmanaged>(ref new byte[] { 40, 0, 0, 0 }[0]),
        };
        public static readonly Cores.UserData.Unmanaged[] Values = new Cores.UserData.Unmanaged[]
        {
            Unsafe.As<byte, Cores.UserData.Unmanaged>(ref new byte[] { 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 100, 0, 0, 0 }[0]),
            Unsafe.As<byte, Cores.UserData.Unmanaged>(ref new byte[] { 47, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 74, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0 }[0]),
        };
        public static readonly t_Enumerable Enumerable;
        public struct t_Enumerable : IEnumerable<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>
        {
            public Enumerator GetEnumerator() => new();
            IEnumerator<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>> IEnumerable<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>.GetEnumerator() { throw new NotImplementedException(); }
            IEnumerator IEnumerable.GetEnumerator() { throw new NotImplementedException(); }
        }
        public struct Enumerator : IEnumerator<KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged>>
        {
            private int index = -1;
            public KeyValuePair<Cores.UserKey.Unmanaged, Cores.UserData.Unmanaged> Current => new(Keys[this.index], Values[this.index]);
            object IEnumerator.Current => Current;
            public Enumerator() { }
            public void Dispose() { }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() { index++; return index < Count; }
            public void Reset() => this.index = -1;
        }
        public static bool TryGetValue(Cores.UserKey.Unmanaged key, out Cores.UserData.Unmanaged value)
        {
            int keyHash = key.GetHashCode();
            switch (keyHash)
            {
                case 15:
                    value = Values[0];
                    return true;
                case 40:
                    value = Values[1];
                    return true;
            }
            value = default;
            return false;
        }
        public static Cores.UserData.Unmanaged GetValue(Cores.UserKey.Unmanaged key)
        {
            if (TryGetValue(key, out var value)) return value;
            throw new System.Collections.Generic.KeyNotFoundException($"Key {key} Not Found.");
        }
        public static bool ContainsKey(Cores.UserKey.Unmanaged key)
        {
            int keyHash = key.GetHashCode();
            return keyHash switch
            {
                15 => true,
                40 => true,
                _ => false,
            };
        }
        public static bool ContainsValue(Cores.UserData.Unmanaged value)
        {
            foreach (var entry in Values)
            {
                if (value.Equals(entry)) return true;
            }
            return false;
        }
    }
}
