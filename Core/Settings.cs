namespace Aurigma.PhotoKiosk.Core
{
    public class Settings
    {
        #region Construction
        public Settings()
        {
            _config = new RawSettings();
        }
        #endregion

        #region [Public methods and props]
        public ValidationLog Validate(bool stopIfFailed)
        {
            return _config.Validate(stopIfFailed);
        }

        public string CdBurnerFileName
        {
            get { return System.Environment.CurrentDirectory + "\\" + RawSettings.CdBurnerFileName; }   
        }

        public string PhotoPrinterFileName
        {
            get { return System.Environment.CurrentDirectory + "\\" + RawSettings.PhotoPrinterFileName;}
        }

        public string SourcePath
        {
            get { return _config.SourcePath.Trim(";".ToCharArray()); }
        }

        public string BluetoothPath
        {
            get { return RawSettings.GetFullPath(_config.BluetoothPath); }
        }
   
        public int MaxCacheSize
        {
            get { return _config.MaxCacheSize; }
        }

        public string PriceFile
        {
            get { return RawSettings.GetFullPath(_config.PriceFile); }
        }

        public string CropFile
        {
            get { return RawSettings.GetFullPath(_config.CropFile); }
        }

        public string OrderStorePath
        {
            get { return RawSettings.GetFullPath(_config.OrderStorePath); }
        }        
        
        public string ResourceFile
        {
            get { return RawSettings.GetFullPath(_config.ResourceFile); }
        }

        public bool EventLogEnabled
        {
            get { return _config.EventLogEnabled; }
        }
        
        public int Timeout
        {
            get { return _config.Timeout; }
        }

        public int FindingPhotosDuration
        {
            get { return _config.FindingPhotosDuration; }
        }

		public string BluetoothHostName
		{
			get { return _config.BluetoothHostName; }
		}

        public string PhotoKioskId
        {
            get { return _config.PhotoKioskId; }
        }
		
		public bool ShowFolders
		{
			get { return _config.ShowFolders; }
		}

        public bool ReceiptPrintingEnabled
        {
            get { return _config.ReceiptPrintingEnabled; }
        }

        public string ReceiptPrinterName
        {
            get { return _config.ReceiptPrinterName; }
        }

        public string ReceiptTemplateFile
        {
            get { return RawSettings.GetFullPath(_config.ReceiptTemplateFile); }
        }

        public int ReceiptShowPhotosCount
        {
            get { return _config.ReceiptShowPhotosCount; }
        }

        public string OnStartOrderProcess
        {
            get { return _config.OnStartOrderProcess; }
        }

        public string OnCancelOrderProcess
        {
            get { return _config.OnCancelOrderProcess; }
        }

        public string OnCompleteOrderProcess
        {
            get { return _config.OnCompleteOrderProcess; }
        }

        public bool CdBurnerEnabled
        {
            get { return _config.CdBurnerEnabled; }
        }

        public string CdBurnerDrive
        {
            get { return _config.CdBurnerDrive; }
        }

        public string CdBurnerImagesPath
        {
            get { return RawSettings.GetFullPath(_config.CdBurnerImagesPath); }
        }

        public string CdBurnerFolderPrefix
        {
            get { return _config.CdBurnerFolderPrefix; }
        }

        public string CdBurnerLabel
        {
            get { return _config.CdBurnerLabel; }
        }

        public bool PhotoPrinterEnabled
        {
            get { return _config.PhotoPrinterEnabled; }
        }

        public string PhotoPrinterName
        {
            get { return _config.PhotoPrinterName; }
        }


        public bool ImageEditorEnabled
        {
            get { return _config.ImageEditorEnabled; }
        }

        public bool ImageEditorRotateEnabled
        {
            get { return _config.ImageEditorRotateEnabled; }
        }

        public bool ImageEditorFlipEnabled
        {
            get { return _config.ImageEditorFlipEnabled; }
        }

        public bool ImageEditorCropEnabled
        {
            get { return _config.ImageEditorCropEnabled; }
        }

        public bool ImageEditorColorEnabled
        {
            get { return _config.ImageEditorColorEnabled; }
        }

        public bool ImageEditorEffectsEnabled
        {
            get { return _config.ImageEditorEffectsEnabled; }
        }

        public bool ImageEditorRedEyeEnabled
        {
            get { return _config.ImageEditorRedEyeEnabled; }
        }

        public bool ContactPhoneEnabled
        {
            get { return _config.ContactPhoneEnabled; }
        }

        public bool ContactEmailEnabled
        {
            get { return _config.ContactEmailEnabled; }
        }

        public bool ContactOrderIdEnabled
        {
            get { return _config.ContactOrderIdEnabled; }
        }
        
        public string MainBanner
        {
            get { return RawSettings.GetFullPath(_config.MainBanner); }
        }

        public string ThemeFile
        {
            get { return RawSettings.GetFullPath(_config.ThemeFile); }
        }

        public string WelcomeScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.WelcomeScreenBanner); }
        }

        public string ChooseUploadWayScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.ChooseUploadWayScreenBanner); }
        }

        public string SelectFoldersScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.SelectFoldersScreenBanner); }
        }

        public string UploadingPhotosScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.UploadingPhotosScreenBanner); }
        }

        public string ProgressScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.ProgressScreenBanner); }
        }

        public string SelectScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.SelectScreenBanner); }
        }

        public string OrderFormingScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.OrderFormingScreenBanner); }
        }

        public string ImageEditorScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.ImageEditorScreenBanner); }
        }

        public string ConfirmOrderScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.ConfirmOrderScreenBanner); }
        }

        public string ContactInfoScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.ContactInfoScreenBanner); }
        }

        public string OrderIdScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.OrderIdScreenBanner); }
        }

        public string BurningScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.BurningScreenBanner); }
        }

        public string PrintingScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.PrintingScreenBanner); }
        }

        public string ThankYouScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.ThankYouScreenBanner); }
        }

        public string ThankYouCancelScreenBanner
        {
            get { return RawSettings.GetFullPath(_config.ThankYouCancelScreenBanner); }
        }

        public string WelcomeScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.WelcomeScreenBackground); }
        }

        public string ChooseUploadWayScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.ChooseUploadWayScreenBackground); }
        }

        public string SelectFoldersScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.SelectFoldersScreenBackground); }
        }

        public string UploadingPhotosScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.UploadingPhotosScreenBackground); }
        }

        public string ProgressScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.ProgressScreenBackground); }
        }

        public string SelectScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.SelectScreenBackground); }
        }

        public string OrderFormingScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.OrderFormingScreenBackground); }
        }

        public string ImageEditorScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.ImageEditorScreenBackground); }
        }

        public string ConfirmOrderScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.ConfirmOrderScreenBackground); }
        }

        public string ContactInfoScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.ContactInfoScreenBackground); }
        }

        public string OrderIdScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.OrderIdScreenBackground); }
        }

        public string BurningScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.BurningScreenBackground); }
        }

        public string PrintingScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.PrintingScreenBackground); }
        }

        public string ThankYouScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.ThankYouScreenBackground); }
        }

        public string ThankYouCancelScreenBackground
        {
            get { return RawSettings.GetFullPath(_config.ThankYouCancelScreenBackground); }
        }
        #endregion

        #region Variables
        private RawSettings _config;
        #endregion
    }
}
