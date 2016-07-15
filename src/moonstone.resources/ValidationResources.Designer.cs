﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace moonstone.resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ValidationResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidationResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("moonstone.resources.ValidationResources", typeof(ValidationResources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bank account &apos;{0}&apos; has been created successfully..
        /// </summary>
        public static string BankAccount_Create_Success {
            get {
                return ResourceManager.GetString("BankAccount_Create_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to retreive bank accounts for gurrent group..
        /// </summary>
        public static string BankAccount_GetAllForCurrentGroup_Error {
            get {
                return ResourceManager.GetString("BankAccount_GetAllForCurrentGroup_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Category &apos;{0}&apos; has been created..
        /// </summary>
        public static string Category_Create_Success {
            get {
                return ResourceManager.GetString("Category_Create_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to retreive categories for current group..
        /// </summary>
        public static string Category_GetAllForCurrentGroup_Error {
            get {
                return ResourceManager.GetString("Category_GetAllForCurrentGroup_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error has occured. Please try again..
        /// </summary>
        public static string Generic_Error {
            get {
                return ResourceManager.GetString("Generic_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please ensure you have entered all required data..
        /// </summary>
        public static string Generic_ModelState_Error {
            get {
                return ResourceManager.GetString("Generic_ModelState_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Group &apos;{0}&apos; has been created successfully..
        /// </summary>
        public static string Group_Create_Success {
            get {
                return ResourceManager.GetString("Group_Create_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transaction &apos;{0}&apos; has been created successfully..
        /// </summary>
        public static string Transaction_Create_Success {
            get {
                return ResourceManager.GetString("Transaction_Create_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to retreive profile information..
        /// </summary>
        public static string User_GetProfileInformation_Error {
            get {
                return ResourceManager.GetString("User_GetProfileInformation_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login failed. Please try again..
        /// </summary>
        public static string User_Login_Failed {
            get {
                return ResourceManager.GetString("User_Login_Failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your account has been locked..
        /// </summary>
        public static string User_Login_LockedOut {
            get {
                return ResourceManager.GetString("User_Login_LockedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your account requires verification. Please check your inbox..
        /// </summary>
        public static string User_Login_RequiresVerification {
            get {
                return ResourceManager.GetString("User_Login_RequiresVerification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully logged in as {0}..
        /// </summary>
        public static string User_Login_Success {
            get {
                return ResourceManager.GetString("User_Login_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid group..
        /// </summary>
        public static string User_SelectGroup_NotInGroup {
            get {
                return ResourceManager.GetString("User_SelectGroup_NotInGroup", resourceCulture);
            }
        }
    }
}
