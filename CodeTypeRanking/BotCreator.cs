using System;
using System.Collections.Generic;

namespace CodeTypeRanking
{
    class BotCreator
    {
        public BotCreator()
        {
        }

        public List<BotData> CreateBots()
        {
            var bots = new List<BotData>();

            var botNames = new List<string>(100)
            {
                //"Koitan(偽物)"
                "Buck"
                , "Madison"
                , "Jen"
                , "Felicia"
                , "Randall"
                , "Stewart"
                , "Benedetta"
                , "Cass"
                , "Deven"
                , "Cordell"
                , "Lodovick"
                , "Cataleya"
                , "Donna"
                , "Candace"
                , "Sigismund"
                , "Kendell"
                , "Kaylee"
                , "Tiera"
                , "Donnie"
                , "Eva"
                , "Edmund"
                , "Hoyt"
                , "Buck"
                , "Maira"
                , "Franscisca"
                , "Millard"
                , "Galen"
                , "Lola"
                , "Humphrey"
                , "Suzie"
                , "Mona"
                , "Mae"
                , "Montgomery"
                , "Leola"
                , "Ruth"
                , "Selenia"
                , "Damon"
                , "Edmond"
                , "Simon"
                , "Luanne"
                , "Kaela"
                , "Andrea"
                , "Beatrix"
                , "Luke"
                , "Cullen"
                , "Tommie"
                , "Harley"
                , "Shelia"
                , "Krystin"
                , "Rusty"
                , "Nan"
                , "Tyrone"
                , "Kevin"
                , "Cody"
                , "Gary"
                , "Wallace"
                , "Carlisle"
                , "Travon"
                , "Clifford"
                , "Page"
                , "Adriana"
                , "Darion"
                , "Nix"
                , "Catharine"
                , "Waverly"
                , "Drake"
                , "Jeff"
                , "Jeremy"
                , "Weldon"
                , "Gillian"
                , "Letty"
                , "Kadence"
                , "Lynne"
                , "Henrietta"
                , "Ray"
                , "Cyrus"
                , "Sharron"
                , "Arlo"
                , "Evie"
                , "Beth"
                , "Harmon"
                , "Georgie"
                , "Jerrie"
                , "Georgina"
                , "Rachelle"
                , "Eric"
                , "Brandi"
                , "Hall"
                , "Jarvis"
                , "Tameka"
                , "Ulysses"
                , "Baxter"
                , "Edith"
                , "Lenora"
                , "India"
                , "Ingram"
                , "Michelle"
                , "Stefanie"
                , "Crystal"
                , "Clinton"
            };

            // 適当に作る
            int botCount = 100;
            for (int idx = 0; idx < botCount; ++idx)
            {
                bots.Add(new BotData()
                {
                    Name = botNames[idx],
                    ScorePerMin = Math.Pow(botCount - idx + 1, 1.4f) / 200.0 * 1.3
                });
            }

            // 一人目を飛びぬけて強くする
            var strongestUser = bots[0];
            strongestUser.ScorePerMin += 1.5f;
            bots[0] = strongestUser;

            return bots;
        }
    }
}
