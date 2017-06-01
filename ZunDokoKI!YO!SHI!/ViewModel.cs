using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication1
{
    public class ViewModel
    {
        public Model _Model { get; } = new Model();
        public ReactiveCommand C_Start { get; } = new ReactiveCommand();
        public bool _isZundokoSelect { get; set; } = true;
        public bool _isTetteyteretteySelect { get; set; } = false;

        public ViewModel()
        {
            C_Start.Subscribe(Start);
        }

        private void Start()
        {
            if (_isZundokoSelect)
            {
                _Model.ZundokoStart();
            }
            else if (_isTetteyteretteySelect)
            {
                _Model.TetteyteretteyStart();
            }
        }
    }
}
