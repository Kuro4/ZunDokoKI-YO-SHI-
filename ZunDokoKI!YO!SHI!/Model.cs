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
        static readonly private Queue<string> queue = new Queue<string>();
        static private Random random = new Random();

        public static IEnumerable<string> StreamCreate(string[] target,string[] pattern)
        {
            queue.Clear();
            while (!queue.SequenceEqual(pattern))
            {
                queue.Enqueue(target[random.Next(target.Count())]);
                if (queue.Count == pattern.Count() + 1)
                {
                    queue.Dequeue();
                }
                yield return queue.Last();
            }
        }
    }

    public class Model
    {
        static readonly private string[] zundoko = { "ズン", "ドコ", };
        static readonly private string[] zundokoPattern = { "ズン","ズン","ズン","ズン","ドコ" };
        static readonly private string kiyoshi = "キ・ヨ・シ！";
        static readonly private string[] tetteyterettey = { "ﾃｯﾃｰﾃﾚｯﾃｰ", "(ﾃｯﾃｰﾃﾚｯﾃｰ)", "ﾃ　ｯ　ﾃ　ｰ　ﾃ　ﾚ　ｯ　ﾃ　ｰ", "ﾊﾟｰﾊﾟﾗｯﾊﾟｰ", "ﾗｰﾗﾗｰﾗｰ" };
        static readonly private string[] tetteyteretteyPattern = { "ﾃｯﾃｰﾃﾚｯﾃｰ","ﾃｯﾃｰﾃﾚｯﾃｰ","ﾃｯﾃｰﾃﾚｯﾃｰ" };
        static readonly private string tetteyterettettettetterey = "ﾃｯﾃｰﾃﾚｯﾃｯﾃｯﾃｯﾃﾚｰ";
        public ReactiveProperty<string> _Result { get; } = new ReactiveProperty<string>();

        public void ZundokoStart()
        {
            StreamStart(zundoko, zundokoPattern, kiyoshi);
        }

        public void TetteyteretteyStart()
        {
            StreamStart(tetteyterettey, tetteyteretteyPattern, tetteyterettettettetterey);
        }

        private void StreamStart(string[] target,string[] pattern,string lastAddString)
        {
            _Result.Value = "";
            foreach (var item in StreamCreater.StreamCreate(target, pattern))
            {
                _Result.Value += item + Environment.NewLine;
            }
            _Result.Value += lastAddString;
        }
    }

}
