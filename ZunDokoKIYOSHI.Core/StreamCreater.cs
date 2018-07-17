using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZunDokoKIYOSHI.Core
{
    public static class StreamCreater
    {
        static readonly private Queue<string> queue = new Queue<string>();
        static private Random random = new Random();
        /// <summary>
        /// 入力したパターンからランダムに出力し、完成パターンと一致すれば出力を終了する
        /// </summary>
        /// <param name="inputPattern">入力パターン</param>
        /// <param name="finishedPattern">完成パターン</param>
        /// <returns></returns>
        public static IEnumerable<string> Create(string[] inputPattern, string[] finishedPattern)
        {
            queue.Clear();
            while (!queue.SequenceEqual(finishedPattern))
            {
                queue.Enqueue(inputPattern[random.Next(inputPattern.Count())]);
                if (queue.Count == finishedPattern.Count() + 1)
                {
                    queue.Dequeue();
                }
                yield return queue.Last();
            }
        }
    }
}
