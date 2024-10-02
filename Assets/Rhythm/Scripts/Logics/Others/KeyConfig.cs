namespace Rhythm
{
    public enum KeyConfig
    {
        Type1,
        Type2,
    }

    public static class KeyConfigExtension
    {
        public static string ToStringQuickly(this KeyConfig config)
        {
            return config switch
            {
                KeyConfig.Type1 => "Type1",
                KeyConfig.Type2 => "Type2",
                _ => throw new System.InvalidOperationException()
            };
        }
    }
}