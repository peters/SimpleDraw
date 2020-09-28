﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace SimpleDraw.ViewModels
{
    public class DashStyleViewModel : ViewModelBase
    {
        private ObservableCollection<double> _dashes;
        private double _offset;

        public ObservableCollection<double> Dashes
        {
            get => _dashes;
            set => this.RaiseAndSetIfChanged(ref _dashes, value);
        }

        public double Offset
        {
            get => _offset;
            set => this.RaiseAndSetIfChanged(ref _offset, value);
        }

        public DashStyleViewModel()
        {
        }

        public DashStyleViewModel(ObservableCollection<double> dashes, double offset)
        {
            _dashes = dashes;
            _offset = offset;
        }

        public DashStyleViewModel Copy(Dictionary<ViewModelBase, ViewModelBase> shared)
        {
            if (shared.TryGetValue(this, out var value))
            {
                return value as DashStyleViewModel;
            }

            var dashes = new ObservableCollection<double>();

            foreach (var dash in _dashes)
            {
                dashes.Add(dash);
            }

            var copy = new DashStyleViewModel()
            {
                Dashes = dashes,
                Offset = _offset
            };

            shared[this] = copy;
            return copy;
        }

        public override ViewModelBase Clone(Dictionary<ViewModelBase, ViewModelBase> shared)
        {
            return Copy(shared);
        }
    }
}
