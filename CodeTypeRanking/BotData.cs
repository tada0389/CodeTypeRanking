using System;

namespace CodeTypeRanking
{
    struct BotData
    {
        public  UserData ToUserData()
        {
            var beginDate = DateTime.Parse("2023/07/28 9:00:00");
            var now = DateTime.Now;
            var diffMin = (now - beginDate).TotalMinutes;
            var score = (int)(ScorePerMin * diffMin);

            return new UserData(
                Name,
                score,
                beginDate
                );
        }

        public string Name;
        public double ScorePerMin;
    }
}
