namespace Nice.Common.Tool.IdCreate
{
    /// <summary>
    /// id生成器
    /// </summary>
    public static class IdCreater
    {
        /// <summary>
        /// 生成一个18位根据时间的id
        /// </summary>
        /// <returns></returns>
        public static string GetBanksId() => BanksId(DateTimeOffset.UtcNow.ToLocalTime(), new Random(Guid.NewGuid().GetHashCode()));
        
        
        static Func<DateTimeOffset, Random, string> BanksId
            = (date, random)
                 => 
                   $"{date.Year}{(random.Next(200000, 888888) + 1).ToString().PadLeft(6, '6')}" +
                   $"{date.Second.ToString().PadLeft(2, '0')}" +
                    $"{(random.Next(1000, 2000) + 1).ToString().PadLeft(4, '0')}" +
                    $"{(random.Next(66, 88) + 1).ToString().PadLeft(2, '2')}";
    }
}
