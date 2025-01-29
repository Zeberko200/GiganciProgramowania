namespace Shared;

public static class EnumHelpers
{
    public static IEnumerable<uint> ToArray<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<uint>();
    }
}