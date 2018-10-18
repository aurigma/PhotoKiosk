// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.GraphicsMill.Codecs;
using ODS.Core;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Bitmap = Aurigma.GraphicsMill.Bitmap;
using Exception = System.Exception;

namespace ODS.Service.RulesEngine.Tasks
{
    public class CropTask : TaskBase
    {
        private readonly string _xKey;
        private readonly string _yKey;
        private readonly string _widthKey;
        private readonly string _heightKey;

        public CropTask(string xKey, string yKey, string widthKey, string heightKey, ITask parent, IEnumerable<ITask> children)
            : base(parent, children)
        {
            _xKey = xKey;
            _yKey = yKey;
            _widthKey = widthKey;
            _heightKey = heightKey;
        }

        protected override string TaskName
        {
            get { return string.Format("CROP"); }
        }

        public override int Priority
        {
            get { return 5; }
        }

        protected override TaskOutput Execute(TaskInput input, ITaskContext context)
        {
            foreach (var file in input.Files)
            {
                Rectangle cropRectangle;
                bool cropped = false;
                if (TryGetCropRectangle(file, out cropRectangle))
                {
                    try
                    {
                        using (var readStream = new FileStream(file.Path, FileMode.Open))
                        {
                            using (var reader = FormatManager.CreateFormatReader(readStream))
                            {
                                using (var frame = reader.LoadFrame(0))
                                {
                                    if ((cropRectangle.X > 0 || cropRectangle.Y > 0) && (cropRectangle.Width < frame.Width || cropRectangle.Height < frame.Height))
                                    {
                                        using (var resultBitmap = new Bitmap(frame.Width, frame.Height, Aurigma.GraphicsMill.PixelFormat.Format32bppArgb))
                                        {
                                            frame.GetBitmap(resultBitmap);
                                            resultBitmap.Transforms.Crop(cropRectangle);

                                            if (reader.MediaFormat == FormatManager.JpegFormat)
                                                resultBitmap.Save(file.Path + ".tmp", new JpegEncoderOptions(95, false));
                                            else
                                                resultBitmap.Save(file.Path + ".tmp", FormatManager.CreateEncoderOptions(reader.MediaFormat));
                                        }
                                        cropped = true;
                                    }
                                }
                            }
                        }

                        if (cropped)
                        {
                            File.Delete(file.Path);
                            File.Move(file.Path + ".tmp", file.Path);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorException(string.Format("Error acquired while cropping file {0}", file.Path), ex);
                    }
                }
            }
            return new TaskOutput(input.Files, input.Params);
        }

        private bool TryGetCropRectangle(IFile file, out Rectangle cropRectangle)
        {
            cropRectangle = new Rectangle();
            try
            {
                if (file.Params.ContainsKey(_xKey) && file.Params.ContainsKey(_yKey) && file.Params.ContainsKey(_widthKey) && file.Params.ContainsKey(_heightKey))
                {
                    int x, y, width, height;
                    int.TryParse(file.Params[_xKey], out x);
                    int.TryParse(file.Params[_yKey], out y);
                    int.TryParse(file.Params[_widthKey], out width);
                    int.TryParse(file.Params[_heightKey], out height);
                    cropRectangle = new Rectangle(x, y, width, height);
                    return width > 0 && height > 0;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput)
        {
        }
    }
}