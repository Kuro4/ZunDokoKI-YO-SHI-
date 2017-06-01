using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Reactive.Linq;
using System.Threading;

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


    public class Model
    {
        static readonly private string kiyoshi = "キ・ヨ・シ！";
        static readonly private string tetteyterettettettetterey = "ﾃｯﾃｰﾃﾚｯﾃｯﾃｯﾃｯﾃﾚｰ";
        public ReactiveProperty<string> _Result { get; } = new ReactiveProperty<string>();

        public void ZundokoStart()
        {
            _Result.Value = "";
            foreach(var zundoko in StreamCreater.ZundokoStream())
            {
                _Result.Value += zundoko + Environment.NewLine;
            }
            _Result.Value += kiyoshi;

        }
        public void TetteyteretteyStart()
        {
            _Result.Value = "";
            foreach(var tetteyterettey in StreamCreater.TetteyteretteyStream())
            {
                _Result.Value += tetteyterettey + Environment.NewLine;
            }
            _Result.Value += tetteyterettettettetterey;
        }
    }

}
