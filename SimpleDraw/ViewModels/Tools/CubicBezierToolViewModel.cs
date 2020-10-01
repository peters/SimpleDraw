﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using ReactiveUI;

namespace SimpleDraw.ViewModels
{
    [DataContract(IsReference = true)]
    public class CubicBezierToolViewModel : ToolBaseViewModel
    {
        private enum State { None, Point3, Point2, Point1 }
        private State _state = State.None;
        private CubicBezierShapeViewModel _cubicBezier;
        private BrushViewModel _brush;
        private PenViewModel _pen;
        private double _hitRadius;
        private bool _tryToConnect;
        private bool _isStroked;
        private bool _isFilled;

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public BrushViewModel Brush
        {
            get => _brush;
            set => this.RaiseAndSetIfChanged(ref _brush, value);
        }

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public PenViewModel Pen
        {
            get => _pen;
            set => this.RaiseAndSetIfChanged(ref _pen, value);
        }

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public bool IsStroked
        {
            get => _isStroked;
            set => this.RaiseAndSetIfChanged(ref _isStroked, value);
        }

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public bool IsFilled
        {
            get => _isFilled;
            set => this.RaiseAndSetIfChanged(ref _isFilled, value);
        }

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public double HitRadius
        {
            get => _hitRadius;
            set => this.RaiseAndSetIfChanged(ref _hitRadius, value);
        }

        [DataMember(IsRequired = false, EmitDefaultValue = true)]
        public bool TryToConnect
        {
            get => _tryToConnect;
            set => this.RaiseAndSetIfChanged(ref _tryToConnect, value);
        }

        [IgnoreDataMember]
        public override string Name => "CubicBezier";

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

                            _cubicBezier = new CubicBezierShapeViewModel()
                            {
                                StartPoint = topLeft ?? new PointViewModel(x, y),
                                Point1 = new PointViewModel(x, y),
                                Point2 = new PointViewModel(x, y),
                                Point3 = new PointViewModel(x, y),
                                IsStroked = _isStroked,
                                IsFilled = _isFilled,
                                Brush = _brush.CloneSelf(shared),
                                Pen = _pen.CloneSelf(shared)
                            };
                            canvas.Decorators.Add(_cubicBezier);
                            canvas.Invalidate();
                            _state = State.Point3;
                        }
                    }
                    break;
                case State.Point3:
                    {
                        if (pointerType == ToolPointerType.Left)
                        {
                            var point3 = default(PointViewModel);

                            if (_tryToConnect)
                            {
                                var result = HitTest.Contains(canvas.Items, x, y, _hitRadius);
                                if (result is PointViewModel point)
                                {
                                    point3 = point;
                                }
                            }

                            if (point3 != null)
                            {
                                _cubicBezier.Point3 = point3;
                            }

                            canvas.Invalidate();

                            _state = State.Point2;
                        }

                        if (pointerType == ToolPointerType.Right)
                        {
                            canvas.Decorators.Remove(_cubicBezier);
                            canvas.Invalidate();
                            _cubicBezier = null;
                            _state = State.None;
                        }
                    }
                    break;
                case State.Point2:
                    {
                        if (pointerType == ToolPointerType.Left)
                        {
                            var point2 = default(PointViewModel);

                            if (_tryToConnect)
                            {
                                var result = HitTest.Contains(canvas.Items, x, y, _hitRadius);
                                if (result is PointViewModel point)
                                {
                                    point2 = point;
                                }
                            }

                            if (point2 != null)
                            {
                                _cubicBezier.Point2 = point2;
                            }

                            canvas.Invalidate();

                            _state = State.Point1;
                        }

                        if (pointerType == ToolPointerType.Right)
                        {
                            canvas.Decorators.Remove(_cubicBezier);
                            canvas.Invalidate();
                            _cubicBezier = null;
                            _state = State.None;
                        }
                    }
                    break;
                case State.Point1:
                    {
                        if (pointerType == ToolPointerType.Left)
                        {
                            var point1 = default(PointViewModel);

                            if (_tryToConnect)
                            {
                                var result = HitTest.Contains(canvas.Items, x, y, _hitRadius);
                                if (result is PointViewModel point)
                                {
                                    point1 = point;
                                }
                            }

                            if (point1 != null)
                            {
                                _cubicBezier.Point1 = point1;
                            }

                            canvas.Decorators.Remove(_cubicBezier);
                            canvas.Items.Add(_cubicBezier);
                            canvas.Invalidate();

                            _cubicBezier = null;
                            _state = State.None;
                        }

                        if (pointerType == ToolPointerType.Right)
                        {
                            canvas.Decorators.Remove(_cubicBezier);
                            canvas.Invalidate();
                            _cubicBezier = null;
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
                case State.Point3:
                    {
                    }
                    break;
                case State.Point1:
                    {
                    }
                    break;
                case State.Point2:
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
                case State.Point3:
                    {
                        if (pointerType == ToolPointerType.None)
                        {
                            _cubicBezier.Point2.X = x;
                            _cubicBezier.Point2.Y = y;
                            _cubicBezier.Point3.X = x;
                            _cubicBezier.Point3.Y = y;
                            canvas.Invalidate();
                        }
                    }
                    break;
                case State.Point2:
                    {
                        if (pointerType == ToolPointerType.None)
                        {
                            _cubicBezier.Point2.X = x;
                            _cubicBezier.Point2.Y = y;
                            _cubicBezier.Point1.X = x;
                            _cubicBezier.Point1.Y = y;
                            canvas.Invalidate();
                        }
                    }
                    break;
                case State.Point1:
                    {
                        if (pointerType == ToolPointerType.None)
                        {
                            _cubicBezier.Point1.X = x;
                            _cubicBezier.Point1.Y = y;
                            canvas.Invalidate();
                        }
                    }
                    break;
            }
        }

        public override ToolBaseViewModel CloneSelf(Dictionary<ViewModelBase, ViewModelBase> shared)
        {
            if (shared.TryGetValue(this, out var value))
            {
                return value as CubicBezierToolViewModel;
            }

            var copy = new CubicBezierToolViewModel()
            {
                Brush = _brush?.CloneSelf(shared),
                Pen = _pen?.CloneSelf(shared),
                IsStroked = _isStroked,
                IsFilled = _isFilled
            };

            shared[this] = copy;
            return copy;
        }

        public override ViewModelBase Clone(Dictionary<ViewModelBase, ViewModelBase> shared)
        {
            return CloneSelf(shared);
        }
    }
}