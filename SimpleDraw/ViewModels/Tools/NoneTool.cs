﻿namespace SimpleDraw.ViewModels
{
    public class NoneTool : ToolBase
    {
        public override string Name => "None";

        public override void Pressed(CanvasViewModel canvas, double x, double y, ToolPointerType pointerType)
        {
        }

        public override void Released(CanvasViewModel canvas, double x, double y, ToolPointerType pointerType)
        {
        }

        public override void Moved(CanvasViewModel canvas, double x, double y, ToolPointerType pointerType)
        {
        }
    }
}
