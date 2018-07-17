using Prism.Interactivity.InteractionRequest;
using Prism.InteractivityExtension;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZunDokoKIYOSHI.Core;

namespace ZunDokoKIYOSHI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public ReactiveProperty<string> LeftInputPattern { get; private set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> LeftFinishedPattern { get; private set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> LeftOutput { get; private set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<bool> IsLeftChecked { get; private set; } = new ReactiveProperty<bool>(true);

        public ReactiveProperty<string> RightInputPattern { get; private set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> RightFinishedPattern { get; private set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> RightOutput { get; private set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<bool> IsRightChecked { get; private set; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<bool> IsStarted { get; private set; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<int> StreamCount { get; private set; } = new ReactiveProperty<int>();
        public ReactiveProperty<string> ResultText { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> StartStopButtonText { get; private set; } = new ReactiveProperty<string>("Start");

        public InteractionRequest<Notification> Notification { get; } = new InteractionRequest<Notification>();

        public ReactiveCommand StreamStartCommand { get; private set; } = new ReactiveCommand();

        private CancellationTokenSource cancellationTokenSource;

        public ShellViewModel()
        {
            StreamStartCommand.Subscribe(async () =>
            {
                if (!this.IsStarted.Value)
                {
                    if (this.IsLeftChecked.Value) await this.StreamStart(this.LeftInputPattern.Value, this.LeftFinishedPattern.Value, this.LeftOutput.Value);
                    else if (this.IsRightChecked.Value) await this.StreamStart(this.RightInputPattern.Value, this.RightFinishedPattern.Value, this.RightOutput.Value);
                }
                else this.StreamStop();
            });
        }

        /// <summary>
        /// Streamを開始する
        /// </summary>
        /// <param name="input"></param>
        /// <param name="finished"></param>
        /// <param name="output"></param>
        private async Task StreamStart(string input, string finished, string output)
        {
            if (this.IsStarted.Value) return;
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(finished)) return;

            var inputPattern = input.Split(',');
            var finishedPattern = finished.Split(',');
            //パターンが一致可能か確認
            if (finishedPattern.Except(inputPattern).Any())
            {
                this.Notification.RaiseEx("エラー", "完成パターン内に、入力パターンに存在しないものがあります。");
                return;
            }
            //初期化
            this.StartStopButtonText.Value = "Stop";
            this.IsStarted.Value = true;
            this.StreamCount.Value = 0;
            this.ResultText.Value = "";
            this.cancellationTokenSource = new CancellationTokenSource();

            //Streamを非同期で生成
            var res = await Task.Run(async () =>
            {
                foreach (var item in StreamCreater.Create(inputPattern, finishedPattern))
                {
                    if (cancellationTokenSource.IsCancellationRequested) return false;
                    this.ResultText.Value += item + ',';
                    this.StreamCount.Value++;
                    await Task.Delay(1);
                }
                this.ResultText.Value += output;
                this.IsStarted.Value = false;
                return true;
            }, this.cancellationTokenSource.Token);
            //キャンセルされなければダイアログ表示
            if (res)
            {
                this.Notification.RaiseEx("パターン一致", $"{finished},{output}\r\n{this.StreamCount.Value}回で完成パターンと一致しました。");
                this.StartStopButtonText.Value = "Start";
            }
        }
        /// <summary>
        /// 動作中のStreamを停止する
        /// </summary>
        private void StreamStop()
        {
            if (!this.IsStarted.Value) return;
            this.StartStopButtonText.Value = "Start";
            this.IsStarted.Value = false;
            this.cancellationTokenSource.Cancel();
        }
    }
}
