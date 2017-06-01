using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Reactive.Linq;

namespace WpfApplication1
{

    public static class StreamCreater
    {
        static readonly private string[] zundoko = { "ズン", "ドコ", };
        static readonly private string zundokoPattern = "ズンズンズンズンドコ";
        static readonly private Queue<string> zundokoQueue = new Queue<string>();

        static readonly private string[] tetteyterettey = { "ﾃｯﾃｰﾃﾚｯﾃｰ", "(ﾃｯﾃｰﾃﾚｯﾃｰ)", "ﾃ　ｯ　ﾃ　ｰ　ﾃ　ﾚ　ｯ　ﾃ　ｰ", "ﾊﾟｰﾊﾟﾗｯﾊﾟｰ", "ﾗｰﾗﾗｰﾗｰ" };
        static readonly private string tetteyteretteyPattern = "ﾃｯﾃｰﾃﾚｯﾃｰﾃｯﾃｰﾃﾚｯﾃｰﾃｯﾃｰﾃﾚｯﾃｰ";
        static readonly private Queue<string> tetteyteretteyQueue = new Queue<string>();

        static private Random random = new Random();

        public static IEnumerable<string> ZundokoStream()
        {
            zundokoQueue.Clear();
            while (string.Join("", zundokoQueue) != zundokoPattern)
            {
                zundokoQueue.Enqueue(zundoko[random.Next(zundoko.Count())]);
                if (zundokoQueue.Count == 6)
                {
                    zundokoQueue.Dequeue();
                }
                yield return zundokoQueue.Last();
            }
        }

        public static IEnumerable<string> TetteyteretteyStream()
        {
            tetteyteretteyQueue.Clear();
            while (string.Join("", tetteyteretteyQueue) != tetteyteretteyPattern)
            {
                tetteyteretteyQueue.Enqueue(tetteyterettey[random.Next(tetteyterettey.Count())]);
                if (tetteyteretteyQueue.Count == 4)
                {
                    tetteyteretteyQueue.Dequeue();
                }
                yield return tetteyteretteyQueue.Last();
            }
        }
    }


    public class ZunDokoModel
    {
        static readonly string kiyoshi = "キ・ヨ・シ！";
        static readonly string tetteyterettettettetterey = "ﾃｯﾃｰﾃﾚｯﾃｯﾃｯﾃｯﾃﾚｰ";
        public ReactiveProperty<string> _Zundoko { get; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> _Tetteyterettey { get; } = new ReactiveProperty<string>("");
        public ReactiveCommand C_ZundokoStart { get; } = new ReactiveCommand();
        public ReactiveCommand C__TetteyteretteyStart { get; } = new ReactiveCommand();

        private void ZundokoStart()
        {               
            foreach(var item in StreamCreater.ZundokoStream())
            {
                _Zundoko.Value += item + Environment.NewLine;
            }
        }

        public ZunDokoModel()
        {
            var str = StreamCreater.TetteyteretteyStream().ToList<string>();
            str.Add(tetteyterettettettetterey);
            foreach (var item in str)
            {
                Console.WriteLine(item);
            }
            C_ZundokoStart.Subscribe(ZundokoStart);
        }
    }

}
