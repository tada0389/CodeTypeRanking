using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NCMBClient;

namespace CodeTypeRanking
{
    class RankingLoader
    {
        const string ClassName = "Users"; // "users" は予約語なので気を付ける
        const string ObjectIdKey = "objectId";
        const string NameKey = "name";
        const string ScoreKey = "score";
        const string BeginDateKey = "date";

        string ObjectId
        {
            get
            {
                if (_objectId is null)
                {
                    _objectId = SaveDataUtil.LoadObjectId();
                    Debug.Print(_objectId);
                }

                return _objectId;
            }
            set
            {
                if (_objectId == value)
                {
                    return;
                }

                _objectId = value;
                SaveDataUtil.SaveObjectId(_objectId);
            }
        }
        string _objectId = null;

        public RankingLoader()
        {
            // データベース初期化
            new NCMB("test", "test"); // key は共有したくないので隠す

            _botCreator = new BotCreator();
        }

        public async Task<UserData> GetMyData()
        {
            var query = new NCMBQuery(ClassName);
            //query.EqualTo(ObjectIdKey, ObjectId); // なぜかヒットしない・
            //if (query.GetCount() > 0)
            //{
            //    // 登録されているのでデータベースのを使用
            //    _myUser = await query.Fetch();
            //    ObjectId = _myUser.ObjectId();

            //    return NCMBObjectToUser(_myUser);
            //}

            // 全て読み込んじゃう
            var users = await query.FetchAll();
            var myObjCandidates = users.Where(x => x.ObjectId() == ObjectId);
            if (myObjCandidates.Any())
            {
                _myUser = myObjCandidates.First();
                var ret = NCMBObjectToUser(_myUser);
                ret.IsHighlighted = System.Windows.Visibility.Visible;
                return ret;
            }

            // 未登録なので初期値
            return new UserData("no_name", 0, DateTime.Now, isHighlighted: true);
        }

        // async void はヤバいらしい
        public async Task<int> SetMyData(UserData data)
        {
            if (_myUser is null)
            {
                _myUser = new NCMBObject(ClassName);
                _myUser.ObjectId(ObjectId);
            }

            _myUser.Set(NameKey, data.Name)
                .Set(ScoreKey, data.Score)
                .Set(BeginDateKey, data.BeginDate);

            // データベースに保存
            await _myUser.Save();

            ObjectId = _myUser.ObjectId();

            return 0;
        }

        public async Task<ObservableCollection<UserData>> Load(UserData myData)
        {
            var users = new ObservableCollection<UserData>();

            if (_myUser is null)
            {
                // まだ登録されていない場合はここで追加
                users.Add(myData);
            }

            // データベースからデータを取得する
            {
                var query = new NCMBQuery(ClassName);
                var serverUsers = await query.FetchAll();
                foreach (var user in serverUsers)
                {
                    if (user.ObjectId() == ObjectId)
                    {
                        // 自身のデータの場合はデータを最新のものを使う
                        users.Add(myData);
                    }
                    else
                    {
                        users.Add(NCMBObjectToUser(user));
                    }
                }
            }

            // bot を追加する
            var bots = _botCreator.CreateBots();

            foreach (var bot in bots)
            {
                users.Add(bot.ToUserData());
            }


            // スコア高い順にソート
            users = new ObservableCollection<UserData>(users.OrderByDescending(x => x.Score));

            // ランク割り当て
            for (int idx = 0, count = users.Count; idx < count; ++idx)
            {
                users[idx].Rank = idx + 1;
            }

            return users;
        }

        UserData NCMBObjectToUser(NCMBObject obj)
        {
            return new UserData(
                name: obj.GetString(NameKey),
                score: obj.GetInt(ScoreKey),
                date: DateTime.Parse(obj.GetString(BeginDateKey))
                );
        }

        NCMBObject _myUser = null;
        BotCreator _botCreator;
    }
}
