// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.GraphicsMill;
using Aurigma.GraphicsMill.Codecs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Bitmap = Aurigma.GraphicsMill.Bitmap;
using CombineMode = Aurigma.GraphicsMill.Transforms.CombineMode;

namespace ODS.Service.RulesEngine.Tasks
{
    public class WatermarkTask : TaskBase
    {
        private readonly string _textKey;
        private readonly string _positionKey;

        public WatermarkTask(string textKey, string positionKey, ITask parent, IEnumerable<ITask> children) : base(parent, children)
        {
            _textKey = textKey;
            _positionKey = positionKey;
        }

        protected override string TaskName
        {
            get { return "WATERMARK"; }
        }

        public override int Priority
        {
            get { return 1; }
        }

        protected override TaskOutput Execute(TaskInput input, ITaskContext context)
        {
            foreach (var taskFile in input.Files)
            {
                var textNote = taskFile.Params[_textKey];
                var textPosition = taskFile.Params[_positionKey];

                using (var image = new Bitmap(taskFile.Path))
                {
                    if (!image.IsRgb || image.IsIndexed || image.IsGrayScale)
                        image.ColorManagement.ConvertToContinuous(ColorSpace.Rgb, false, false);

                    var renderingSize = new Size(image.Width, image.Height);

                    double materialWidth = (double)renderingSize.Width / 100;
                    double materialHeight = (double)renderingSize.Height / 100;

                    double lineSize = 0;
                    if (textPosition == "Left" || textPosition == "Right")
                    {
                        lineSize = materialWidth / 15;
                        materialWidth -= lineSize;
                    }
                    else
                    {
                        lineSize = materialHeight / 15;
                        materialHeight -= lineSize;
                    }

                    int textWidth = 0;
                    int textHeight = (int)Math.Round(lineSize * image.Height / materialHeight);

                    if (textPosition == "Left" || textPosition == "Right")
                        textWidth = image.Height;
                    else
                        textWidth = image.Width;

                    using (Bitmap text = new Bitmap(System.Drawing.Color.FromArgb(240, 240, 240), textWidth, textHeight, PixelFormat.Format32bppArgb))
                    {
                        using (Graphics graphics = text.GetGdiplusGraphics())
                        {
                            System.Drawing.Color sourceColor = System.Drawing.Color.White;
                            System.Drawing.Color destColor = System.Drawing.Color.White;

                            if (textPosition == "Left" || textPosition == "Top")
                                sourceColor = System.Drawing.Color.FromArgb(200, 200, 200);
                            else
                                destColor = System.Drawing.Color.FromArgb(200, 200, 200);

                            var gradient = new LinearGradientBrush(new Rectangle(0, 0, text.Width, text.Height), sourceColor, destColor, LinearGradientMode.Vertical);
                            graphics.FillRectangle(gradient, 0, 0, text.Width, text.Height);

                            using (var drawFont = new Aurigma.GraphicsMill.Drawing.Font("Trebuchet MS", textHeight / 2))
                            {
                                drawFont.Antialiased = true;
                                drawFont.Unit = Unit.Point;
                                drawFont.ClearType = true;

                                using (var drawBrush = new SolidBrush(System.Drawing.Color.Black))
                                {
                                    var drawFormat = new StringFormat();
                                    drawFormat.Alignment = StringAlignment.Center;
                                    drawFormat.LineAlignment = StringAlignment.Center;

                                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                    graphics.DrawString(textNote, drawFont, drawBrush, new RectangleF(0, 0, textWidth, textHeight), drawFormat);
                                }
                            }
                        }

                        int resultWidth = image.Width;
                        int resultHeight = image.Height;

                        if (textPosition == "Left" || textPosition == "Right")
                        {
                            text.Transforms.RotateAndFlip(RotateFlipType.Rotate270FlipNone);
                            resultWidth += text.Width;
                        }
                        else
                            resultHeight += text.Height;

                        using (var result = new Bitmap(resultWidth, resultHeight, PixelFormat.Format32bppArgb))
                        {
                            switch (textPosition)
                            {
                                case "Left":
                                    text.Draw(result, 0, 0, CombineMode.AlphaOverlapped, 1.0f);
                                    image.Draw(result, text.Width, 0, CombineMode.AlphaOverlapped, 1.0f);
                                    break;

                                case "Right":
                                    text.Draw(result, image.Width, 0, CombineMode.AlphaOverlapped, 1.0f);
                                    image.Draw(result, 0, 0, CombineMode.AlphaOverlapped, 1.0f);
                                    break;

                                case "Top":
                                    text.Draw(result, 0, 0, CombineMode.AlphaOverlapped, 1.0f);
                                    image.Draw(result, 0, text.Height, CombineMode.AlphaOverlapped, 1.0f);
                                    break;

                                default:
                                    text.Draw(result, 0, image.Height, CombineMode.AlphaOverlapped, 1.0f);
                                    image.Draw(result, 0, 0, CombineMode.AlphaOverlapped, 1.0f);
                                    break;
                            }

                            var pngEncoder = new PngEncoderOptions();
                            result.Save(taskFile.Path, pngEncoder);
                        }
                    }
                }
            }
            return new TaskOutput(input.Files, input.Params);
        }

        protected override void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput)
        {
        }
    }
}