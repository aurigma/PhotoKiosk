﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aurigma.PhotoKiosk.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Aurigma.PhotoKiosk.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to burn photos to disc.
        ///The problem description is logged. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessageBurningError {
            get {
                return ResourceManager.GetString("MessageBurningError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to read Photo Kiosk configuration. 
        ///The problem description is logged. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessageConfigReadingError {
            get {
                return ResourceManager.GetString("MessageConfigReadingError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to read crops file. 
        ///The problem description is logged. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessageCropsReadingError {
            get {
                return ResourceManager.GetString("MessageCropsReadingError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The drive reported that it is in the process of becoming ready. Please try the request again later..
        /// </summary>
        internal static string MessageDriveIsNotReady {
            get {
                return ResourceManager.GetString("MessageDriveIsNotReady", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to load image {0}.
        ///{1}.
        /// </summary>
        internal static string MessageImageLoadError {
            get {
                return ResourceManager.GetString("MessageImageLoadError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Insufficient disk space on drive {0}. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessageLowMemory {
            get {
                return ResourceManager.GetString("MessageLowMemory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The drive reported that the media is write protected..
        /// </summary>
        internal static string MessageMediaIsReadOnly {
            get {
                return ResourceManager.GetString("MessageMediaIsReadOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The media is inserted upside down..
        /// </summary>
        internal static string MessageMediaUpsideDown {
            get {
                return ResourceManager.GetString("MessageMediaUpsideDown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no media in the device..
        /// </summary>
        internal static string MessageNoMedia {
            get {
                return ResourceManager.GetString("MessageNoMedia", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Another instance of Photo Kiosk is already running..
        /// </summary>
        internal static string MessageOnlyOneInstanceCanBeRun {
            get {
                return ResourceManager.GetString("MessageOnlyOneInstanceCanBeRun", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to print photos.
        ///The problem description is logged. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessagePhotoPrinterError {
            get {
                return ResourceManager.GetString("MessagePhotoPrinterError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to read price file. 
        ///The problem description is logged. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessagePriceReadingError {
            get {
                return ResourceManager.GetString("MessagePriceReadingError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to print a receipt.
        ///The problem description is logged. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessageReceiptPrinterError {
            get {
                return ResourceManager.GetString("MessageReceiptPrinterError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to read localization file. 
        ///The problem description is logged. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessageResourceReadingError {
            get {
                return ResourceManager.GetString("MessageResourceReadingError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to run &quot;{0}&quot; process.
        ///The problem description is logged. 
        ///If you see this message, please 
        ///contact Photo Kiosk operator..
        /// </summary>
        internal static string MessageRunProcessError {
            get {
                return ResourceManager.GetString("MessageRunProcessError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding this file or directory would result in a result image having a size larger than the current configured limit..
        /// </summary>
        internal static string MessageToManyFiles {
            get {
                return ResourceManager.GetString("MessageToManyFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to create a log file: .
        /// </summary>
        internal static string MessageUnableToCreateLog {
            get {
                return ResourceManager.GetString("MessageUnableToCreateLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to write error info in Windows Application Log..
        /// </summary>
        internal static string MessageUnableToUseWinLog {
            get {
                return ResourceManager.GetString("MessageUnableToUseWinLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The media is not compatible or of unknown physical format..
        /// </summary>
        internal static string MessageUnknownMedia {
            get {
                return ResourceManager.GetString("MessageUnknownMedia", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current media is not supported..
        /// </summary>
        internal static string MessageUnsupportedMedia {
            get {
                return ResourceManager.GetString("MessageUnsupportedMedia", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IMAPI supports none of the multisession type(s) provided on the current media..
        /// </summary>
        internal static string MessageUnsupportedMultisession {
            get {
                return ResourceManager.GetString("MessageUnsupportedMultisession", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This is a trial version. Prints of only 5 photos will be actually made..
        /// </summary>
        internal static string TrialMessage {
            get {
                return ResourceManager.GetString("TrialMessage", resourceCulture);
            }
        }
    }
}