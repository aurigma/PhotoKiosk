// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace Aurigma.PhotoKiosk.Core
{
    public class Constants
    {
        private Constants()
        {
        }

        public const string MmUnits = "mm";
        public const string InchUnits = "inch";

        // Common
        public const double Eps = 0.0001;

        // Context item names
        public const string EditPhotoContextName = "PhotoForEdit";

        public const string OrderContextName = "OrderedItems";
        public const string FoundPhotos = "FoundPhotos";
        public const string PhotosToRemove = "PhotosToRemove";

        // Stage names
        public const string WelcomeStageName = "WelcomeStage";

        public const string SelectStageName = "SelectStage";
        public const string ImageEditorStageName = "ImageEditorStage";
        public const string ProcessStageName = "ProcessOrderStage";
        public const string UserInfoStageName = "UserInfoStage";
        public const string FindingPhotosStageName = "FindingPhotosStage";

        // Command names
        public const string SwitchToStage = "SwitchToStageCommand";

        public const string SwitchToScreen = "SwitchToScreenCommand";
        public const string ResetOrderData = "ResetOrderDataCommand";

        // Style names
        public const string SelectItemStyleName = "SelectItemStyle";

        public const string OrderItemStyleName = "EditItemStyle";

        // Resource Keys
        public const string ImagePleaseWaitKey = "PleaseWait";

        public const string ImageMagnifierKey = "MagnifierButton";
        public const string ImageEditKey = "EditButton";
        public const string ImageBigCheckBoxKey = "BigCheckBoxButton";
        public const string ImageRemoveBigButtonKey = "RemoveBigButton";
        public const string ImageAllFormatsCheckBoxKey = "AllFormatsCheckBoxButton";
        public const string ImageGreenCropKey = "GreenCropButton";
        public const string ImageUndoKey = "UndoButton";
        public const string ImageRedoKey = "RedoButton";
        public const string ImageRotateLeftKey = "RotateLeftButton";
        public const string ImageRotateRightKey = "RotateRightButton";
        public const string ImageFlipVerticalKey = "FlipVerticalButton";
        public const string ImageFlipHorizontalKey = "FlipHorizontalButton";
        public const string ImageSaveKey = "SaveButton";
        public const string ImageSaveAsNewKey = "SaveAsNewButton";
        public const string ImageColorCorrectionKey = "ColorCorrectionButton";
        public const string ImageAutoLevelsKey = "AutoLevelsButton";
        public const string ImageEffectsKey = "EffectsButton";
        public const string ImageRedEyeRemovalKey = "RedEyeRemovalButton";
        public const string ImageBlackAndWhiteKey = "BlackAndWhiteButton";
        public const string ImageSepiaKey = "SepiaButton";
        public const string ImageRedEyeNextKey = "RedEyeNextButton";
        public const string ImageRedEyeManualKey = "RedEyeManualButton";
        public const string ImageRedEyeRemoveKey = "RedEyeRemoveButton";
        public const string ImageBluetoothKey = "BluetoothButton";
        public const string ImageStorageKey = "StorageButton";
        public const string ImageBluetoothLogoKey = "BluetoothLogo";
        public const string ImageBannerKey = "ImageBanner";
        public const string ImageArrowPreviousKey = "ArrowPreviousButton";
        public const string ImageArrowNextKey = "ArrowNextButton";
        public const string ImageRemoveSmallButtonKey = "RemoveSmallButton";
        public const string ImageSetCropFrameButtonKey = "SetCropFrameButton";
        public const string ImageCheckButtonKey = "CheckButton";

        // Common
        public const string CancelTextKey = "CancelButton";

        public const string CancelTextDefValue = "Cancel order";

        public const string BackButtonTextKey = "BackButton";
        public const string BackButtonTextDefValue = "Back";

        public const string NextButtonTextKey = "NextButton";
        public const string NextButtonTextDefValue = "Next";

        public const string MessageOkTextKey = "OkMessage";
        public const string MessageOkTextDefValue = "OK";

        public const string MessageCancelTextKey = "CancelMessage";
        public const string MessageCancelTextDefValue = "Cancel";

        public const string MessageYesTextKey = "YesMessage";
        public const string MessageYesTextDefValue = "Yes";

        public const string MessageNoTextKey = "NoMessage";
        public const string MessageNoTextDefValue = "No";

        public const string MessageSureWantToCancelOrderKey = "SureWantToCancelOrderMessage";
        public const string MessageSureWantToCancelOrderDefValue = "Do you really want to discard your order?";

        public const string MessageBeforeResetKey = "BeforeResetMessage";
        public const string MessageBeforeResetDefValue = "Order creation is suspended due to user inactivity. &#xd;&#xa;Please click \"Back to Order\" or your order will be automatically discarded in a few seconds.";

        public const string BackToOrderButtonKey = "BackToOrderButton";
        public const string BackToOrderButtonDefValue = "Back to Order";

        // Welcome Screen
        public const string WelcomeStageButtonNextTextKey = "WelcomeScreenNextButton";

        public const string WelcomeStageButtonNextTextDefValue = "Start";

        public const string WelcomeStageButtonOrderPhotosTextKey = "WelcomeScreenOrderPhotosButton";
        public const string WelcomeStageButtonOrderPhotosTextDefValue = "Order photos";

        public const string WelcomeStageButtonPrintPhotosTextKey = "WelcomeScreenPrintPhotosButton";
        public const string WelcomeStageButtonPrintPhotosTextDefValue = "Print photos";

        public const string WelcomeStageButtonBurnPhotosTextKey = "WelcomeScreenBurnPhotosButton";
        public const string WelcomeStageButtonBurnPhotosTextDefValue = "Burn photos to CD/DVD";

        public const string WelcomeStageButtonProcessOrderTextKey = "WelcomeScreenProcessOrderButton";
        public const string WelcomeStageButtonProcessOrderTextDefValue = "Process order";

        public const string WelcomeTextKey = "WelcomeScreenHeader";
        public const string WelcomeTextDefValue = "Welcome to Photo Kiosk";

        public const string WelcomeNoticeBluetoothDisabledTextKey = "WelcomeScreenBluetoothDisabledHint";
        public const string WelcomeNoticeBluetoothDisabledTextDefValue = "Insert your CD, DVD, Flash drive, or another media and click \"Start\".";

        public const string WelcomeNoticeBluetoothEnabledTextKey = "WelcomeScreenBluetoothEnabledHint";
        public const string WelcomeNoticeBluetoothEnabledTextDefValue = "Click \"Start\" to begin.";

        public const string WelcomeNoticeBluetoothDisabledMultiActionTextKey = "WelcomeScreenBluetoothDisabledMultiActionsHint";
        public const string WelcomeNoticeBluetoothDisabledMultiActionTextDefValue = "Insert your CD, DVD, Flash drive, or another media and choose a service.";

        public const string WelcomeNoticeBluetoothEnabledMultiActionTextKey = "WelcomeScreenBluetoothEnabledMultiActionsHint";
        public const string WelcomeNoticeBluetoothEnabledMultiActionTextDefValue = "Choose a service.";

        // Choose Upload Way Screen
        public const string SelectDeviceTextKey = "ChooseUploadWayScreenHeader";

        public const string SelectDeviceTextDefValue = "Choosing How to Upload Photos";

        public const string SelectDeviceNoticeTextKey = "ChooseUploadWayScreenHint";
        public const string SelectDeviceNoticeTextDefValue = "Choose the appropriate way of transferring the photos for making an order and click \"Next\".";

        public const string MediaStorageTitleTextKey = "MediaStorageTitle";
        public const string MediaStorageTitleTextDefValue = "Portable storage media";

        public const string MediaStorageDescriptionTextKey = "MediaStorageDescription";
        public const string MediaStorageDescriptionTextDefValue = "This will transfer the photos from portable storages such as USB Flash drives, SD, MMC and CF memory cards as well as CD and DVD. Insert your media to continue to the next step.";

        public const string BluetoothTitleTextKey = "BluetoothTitle";
        public const string BluetoothTitleTextDefValue = "Bluetooth";

        public const string BluetoothDescriptionTextKey = "BluetoothDescription";
        public const string BluetoothDescriptionTextDefValue = "This will transfer the photos from mobile phone or from PDA via Bluetooth. Make sure your device supports Bluetooth data transfer and follow the instructions for the next step.";

        // Select Folders Screen
        public const string SelectFoldersTextKey = "SelectFoldersScreenHeader";

        public const string SelectFoldersTextDefValue = "Selecting Folders";

        public const string SelectFoldersNoticeTextKey = "SelectFoldersScreenHint";
        public const string SelectFoldersNoticeTextDefValue = "Select folders to upload photos from and click \"Next\".";

        public const string SelectedFoldersLabelTextKey = "SelectedFoldersLabel";
        public const string SelectedFoldersLabelTextDefValue = "Selected folders: ";

        public const string PhotosLabelTextKey = "PhotosLabel";
        public const string PhotosLabelTextDefValue = "Photos: ";

        public const string RemovableDriveTypeTextKey = "RemovableDiskText";
        public const string RemovableDriveTypeTextDefValue = "Removable Disk";

        public const string CDRomDriveTypeTextKey = "CDDVDDiskText";
        public const string CDRomDriveTypeTextDefValue = "CD/DVD Disk";

        public const string MessageNoFilesFoundKey = "FilesNotFoundMessage";
        public const string MessageNoFilesFoundDefValue = "No files found.";

        // Uploading Photos Screen
        public const string LoadPhotosTextKey = "UploadingPhotosScreenHeader";

        public const string LoadPhotosTextDefValue = "Uploading Photos";

        public const string ReadyToReceivePhotosTextKey = "ReadyToReceivePhotosText";
        public const string ReadyToReceivePhotosTextDefValue = "Ready to receive photos";

        public const string ReceivedPhotosCountTextKey = "ReceivedPhotosCountText";
        public const string ReceivedPhotosCountTextDefValue = "Received photos count: ";

        public const string HowToLoadPhotosUsingBluetoothTextKey = "HowToLoadPhotosUsingBluetoothText";
        public const string HowToLoadPhotosUsingBluetoothTextDefValue = "Photo Kiosk is expecting the data to be transferred. Please upload photos from your portable device to the Bluetooth host named #BluetoothHostName#. The amount of photos to be transferred will be displayed to you.\n\nAfter all necessary photos have been uploaded, click \"Next\" to create the order.";

        // Finding Photos Screen
        public const string FindingPhotosStepTextKey = "FindingPhotosHeader";

        public const string FindingPhotosStepTextDefValue = "Searching photos...";

        public const string FindingPhotosNoticeTextKey = "FindingPhotosHint";
        public const string FindingPhotosNoticeTextDefValue = "Please wait.";

        // ThumbnailList Control
        public const string TabAllTextKey = "TabAllButton";

        public const string TabAllTextDefValue = "All";

        public const string TabCheckedTextKey = "TabCheckedButton";
        public const string TabCheckedTextDefValue = "Checked";

        public const string TabUncheckedTextKey = "TabUncheckedButton";
        public const string TabuncheckedTextDefValue = "Unchecked";

        public const string FindingPhotosPagesTextKey = "Pages";
        public const string FindingPhotosPagesTextDefValue = "Pages";

        public const string PagesInfoKey = "PagesInfo";
        public const string PagesInfoDefValue = "Photos #0#, Pages #1# ";

        public const string ThumbnailCheckTextKey = "ThumbnailCheckButton";
        public const string ThumbnailCheckTextDefValue = "check";

        public const string ThumbnailPreviewTextKey = "ThumbnailPreviewButton";
        public const string ThumbnailPreviewTextDefValue = "preview";

        public const string ThumbnailRemoveTextKey = "ThumbnailRemoveButton";
        public const string ThumbnailRemoveTextDefValue = "don't print";

        public const string ThumbnailPrintsTextKey = "ThumbnailPrintsButton";
        public const string ThumbnailPrintsDefValue = "prints";

        public const string ThumbnailSetFrameTextKey = "ThumbnailSetFrameButton";
        public const string ThumbnailSetFrameTextDefValue = "set\nprint area";

        public const string ThumbnailEditTextKey = "ThumbnailEditButton";
        public const string ThumbnailEditTextDefValue = "edit";

        public const string ThumbnailLowQualityTextKey = "ThumbnailLowQuality";
        public const string ThumbnailLowQualityTextDefValue = "Low quality for {0}";

        public const string MessageLoadingImageErrorKey = "LoadingImageErrorMessage";
        public const string MessageLoadingImageErrorDefValue = "Unable to open photo. It cannot be viewed or edited, but still can be ordered.";

        // Select Photos Screen
        public const string SelectStepTextKey = "SelectStepHeader";

        public const string SelectStepTextDefValue = "Step 1: Select photos you want to print";

        public const string SelectStepCdBurningTextKey = "SelectStepCdBurningHeader";
        public const string SelectStepCdBurningTextDefValue = "Step 1: Select photos you want to burn to CD/DVD";

        public const string SelectStepNoticeTextKey = "SelectStepHint";
        public const string SelectStepNoticeTextDefValue = "Click a photo to add it to the order.";

        public const string SelectStepNoticeCdBurningTextKey = "SelectStepCdBurningHint";
        public const string SelectStepNoticeCdBurningTextDefValue = "Click a photo to add it to the disc.";

        public const string FindingPhotosAllTextKey = "CheckAllButton";
        public const string FindingPhotosAllTextDefValue = "Check all";

        public const string SelectStepPhotosLabelTextKey = "TotalPhotosLabel";
        public const string SelectStepPhotosLabelTextDefValue = "Photos: ";

        public const string SelectStepSizeLabelTextKey = "TotalSizeLabel";
        public const string SelectStepSizeLabelTextDefValue = "Total size: ";

        public const string SelectStepKBLabelTextKey = "KilobytesLabel";
        public const string SelectStepKBLabelTextDefValue = "KB";

        public const string SelectStepMBLabelTextKey = "MegabytesLabel";
        public const string SelectStepMBLabelTextDefValue = "MB";

        public const string SelectStepGBLabelTextKey = "GigabytesLabel";
        public const string SelectStepGBLabelTextDefValue = "GB";

        public const string MessageCheckSomePhotosKey = "CheckSomePhotosMessage";
        public const string MessageCheckSomePhotosDefValue = "There are no photos selected for an order.\nPlease, select at least one photo \nto proceed to the next step.";

        public const string MessageMinimumCostKey = "MinimumCostMessage";
        public const string MessageMinimumCostDefValue = "Your order cost ({0}) is less than minimum allowed cost ({1}).\nPlease order more photos.";

        // Order Forming Screen
        public const string OrderFormingStepTextKey = "OrderFormingHeader";

        public const string OrderFormingStepTextDefValue = "Step 2: Specify order details";

        public const string OrderFomingNoticeTextKey = "OrderFormingHint";
        public const string OrderFomingNoticeTextDefValue = "Click on number of prints to choose print format and amount of copies.";

        public const string AllOrderTextKey = "AllOrderHeader";
        public const string AllOrderTextDefValue = "Your Order";

        public const string OrderInfoControlFormatTextKey = "OrderInfoFormat";
        public const string OrderInfoControlFormatDefValue = "Format";

        public const string OrderInfoControlPriceTextKey = "OrderInfoControlPrice";
        public const string OrderInfoControlPriceTextDefValue = "Price";

        public const string OrderInfoControlCountTextKey = "OrderInfoCount";
        public const string OrderInfoControlCountDefValue = "Quantity";

        public const string OrderInfoControlCostTextKey = "OrderInfoCost";
        public const string OrderInfoControlCostTextDefValue = "Cost";

        public const string OrderInfoControlDiscountCostTextKey = "OrderInfoDiscountCost";
        public const string OrderInfoControlDiscountCostTextDefValue = "Discount cost";

        public const string OrderInfoControlServiceTextKey = "OrderInfoService";
        public const string OrderInfoControlServiceTextDefValue = "Service";

        public const string OrderInfoControlTotalTextKey = "OrderInfoTotal";
        public const string OrderInfoControlTotalTextDefValue = "Total";

        public const string OrderInfoControlOrderTotalTextKey = "OrderInfoOrderTotal";
        public const string OrderInfoControlOrderTotalTextDefValue = "Order total";

        public const string OrderInfoControlSalesTaxTextKey = "OrderInfoSalesTax";
        public const string OrderInfoControlSalesTaxTextDefValue = "Sales tax";

        public const string OrderInfoControlTotalPaymentKey = "OrderInfoTotalPayment";
        public const string OrderInfoControlTotalPaymentDefValue = "Total payment";

        public const string OrderInfoControlCdBurningTextKey = "OrderInfoCdBurning";
        public const string OrderInfoControlCdBurningTextDefValue = "CD/DVD burning";

        public const string SetForAllButtonUpTextKey = "SetFormatUpButton";
        public const string SetForAllButtonUpTextDefValue = "Set Format";

        public const string SetForAllButtonDownTextKey = "SetFormatDownButton";
        public const string SetForAllButtonDownTextDefValue = "for all photos in the order";

        public const string ChoosePaperFormatsTextKey = "ChoosePaperFormatsHint";
        public const string ChoosePaperFormatsTextDefValue = "Please specify quantities for each print format.";

        public const string ChoosePaperFormatsWarningTextKey = "ChoosePaperFormatsWarning";
        public const string ChoosePaperFormatsWarningDefValue = "This operation may overwrite print formats previously specified for photos. Continue?";

        public const string OrderParamsHeaderTextKey = "OrderParamsHeaderText";
        public const string OrderParamsHeaderTextDefValue = "Print options";

        public const string OrderParamsPaperTypeTextKey = "OrderParamsPaperTypeText";
        public const string OrderParamsPaperTypeTextDefValue = "Paper type:";

        public const string OrderParamsCropModeTextKey = "OrderParamsCropModeText";
        public const string OrderParamsCropModeTextDefValue = "Crop mode:";

        public const string ChangeOrderParamsButtonTextKey = "ChangeOrderParamsButton";
        public const string ChangeOrderParamsButtonTextDefValue = "Change print oprions";

        public const string SelectPaperTypeTextKey = "SelectPaperTypeHint";
        public const string SelectPaperTypeTextDefValue = "Choose paper type";

        public const string SelectCropModeTextKey = "SelectCropModeText";
        public const string SelectCropModeTextDefValue = "How to crop images?";

        public const string CropToFillTextKey = "CropToFillText";
        public const string CropToFillTextDefValue = "Crop to Fit into Format";

        public const string CropToFillTextDescriptionKey = "CropToFillTextDescription";
        public const string CropToFillTextDescriptionDefValue = "The photo is stretched to completely fill the paper. The parts that go beyond the paper format are cropped.";

        public const string ResizeToFitTextKey = "ResizeToFitText";
        public const string ResizeToFitTextDefValue = "Preserve Aspect Ratio";

        public const string ResizeToFitTextDescriptionKey = "ResizeToFitTextDescription";
        public const string ResizeToFitTextDescriptionDefValue = "The photos will be fit into the paper format. If the aspect ratio of the paper differs from that of the photo, whitespaces are left.";

        // Image Viewer/Editor
        public const string ImageEditorStageViewKey = "ImageEditorViewHeader";

        public const string ImageEditorStageViewDefValue = "View photo";

        public const string ImageEditorStageEditTextKey = "ImageEditorEditHeader";
        public const string ImageEditorStageEditTextDefValue = "View and edit photos";

        public const string ImageEditorStageSetCropTextKey = "ImageEditorCropHeader";
        public const string ImageEditorStageSetCropTextDefValue = "Set print area";

        public const string ImageEditorReturnTextKey = "ImageEditorReturnButton";
        public const string ImageEditorReturnTextDefValue = "Back to photos";

        public const string ImageEditorPreviousPhotoTextKey = "ImageEditorPreviousButton";
        public const string ImageEditorPreviousPhotoTextDefValue = "Previous photo";

        public const string ImageEditorNextPhotoTextKey = "ImageEditorNextButton";
        public const string ImageEditorNextPhotoTextDefValue = "Next photo";

        public const string ImageEditorUndoTextKey = "ImageEditorUndoButton";
        public const string ImageEditorUndoTextDefValue = "Undo";

        public const string ImageEditorRedoTextKey = "ImageEditorRedoButton";
        public const string ImageEditorRedoTextDefValue = "Redo";

        public const string ImageEditorRotateLeftTextKey = "ImageEditorRotateLeftButton";
        public const string ImageEditorRotateLeftTextDefValue = "Rotate left";

        public const string ImageEditorRotateRightTextKey = "ImageEditorRotateRightButton";
        public const string ImageEditorRotateRightTextDefValue = "Rotate right";

        public const string ImageEditorFlipTextKey = "ImageEditorFlipButton";
        public const string ImageEditorFlipTextDefValue = "Flip";

        public const string ImageEditorCropTextKey = "ImageEditorCropButton";
        public const string ImageEditorCropTextDefValue = "Crop";

        public const string ImageEditorCropOptionTextKey = "ImageEditorCropOptionButton";
        public const string ImageEditorCropOptionTextDefValue = "Crop";

        public const string ImageEditorColorCorrectionTextKey = "ImageEditorColorCorrectionButton";
        public const string ImageEditorColorCorrectionTextDefValue = "Color Correction";

        public const string ImageEditorEffectsTextKey = "ImageEditorEffectsButton";
        public const string ImageEditorEffectsTextDefValue = "Effects";

        public const string ImageEditorRedEyeTextKey = "ImageEditorRedEyeButton";
        public const string ImageEditorRedEyeTextDefValue = "Red-eye removal";

        public const string ImageEditorSaveTextKey = "ImageEditorSaveButton";
        public const string ImageEditorSaveTextDefValue = "Save";

        public const string ImageEditorSaveAsNewTextKey = "ImageEditorSaveAsNewButton";
        public const string ImageEditorSaveAsNewTextDefValue = "Save as new";

        public const string ImageEditorApplyTextKey = "ImageEditorApplyButton";
        public const string ImageEditorApplyTextDefValue = "Apply";

        public const string ImageEditorCancelTextKey = "ImageEditorCancelButton";
        public const string ImageEditorCancelDefValue = "Cancel";

        public const string ImageEditorInvertTextKey = "ImageEditorInvertButton";
        public const string ImageEditorInvertTextDefValue = "Rotate crop frame";

        public const string ImageEditorBrightnessTextKey = "ImageEditorBrightnessText";
        public const string ImageEditorBrightnessTextDefValue = "Brightness";

        public const string ImageEditorContrastTextKey = "ImageEditorContrastText";
        public const string ImageEditorContrastTextDefValue = "Contrast";

        public const string ImageEditorAutoLevelsTextKey = "ImageEditorAutoLevelsButton";
        public const string ImageEditorAutoLevelsTextDefValue = "Auto Correction";

        public const string ImageEditorBlackAndWhiteTextKey = "ImageEditorBlackAndWhiteButton";
        public const string ImageEditorBlackAndWhiteDefValue = "Black and white";

        public const string ImageEditorSepiaTextKey = "ImageEditorSepiaButton";
        public const string ImageEditorSepiaTextDefValue = "Sepia";

        public const string ImageEditorRedEyeStep1TextKey = "ImageEditorRedEyeStep1";
        public const string ImageEditorRedEyeStep1TextDefValue = "Select red-eyed face. Be as precise as possible.\n\nClick \"Next\" button when you selected the face.";

        public const string ImageEditorRedEyeStep2TextKey = "ImageEditorRedEyeStep2";
        public const string ImageEditorRedEyeStep2TextDefValue = "Are you satisfied?\n\nIf yes click \"Apply\" button to save this result.\n\nIf you want to correct red-eyed face manually click \"Manual\" button.";

        public const string ImageEditorRedEyeStep3TextKey = "ImageEditorRedEyeStep3";
        public const string ImageEditorRedEyeStep3TextDefValue = "Click red eyes. If results not ok, use \"Undo\" button to cancel last change.\n\nWhen you satisfied click the \"Apply\" button.";

        public const string ImageEditorRedEyeNextTextKey = "ImageEditorRedEyeNextButton";
        public const string ImageEditorRedEyeNextTextDefValue = "Next";

        public const string ImageEditorRedEyeManualTextKey = "ImageEditorRedEyeManualButton";
        public const string ImageEditorRedEyeManualTextDefValue = "Manual";

        public const string ImageEditorRedEyeRemoveTextKey = "ImageEditorRedEyeRemoveButton";
        public const string ImageEditorRedEyeRemoveTextDefValue = "Remove";

        public const string MessageWantToExitTextKey = "ImageEditorExitConfirmMessage";
        public const string MessageWantToExitDefValue = "Do you really want to return \nto a list of photos without saving changes?";

        public const string MessageEditNextTextKey = "ImageEditorEditNextPhotoMessage";
        public const string MessageEditNextTextDefValue = "Do you really want to edit next image \nwithout saving current changes?";

        public const string MessageEditPreviousTextKey = "ImageEditorEditPreviousPhotoMessage";
        public const string MessageEditPreviousTextDefValue = "Do you really want to edit previous image \nwithout saving current changes?";

        public const string MessageMinCropSizeKey = "MinCropSizeMessage";
        public const string MessageMinCropSizeDefValue = "Crop cannot be performed because selected area is too small.";

        public const string MessageTransformImageKey = "TransformImageMessage";
        public const string MessageTransformImageDefValue = "Please wait.";

        // Additional Services Screen
        public const string AdditionalServicesTextKey = "AdditionalServicesHeader";

        public const string AdditionalServicesTextDefValue = "Choose additional services";

        public const string AdditionalServicePriceTextKey = "AdditionalServicePrice";
        public const string AdditionalServicePriceTextDefValue = "{0}\nPrice: {1}";

        // Confirm Order Screen
        public const string OrderConfirmTextKey = "OrderConfirmHeader";

        public const string OrderConfirmTextDefValue = "Step 3: Review your order";

        public const string OrderConfirmCdBurningTextKey = "OrderConfirmCdBurningHeader";
        public const string OrderConfirmCdBurningTexDefValue = "Step 2: Review your order";

        public const string OrderConfirmNoticeTextKey = "OrderConfirmHint";
        public const string OrderConfirmNoticeTextDefValue = "Review your order details. If required, return to previous steps and change your order.";

        public const string OrderConfirmNoticeCdBurningTextKey = "OrderConfirmCdBurningHint";
        public const string OrderConfirmNoticeCdBurningTextDefValue = "Review your order details. If required, return to previous step and change your order.";

        // Contact Info Screen
        public const string ContactInfoTextKey = "ContactInfoHeader";

        public const string ContactInfoTextDefValue = "Step 4: Enter your contact information";

        public const string ContactInfoCdBurningTextKey = "ContactInfoCdBurningHeader";
        public const string ContactInfoCdBurningTextDefValue = "Step 3: Enter your contact information";

        public const string ContactInfoNoticeTextKey = "ContactInfoHint";
        public const string ContactInfoNoticeTextDefValue = "Fill in your contact information.";

        public const string YourNameTextKey = "YourName";
        public const string YourNameTextDefValue = "Name";

        public const string YourPhoneTextKey = "YourPhone";
        public const string YourPhoneTextDefValue = "Phone";

        public const string YourEmailTextKey = "YourEmail";
        public const string YourEmailTextDefValue = "E-mail";

        public const string KeyboardLayoutKey = "KeyboardLayout";
        public const string KeyboardLayoutDefValue = "1234567890@#EndRow#QWERTYUIOP#Backspace#EndRow#ASDFGHJKL-#EndRow#ZXCVBNM,.#EndRow#EndRow#Shift:!$%#Space";
        public const string KeyboardShiftedLayoutKey = "KeyboardShiftedLayout";
        public const string KeyboardShiftedLayoutDefValue = "!@#Hash#$%&*_+=/#EndRow#QWERTYUIOP#Backspace#EndRow#ASDFGHJKL-#EndRow#ZXCVBNM,.#EndRow#EndRow#Shift:123#Space";

        public const string MessageEnterYourNameKey = "EnterYourNameMessage";
        public const string MessageEnterYourNameDefValue = "Please enter your name.";

        public const string MessageEnterYourPhoneKey = "EnterYourPhoneMessage";
        public const string MessageEnterYourPhoneDefValue = "Please enter your phone number.";

        public const string MessageEnterYourEmailKey = "EnterYourEmailMessage";
        public const string MessageEnterYourEmailDefValue = "Please enter valid e-mail address.";

        public const string MessageProgressOrderKey = "ProgressOrderMessage";
        public const string MessageProgressOrderDefValue = "Please wait while your order is processed.";

        public const string MessagePreparingImageKey = "PreparingImageMessage";
        public const string MessagePreparingImageDefValue = "Preparing files to burn to CD/DVD.";

        // Order Id Screen
        public const string OrderIdTextKey = "OrderIdHeader";

        public const string OrderIdTextDefValue = "Step 5: Enter your order ID";

        public const string OrderIdCdBurningTextKey = "OrderIdCdBurningHeader";
        public const string OrderIdCdBurningTextDefValue = "Step 4: Enter your order ID";

        public const string OrderIdProcessOrderTextKey = "OrderIdProcessOrderHeader";
        public const string OrderIdProcessOrderTextDefValue = "Enter your order ID and activation code";

        public const string OrderIdNoticeTextKey = "OrderIdHint";
        public const string OrderIdNoticeTextDefValue = "Fill in your order identifier.";

        public const string OrderIdProcessOrderNoticeTextKey = "OrderIdProcessOrderHint";
        public const string OrderIdProcessOrderNoticeTextDefValue = "Fill in your order identifier and activation code.";

        public const string YourOrderIdTextKey = "YourOrderId";
        public const string YourOrderIdTextDefValue = "Order ID";

        public const string YourActivationCodeTextKey = "YourActivationCode";
        public const string YourActivationCodeTextDefValue = "Payment confirmation code";

        public const string MessageEnterYourOrderIdKey = "EnterYourOrderIdMessage";
        public const string MessageEnterYourOrderIdDefValue = "Please enter your order identifier.";

        public const string MessageEnterYourActivationCodeKey = "EnterYourActivationCodeMessage";
        public const string MessageEnterYourActivationCodeDefValue = "Please enter your payment confirmation code.";

        public const string MessageActivationCodeIncorrectKey = "ActivationCodeIsIncorrectMessage";
        public const string MessageActivationCodeIncorrectDefValue = "Order identifier or payment confirmation code is incorrect.";

        // Burning Screen
        public const string BurningScreenTextKey = "BurningScreenHeader";

        public const string BurningScreenTextDefValue = "Burning photos to CD/DVD";

        public const string BurningScreenBurnButtonKey = "BurningScreenBurnButton";
        public const string BurningScreenBurnButtonDefValue = "Burn";

        public const string BurningScreenBeforeTextKey = "BurningScreenText";
        public const string BurningScreenBeforeTextDefValue = "Insert your CD/DVD and click \"Burn\".";

        public const string BurningScreenInProcessTextKey = "BurningScreenInProcessText";
        public const string BurningScreenInProcessTextDefValue = "Please wait while photos are burned to the disc. Do not eject the media!";

        public const string BurningScreenMessageTryAgainTextKey = "BurningScreenTryAgainText";
        public const string BurningScreenMessageTryAgainTextDefValue = "Try again";

        public const string BurningScreenMessageCancelTextKey = "BurningScreenCancelText";
        public const string BurningScreenMessageCancelTextDefValue = "Cancel order";

        // Printing Screen
        public const string PrintingScreenTextKey = "PrintingScreenHeader";

        public const string PrintingScreenTextDefValue = "Printing Photos...";

        public const string PrintingScreenNoticeKey = "PrintingScreenHint";
        public const string PrintingScreenNoticeDefValue = "Please wait while your photos are being printed.";

        // ThankYou Screen
        public const string ThankYouScreenTextKey = "ThankYouScreenHeader";

        public const string ThankYouScreenTextDefValue = "Thank you for your choice";

        public const string ThankYouScreenNoticeTextKey = "ThankYouScreenHint";
        public const string ThankYouScreenNoticeTextDefValue = "Do not forget to take your storage media off!";

        public const string ThankYouScreenOrderIdKey = "ThankYouScreenOrderId";
        public const string ThankYouScreenOrderIdDefValue = "Your order ID is #0#";

        public const string ThanksButtonTextKey = "ThanksButton";
        public const string ThanksButtonTextDefValue = "Quit";

        // Resources.xml elements
        public const string ResourceTextTableName = "textResource";

        public const string ResourceKeyName = "key";
        public const string ResourceValueName = "value";

        // Instant keyword
        public const string InstantKey = "Instant";

        // Crop modes
        public const string CropToFillModeName = "cropToFillFormat";

        public const string ResizeToFitModeName = "preserveAspectRatio";

        // OrderInfo
        public const string OrderInfoXmlFileName = "OrderInfo.xml";

        // Constants and limitations
        public const char SpecialKeyFramer = '#';

        public const int WelcomeStageInactiveTime = 0;
        public const int BeforeResetMessageTime = 20000;
        public const string BluetoothHostNamePlaceHolder = "#BluetoothHostName#";

        // Finding photos stage
        public const int FindingPhotosTimerInterval = 1000;

        public const int FindingPhotosMoveImageDuration = 6000;
        public static readonly string[] SupportableExtensions = { ".jpg", ".jpeg", ".tif", ".tiff", ".bmp", ".png", ".psd" };

        // ThumnailList
        public const int MaxPhotoFilenameLength = 16;

        public const double DefaultListBoxItemWidth = 50;
        public const int SelectScreenItemsInRow = 4;
        public const int SelectScreenItemsInColumn = 2;
        public const int OrderFormingScreenItemsInRow = 3;
        public const int OrderFormingScreenItemsInColumn = 2;

        // Scroll width
        public const int ScrollControlWidth = 30;

        public const float CacheClearingRatio = 0.5f;
        public const int InactionTimerInterval = 2000;

        // ImageEditor result key
        public const string ImageEditorResultKey = "ImageEditorResult";

        public const string EditedPhotoFileNameFormat = "EditedPhoto_{0}.jpg";
        public const string ImageEditorModeKey = "ImageEditorModeKey";

        public const float MinCropWidth = 10;
        public const float MinCropHeight = 10;
        public const int MaxCropsCount = 12;

        // Keyboard
        public const int MaxUserNameLength = 27;

        public const int MaxPhoneLength = 27;
        public const int MaxEmailLength = 27;
        public const int MaxOrderIdLength = 37;
        public const int MaxActivationCodeLength = 8;

        // Process order
        public const int MaxOrderItemForTrialVersion = 5;

        // Photo Printer
        public const int MaxCopiesCount = 20;

        // ProgressDialog timout (in seconds)
        public const int ProgressDialogTimeout = 2;
    }
}