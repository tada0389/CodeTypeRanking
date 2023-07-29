using System;
using System.Windows.Input;
using System.Runtime.InteropServices;
using Reactive.Bindings;
using System.Timers;

namespace CodeTypeRanking
{
    /// <summary>
    /// スコア計算処理
    /// </summary>
    class ScoreCalcAlgorithm
    {
        public ScoreCalcAlgorithm(ReactiveProperty<int> score)
        {
            _score = score;
            _timer = new Timer(8.0);
            _timer.Elapsed += MyTimer_Tick;
            _timer.Start();
        }

        void MyTimer_Tick(object sender, EventArgs e)
        {
            //KeyをWindowsAPIで使う仮想キーコードに変換
            var addScore = 0;
            foreach (var keyObj in Enum.GetValues(typeof(Key)))
            {
                var key = (Key)keyObj;
                var vKey = KeyInterop.VirtualKeyFromKey((Key)key);
                var stateNext = GetKeyState(vKey);

                var statePrev = _keyStatePrev[(int)key];

                if (stateNext != statePrev)
                {
                    if (stateNext < 0)
                    {
                        // 押された
                        ++addScore;
                    }
                    _keyStatePrev[(int)key] = stateNext;
                }
            }

            _score.Value = _score.Value + addScore;
        }

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        static extern short GetKeyState(int vKey);

        short[] _keyStatePrev = new short[(int)Key.DeadCharProcessed + 1];
        ReactiveProperty<int> _score;
        Timer _timer;
    }
}
