using System;

namespace CodeTypeRanking
{
    public class UserData // struct のほうがいいかも？
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string BeginDate => _beginDate.ToString("yyyy/MM/dd");
        public System.Windows.Visibility IsHighlighted { get; set; } // boolean型にしたかったけどバインドに一工夫必要。時短。

        public UserData(string name, int score, DateTime date, bool isHighlighted = false)
        {
            Rank = -1;
            Name = name;
            Score = score;
            _beginDate = date;
            IsHighlighted = isHighlighted? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        DateTime _beginDate;
    }
}
