// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.GraphicsMill;
using Aurigma.GraphicsMill.Codecs;
using Aurigma.GraphicsMill.Transforms;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using Bitmap = Aurigma.GraphicsMill.Bitmap;

namespace ODS.Service.RulesEngine.Tasks
{
    public class ConvertTask : TaskBase
    {
        private readonly ImageFormat _format;
        private readonly ConvertFormat _convertFormat;

        public enum ConvertFormat
        {
            Jpeg,
            Tiff,
            Png
        }

        public ConvertTask(ConvertFormat format, ITask parent, IEnumerable<ITask> children) : base(parent, children)
        {
            _convertFormat = format;
            switch (format)
            {
                case ConvertFormat.Jpeg:
                    _format = ImageFormat.Jpeg;
                    break;

                case ConvertFormat.Tiff:
                    _format = ImageFormat.Tiff;
                    break;

                case ConvertFormat.Png:
                    _format = ImageFormat.Png;
                    break;
            }
        }

        protected override string TaskName
        {
            get { return string.Format("CONVERT"); }
        }

        public override int Priority
        {
            get { return 5; }
        }

        protected override TaskOutput Execute(TaskInput input, ITaskContext context)
        {
            foreach (var file in input.Files)
            {
                try
                {
                    IEncoderOptions options = new JpegEncoderOptions(90, false);
                    switch (_convertFormat)
                    {
                        case ConvertFormat.Jpeg:
                            options = new JpegEncoderOptions(90, false);
                            break;

                        case ConvertFormat.Png:
                            options = new PngEncoderOptions();
                            break;

                        case ConvertFormat.Tiff:
                            options = new TiffEncoderOptions(CompressionType.Zip);
                            break;
                    }

                    var isConverted = false;
                    using (var readStream = new FileStream(file.Path, FileMode.Open))
                    {
                        using (var reader = FormatManager.CreateFormatReader(readStream))
                        {
                            if (reader.MediaFormat != options.MediaFormat)
                            {
                                using (var frame = reader.LoadFrame(0))
                                {
                                    using (var resultBitmap = new Bitmap(frame.Width, frame.Height, Aurigma.GraphicsMill.PixelFormat.Format24bppRgb))
                                    {
                                        frame.GetBitmap(resultBitmap);

                                        var conveter = new PixelFormatConverter { DestinationPixelFormat = Aurigma.GraphicsMill.PixelFormat.Format32bppArgb };
                                        conveter.ApplyTransform(resultBitmap);

                                        if (resultBitmap.HasAlpha)
                                            resultBitmap.Channels.DiscardAlpha(RgbColor.White);

                                        resultBitmap.Save(file.Path + ".tmp", options);
                                        isConverted = true;
                                    }
                                }
                            }
                        }
                    }

                    if (isConverted)
                    {
                        File.Delete(file.Path);
                        File.Move(file.Path + ".tmp", file.Path);
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.ErrorException(string.Format("Error while converting file {1} to {0}", _convertFormat, file.Path), ex);
                }
            }
            return new TaskOutput(input.Files, input.Params);
        }

        protected override void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput)
        {
        }
    }
}