using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Reactive.Bindings;
using System.Windows.Threading;

namespace CodeTypeRanking.ViewModels
{
    class ViewModel
    {
        public ReactiveProperty<string> TitleName { get; }

        public ReactiveProperty<int> MyScore { get; }
        public ReactiveProperty<string> MyScoreDiff { get; }
        public ReactiveProperty<string> MyName { get; }

        public ViewModel(MainWindow parent)
        {
            _parent = parent;
            _parent.lvw.FontSize = 20.0;

            TitleName = new ReactiveProperty<string>("実装量ランキング");

            _rankingLoader = new RankingLoader();

            MyScore = new ReactiveProperty<int>(_startScore);
            MyScoreDiff = new ReactiveProperty<string>("+0");
            MyName = new ReactiveProperty<string>("no_name");

            _scoreCalcAlgorithm = new ScoreCalcAlgorithm(MyScore);

            MyScore.Subscribe(_ =>
            {
                MyScoreDiff.Value = $"+{MyScore.Value - _startScore}";

                var titleName = MyScore.Value == 0 ? "実装量ランキング"
                    : $"{MyScore.Value} types - 実装量ランキング";
                TitleName.Value = titleName;

                if (_myData != null)
                {
                    _myData.Score = MyScore.Value;
                }
            });

            MyName.Subscribe(_ =>
            {
                if (_myData != null)
                {
                    _myData.Name = MyName.Value;
                }
            });

            _ = InitAsync();
        }

        public async Task<int> InitAsync()
        {
            // 自身のデータを取得
            _myData = await _rankingLoader.GetMyData();
            _startScore = _myData.Score;
            MyScore.Value = _startScore;
            MyName.Value = _myData.Name;

            // ランキングデータ取得
            _users = await _rankingLoader.Load(_myData);
            _parent.lvw.ItemsSource = _users;

            // タイマーで一定間隔ごとに実行
            _timer = new DispatcherTimer(DispatcherPriority.Normal);
            _timer.Interval = new TimeSpan(0, 0, 5); // 10秒毎に更新
            _timer.Tick += (async (s, e) =>
            {
                // 自身のデータを登録
                await _rankingLoader.SetMyData(_myData);

                // ランキングデータを更新する
                _users = await _rankingLoader.Load(_myData);

                // ランキング表更新
                _parent.lvw.ItemsSource = _users;
            });
            _timer.Start();

            return 0;
        }

        MainWindow _parent;
        ScoreCalcAlgorithm _scoreCalcAlgorithm;
        RankingLoader _rankingLoader;
        ObservableCollection<UserData> _users;
        DispatcherTimer _timer;
        UserData _myData;
        int _startScore = 0;
        int _curRank = 0;
    }
}
