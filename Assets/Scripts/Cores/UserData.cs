using SOConstantsGenerator;
using System;
using Unity.Collections;

namespace Cores;

// Example
// - Has nested types
// - Has Managed and Unmanaged variants + converter
public static class UserData
{
    // The Managed part for Editor only
    [Serializable]
    public class Managed
    {
        public UserNameInfo NameInfo;
        public int Age;
        public int Balance;
    }

    // The Unmanaged part for runtime only
    [Serializable]
    public struct Unmanaged
    {
        public UserNameInfoUnmanaged NameInfo;
        public int Age;
        public int Balance;

        public FixedString64Bytes ToFixedString()
        {
            return $"{Age}, {Balance}";
        }
    }

    [Serializable]
    public struct UnmanagedTest
    {
        public int Balance;

        public FixedString64Bytes ToFixedString()
        {
            return $"{Balance}";
        }
    }

    // The UnmanagedConverter to convert Managed to Unmanaged
    public struct UnmanagedConverter : ITypeConverter<Managed, Unmanaged>
    {
        public Unmanaged Convert(Managed source)
        {
            return new()
            {
                NameInfo = new UserNameInfoUnmanagedConverter().Convert(source.NameInfo),
                Age = source.Age,
                Balance = source.Balance,
            };
        }
    }
}

[Serializable]
public class UserNameInfo
{
    public string FamilyName;
    public string LastName;
}

[Serializable]
public struct UserNameInfoUnmanaged
{
    public FixedString64Bytes FamilyName;
    public FixedString64Bytes LastName;
}

public class UserNameInfoUnmanagedConverter : ITypeConverter<UserNameInfo, UserNameInfoUnmanaged>
{
    public UserNameInfoUnmanaged Convert(UserNameInfo source)
    {
        return new()
        {
            FamilyName = source.FamilyName,
            LastName = source.LastName,
        };
    }
}

public static class UserKey
{
    [Serializable]
    public class Managed : IEquatable<Managed>
    {
        public int Value;

        public bool Equals(Managed other) => Value == other.Value;

        public override bool Equals(object obj) => Equals((Managed)obj);

        public override int GetHashCode() => Value.GetHashCode();
    }

    [Serializable]
    public struct Unmanaged : IEquatable<Unmanaged>
    {
        public int Value;

        public bool Equals(Unmanaged other) => Value == other.Value;

        public override bool Equals(object obj) => Equals((Unmanaged)obj);

        public override int GetHashCode() => Value.GetHashCode();

        public FixedString64Bytes ToFixedString()
        {
            return $"{Value}";
        }
    }

    public class UnmanagedConverter : ITypeConverter<Managed, Unmanaged>
    {
        public Unmanaged Convert(Managed source)
        {
            return new()
            {
                Value = source.Value,
            };
        }
    }
}