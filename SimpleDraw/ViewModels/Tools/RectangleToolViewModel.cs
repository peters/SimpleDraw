﻿using System.Collections.Generic;
using ReactiveUI;

namespace SimpleDraw.ViewModels
{
    public class RectangleToolViewModel : ToolBaseViewModel
    {
        private enum State { None, Pressed }
        private State _state = State.None;
        private RectangleShapeViewModel _rectangle;
        private BrushViewModel _brush;
        private PenViewModel _pen;
        private double _hitRadius;
        private bool _tryToConnect;
        private bool _isStroked;
        private bool _isFilled;
        private double _radiusX;
        private double _radiusY;

        public BrushViewModel Brush
        {
            get => _brush;
            set => this.RaiseAndSetIfChanged(ref _brush, value);
        }

        public PenViewModel Pen
        {
            get => _pen;
            set => this.RaiseAndSetIfChanged(ref _pen, value);
        }

        public bool IsStroked
        {
            get => _isStroked;
            set => this.RaiseAndSetIfChanged(ref _isStroked, value);
        }

        public bool IsFilled
        {
            get => _isFilled;
            set => this.RaiseAndSetIfChanged(ref _isFilled, value);
        }

        public double RadiusX
        {
            get => _radiusX;
            set => this.RaiseAndSetIfChanged(ref _radiusX, value);
        }

        public double RadiusY
        {
            get => _radiusY;
            set => this.RaiseAndSetIfChanged(ref _radiusY, value);
        }

        public double HitRadius
        {
            get => _hitRadius;
            set => this.RaiseAndSetIfChanged(ref _hitRadius, value);
        }

        public bool TryToConnect
        {
            get => _tryToConnect;
            set => this.RaiseAndSetIfChanged(ref _tryToConnect, value);
        }

        public override string Name => "Rectangle";

        public override void Pressed(CanvasViewModel canvas, double x, double y, ToolPointerType pointerType, ToolKeyModifiers keyModifiers)
        {
            switch (_state)
            {
                case State.None:
                    {
                        if (pointerType == ToolPointerType.Left)
                        {
                            var shared = new Dictionary<ViewModelBase, ViewModelBase>();
                            var topLeft = default(PointViewModel);

                            if (_tryToConnect)
                            {
                                var result = HitTest.Contains(canvas.Items, x, y, _hitRadius);
                                if (result is PointViewModel point)
                                {
                                    topLeft = point;
                                }
                            }

                            _rectangle = new RectangleShapeViewModel()
                            {
                                TopLeft = topLeft ?? new PointViewModel(x, y),
                                BottomRight = new PointViewModel(x, y),
                                IsStroked = _isStroked,
                                IsFilled = _isFilled,
                                RadiusX = _radiusX,
                                RadiusY = _radiusY,
                                Brush = _brush.Copy(shared),
                                Pen = _pen.Copy(shared)
                            };
                            canvas.Decorators.Add(_rectangle);
                            _state = State.Pressed;
                        }
                    }
                    break;
                case State.Pressed:
                    {
                        if (pointerType == ToolPointerType.Left)
                        {
                            var bottomRight = default(PointViewModel);

                            if (_tryToConnect)
                            {
                                var result = HitTest.Contains(canvas.Items, x, y, _hitRadius);
                                if (result is PointViewModel point)
                                {
                                    bottomRight = point;
                                }
                            }

                            if (bottomRight != null)
                            {
                                _rectangle.BottomRight = bottomRight;
                            }

                            canvas.Decorators.Remove(_rectangle);
                            canvas.Items.Add(_rectangle);

                            _rectangle = null;
                            _state = State.None;
                        }

                        if (pointerType == ToolPointerType.Right)
                        {
                            canvas.Decorators.Remove(_rectangle);
                            _rectangle = null;
                            _state = State.None;
                        }
                    }
                    break;
            }
        }

        public override void Released(CanvasViewModel canvas, double x, double y, ToolPointerType pointerType, ToolKeyModifiers keyModifiers)
        {
            switch (_state)
            {
                case State.None:
                    {
                    }
                    break;
                case State.Pressed:
                    {
                    }
                    break;
            }
        }

        public override void Moved(CanvasViewModel canvas, double x, double y, ToolPointerType pointerType, ToolKeyModifiers keyModifiers)
        {
            switch (_state)
            {
                case State.None:
                    {
                    }
                    break;
                case State.Pressed:
                    {
                        if (pointerType == ToolPointerType.None)
                        {
                            _rectangle.BottomRight.X = x;
                            _rectangle.BottomRight.Y = y;
                        }
                    }
                    break;
            }
        }

        public override ToolBaseViewModel Copy(Dictionary<ViewModelBase, ViewModelBase> shared)
        {
            if (shared.TryGetValue(this, out var value))
            {
                return value as RectangleToolViewModel;
            }

            var copy = new RectangleToolViewModel()
            {
                Brush = _brush?.Copy(shared),
                Pen = _pen?.Copy(shared),
                IsStroked = _isStroked,
                IsFilled = _isFilled,
                RadiusX = _radiusX,
                RadiusY = _radiusY
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
