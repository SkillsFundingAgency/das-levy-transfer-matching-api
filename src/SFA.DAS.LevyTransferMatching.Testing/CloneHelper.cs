using Newtonsoft.Json;

namespace SFA.DAS.LevyTransferMatching.Testing;

public static class CloneHelper
{
    public static T Clone<T>(T source)
    {
        var serialized = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<T>(serialized);
    }

    public static U Clone<T, U>(T source)
    {
        var serialized = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<U>(serialized);
    }
}