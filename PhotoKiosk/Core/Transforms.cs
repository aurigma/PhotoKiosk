// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Aurigma.PhotoKiosk
{
    public class Transform
    {
        public Transform()
        {
        }

        public virtual void Apply(Aurigma.GraphicsMill.Bitmap bitmap)
        {
        }

        public virtual event Aurigma.GraphicsMill.ProgressEventHandler Progress;

        public virtual int TransformsCount
        {
            get { return 0; }
        }

        public virtual int TransformProgressStepCount
        {
            get { return 0; }
        }
    }

    public class RotateAndFlipTransform : Transform
    {
        public RotateAndFlipTransform(RotateFlipType rotateFlipType)
        {
            _rotateFlipType = rotateFlipType;
        }

        public override void Apply(Aurigma.GraphicsMill.Bitmap bitmap)
        {
            base.Apply(bitmap);
            bitmap.Progress += new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
            bitmap.Transforms.RotateAndFlip(_rotateFlipType);
            bitmap.Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
        }

        public override event Aurigma.GraphicsMill.ProgressEventHandler Progress;

        public RotateFlipType RotateFlipType
        {
            get { return _rotateFlipType; }
            set { _rotateFlipType = value; }
        }

        public override int TransformsCount
        {
            get { return 1; }
        }

        public override int TransformProgressStepCount
        {
            get { return 200; }
        }

        private void ProgressEventHandler(object sender, Aurigma.GraphicsMill.ProgressEventArgs e)
        {
            if (Progress != null)
                Progress(sender, e);
        }

        private RotateFlipType _rotateFlipType;
    }

    public class ColorCorrectionTransform : Transform
    {
        public ColorCorrectionTransform(bool autoLevels, float brightnessAmount, float contrastAmount)
        {
            _autoLevels = autoLevels;
            _brightnessAmount = brightnessAmount;
            _contrastAmount = contrastAmount;
            _transformsCount = 0;

            if (_autoLevels)
                _transformsCount++;
            if (_brightnessAmount != 0 || _contrastAmount != 0)
                _transformsCount++;
        }

        public override void Apply(Aurigma.GraphicsMill.Bitmap bitmap)
        {
            base.Apply(bitmap);

            bitmap.Progress += new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);

            if (_autoLevels)
                bitmap.ColorAdjustment.AutoLevels();

            if (_brightnessAmount != 0 || _contrastAmount != 0)
                bitmap.ColorAdjustment.BrightnessContrast(_brightnessAmount, _contrastAmount);

            bitmap.Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
        }

        public override event Aurigma.GraphicsMill.ProgressEventHandler Progress;

        public override int TransformsCount
        {
            get { return _transformsCount; }
        }

        public override int TransformProgressStepCount
        {
            get { return _transformsCount * 200; }
        }

        public bool AutoLevels
        {
            get { return _autoLevels; }
            set { _autoLevels = value; }
        }

        public float BrightnessAmount
        {
            get { return _brightnessAmount; }
            set { _brightnessAmount = value; }
        }

        public float ContrastAmount
        {
            get { return _contrastAmount; }
            set { _contrastAmount = value; }
        }

        private void ProgressEventHandler(object sender, Aurigma.GraphicsMill.ProgressEventArgs e)
        {
            if (Progress != null)
                Progress(sender, e);
        }

        private bool _autoLevels;
        private float _brightnessAmount;
        private float _contrastAmount;
        private int _transformsCount;
    }

    public class CropTransform : Transform
    {
        public CropTransform(float x, float y, float width, float height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public override void Apply(Aurigma.GraphicsMill.Bitmap bitmap)
        {
            base.Apply(bitmap);

            bitmap.Progress += new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
            bitmap.Transforms.Crop(_x, _y, _width, _height);
            bitmap.Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
        }

        public override event Aurigma.GraphicsMill.ProgressEventHandler Progress;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public override int TransformsCount
        {
            get { return 1; }
        }

        public override int TransformProgressStepCount
        {
            get { return 200; }
        }

        private void ProgressEventHandler(object sender, Aurigma.GraphicsMill.ProgressEventArgs e)
        {
            if (Progress != null)
                Progress(sender, e);
        }

        private float _x;
        private float _y;
        private float _width;
        private float _height;
    }

    public enum EffectTransforms
    {
        BlackAndWhite,
        Sepia,
        None
    }

    public class EffectTransform : Transform
    {
        public EffectTransform(EffectTransforms effect)
        {
            _effectTransform = effect;
            _transformsCount = 0;

            switch (_effectTransform)
            {
                case EffectTransforms.BlackAndWhite:
                    _transformsCount = 1;
                    break;

                case EffectTransforms.Sepia:
                    _transformsCount = 2;
                    break;

                case EffectTransforms.None:
                    break;
            }
        }

        public override void Apply(Aurigma.GraphicsMill.Bitmap bitmap)
        {
            base.Apply(bitmap);
            bitmap.Progress += new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
            switch (_effectTransform)
            {
                case EffectTransforms.BlackAndWhite:
                    bitmap.ColorAdjustment.Desaturate();
                    break;

                case EffectTransforms.Sepia:
                    bitmap.ColorAdjustment.Desaturate();
                    Single[] objColorBalance = { 0F, 0.05F, 0.1F, 0F };
                    bitmap.ColorAdjustment.ChannelBalance(objColorBalance);
                    break;

                case EffectTransforms.None:
                    break;
            }
            bitmap.Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
        }

        public override event Aurigma.GraphicsMill.ProgressEventHandler Progress;

        public EffectTransforms Effect
        {
            get { return _effectTransform; }
            set { _effectTransform = value; }
        }

        public override int TransformsCount
        {
            get { return _transformsCount; }
        }

        public override int TransformProgressStepCount
        {
            get { return _transformsCount * 200; }
        }

        private void ProgressEventHandler(object sender, Aurigma.GraphicsMill.ProgressEventArgs e)
        {
            if (Progress != null)
                Progress(sender, e);
        }

        private EffectTransforms _effectTransform;
        private int _transformsCount;
    }

    public class RedEyeRemovalTransform : Transform
    {
        public RedEyeRemovalTransform()
        {
            _transforms = new List<Transform>();
        }

        public override void Apply(Aurigma.GraphicsMill.Bitmap bitmap)
        {
            base.Apply(bitmap);
            foreach (Transform transform in _transforms)
            {
                transform.Progress += new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
                transform.Apply(bitmap);
                transform.Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
            }
        }

        public override event Aurigma.GraphicsMill.ProgressEventHandler Progress;

        public override int TransformsCount
        {
            get { return 1; }
        }

        public override int TransformProgressStepCount
        {
            get { return _transforms.Count * 200; }
        }

        public List<Transform> Transforms
        {
            get { return _transforms; }
            set { _transforms = value; }
        }

        private void ProgressEventHandler(object sender, Aurigma.GraphicsMill.ProgressEventArgs e)
        {
            if (Progress != null)
                Progress(sender, e);
        }

        private List<Transform> _transforms;
    }

    public class AutoRedEyeRemoval : Transform
    {
        public AutoRedEyeRemoval(RectangleF redEyeRect)
        {
            _redEyeRemoval = new Aurigma.GraphicsMill.Transforms.RedEyeRemoval();
            _redEyeRemoval.Mode = Aurigma.GraphicsMill.Transforms.RedEyeRemovalMode.Semiautomatic;
            _redEyeRemoval.FaceRegion = redEyeRect;
        }

        public override void Apply(Aurigma.GraphicsMill.Bitmap bitmap)
        {
            base.Apply(bitmap);
            _redEyeRemoval.Progress += new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
            _redEyeRemoval.ApplyTransform(bitmap);
            _redEyeRemoval.Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
        }

        public override event Aurigma.GraphicsMill.ProgressEventHandler Progress;

        private void ProgressEventHandler(object sender, Aurigma.GraphicsMill.ProgressEventArgs e)
        {
            if (Progress != null)
                Progress(sender, e);
        }

        private Aurigma.GraphicsMill.Transforms.RedEyeRemoval _redEyeRemoval;
    }

    public class ManualRedEyeRemoval : Transform
    {
        public ManualRedEyeRemoval(RectangleF redEyeRect, PointF redEyePoint)
        {
            _redEyeRemoval = new Aurigma.GraphicsMill.Transforms.RedEyeRemoval();
            _redEyeRemoval.Mode = Aurigma.GraphicsMill.Transforms.RedEyeRemovalMode.Manual;

            _redEyeRemoval.FaceRegion = redEyeRect;
            _redEyeRemoval.EyePoint = redEyePoint;
        }

        public override void Apply(Aurigma.GraphicsMill.Bitmap bitmap)
        {
            base.Apply(bitmap);
            _redEyeRemoval.Progress += new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
            _redEyeRemoval.ApplyTransform(bitmap);
            _redEyeRemoval.Progress -= new Aurigma.GraphicsMill.ProgressEventHandler(ProgressEventHandler);
        }

        public override event Aurigma.GraphicsMill.ProgressEventHandler Progress;

        private void ProgressEventHandler(object sender, Aurigma.GraphicsMill.ProgressEventArgs e)
        {
            if (Progress != null)
                Progress(sender, e);
        }

        private Aurigma.GraphicsMill.Transforms.RedEyeRemoval _redEyeRemoval;
    }
}