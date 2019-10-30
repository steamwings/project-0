﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project0.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Project0.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Here are your accounts..
        /// </summary>
        internal static string AccountMenu {
            get {
                return ResourceManager.GetString("AccountMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You already have an account with this name..
        /// </summary>
        internal static string AccountNameUnavailable {
            get {
                return ResourceManager.GetString("AccountNameUnavailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @&quot;^\d{0,14}(.\d{2})?$&quot;.
        /// </summary>
        internal static string AmountRegex {
            get {
                return ResourceManager.GetString("AmountRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to close this account?.
        /// </summary>
        internal static string AreYouSureDeleteAccount {
            get {
                return ResourceManager.GetString("AreYouSureDeleteAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to close all your accounts with us?.
        /// </summary>
        internal static string AreYouSureDeleteCustomer {
            get {
                return ResourceManager.GetString("AreYouSureDeleteCustomer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You cannot close this account because funds are owed..
        /// </summary>
        internal static string CannotCloseFundsOwed {
            get {
                return ResourceManager.GetString("CannotCloseFundsOwed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You cannot close all your accounts because funds are owed..
        /// </summary>
        internal static string CannotCloseOutFundsOwed {
            get {
                return ResourceManager.GetString("CannotCloseOutFundsOwed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You still have ${0} in your account. What would you like to do?.
        /// </summary>
        internal static string CloseRemoveFunds {
            get {
                return ResourceManager.GetString("CloseRemoveFunds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to close this account prematurely? Funds can be transferred out after closing..
        /// </summary>
        internal static string ConfirmClosePremature {
            get {
                return ResourceManager.GetString("ConfirmClosePremature", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WARNING: You will lose access to funds!
        ///Are you sure you want to delete your account?.
        /// </summary>
        internal static string ConfirmCloseWithFunds {
            get {
                return ResourceManager.GetString("ConfirmCloseWithFunds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You will need another checking account to transfer..
        /// </summary>
        internal static string CreateCheckingNeeded {
            get {
                return ResourceManager.GetString("CreateCheckingNeeded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a password: .
        /// </summary>
        internal static string CreatePassword {
            get {
                return ResourceManager.GetString("CreatePassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a username for your new account..
        /// </summary>
        internal static string CreateUsername {
            get {
                return ResourceManager.GetString("CreateUsername", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This is your only account, so closing this account will remove you from our system..
        /// </summary>
        internal static string DeletingFinalAccount {
            get {
                return ResourceManager.GetString("DeletingFinalAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter an amount in dollars:.
        /// </summary>
        internal static string EnterDollarAmount {
            get {
                return ResourceManager.GetString("EnterDollarAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter your password..
        /// </summary>
        internal static string EnterPassword {
            get {
                return ResourceManager.GetString("EnterPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter your username to login..
        /// </summary>
        internal static string EnterUsername {
            get {
                return ResourceManager.GetString("EnterUsername", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid program state!.
        /// </summary>
        internal static string ErrorInvalidProgramFlow {
            get {
                return ResourceManager.GetString("ErrorInvalidProgramFlow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to [login] to an existing account or [register] for a new account?.
        /// </summary>
        internal static string ExistingAccountOrRegister {
            get {
                return ResourceManager.GetString("ExistingAccountOrRegister", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please give this account a name..
        /// </summary>
        internal static string GiveAccountName {
            get {
                return ResourceManager.GetString("GiveAccountName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thank you for banking with Console First. Goodbye..
        /// </summary>
        internal static string Goodbye {
            get {
                return ResourceManager.GetString("Goodbye", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This operation cannot be completed on this account..
        /// </summary>
        internal static string IncompatibleAccount {
            get {
                return ResourceManager.GetString("IncompatibleAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid input. Try again..
        /// </summary>
        internal static string InvalidInputTryAgain {
            get {
                return ResourceManager.GetString("InvalidInputTryAgain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can [view] an account, open a [new] account, [transfer] between accounts, [close] all accounts or [logout]..
        /// </summary>
        internal static string MainMenuOptions {
            get {
                return ResourceManager.GetString("MainMenuOptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to no.
        /// </summary>
        internal static string no {
            get {
                return ResourceManager.GetString("no", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation cancelled..
        /// </summary>
        internal static string OperationCancelled {
            get {
                return ResourceManager.GetString("OperationCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation complete..
        /// </summary>
        internal static string OperationComplete {
            get {
                return ResourceManager.GetString("OperationComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation failed..
        /// </summary>
        internal static string OperationFailed {
            get {
                return ResourceManager.GetString("OperationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password you entered is incorrect..
        /// </summary>
        internal static string PasswordIncorrect {
            get {
                return ResourceManager.GetString("PasswordIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A password must be at least {0} characters long..
        /// </summary>
        internal static string PasswordLength {
            get {
                return ResourceManager.GetString("PasswordLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter the number corresponding to your selection: .
        /// </summary>
        internal static string PleaseEnter {
            get {
                return ResourceManager.GetString("PleaseEnter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Press any key to continue....
        /// </summary>
        internal static string PressAnyKey {
            get {
                return ResourceManager.GetString("PressAnyKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select an account to transfer from..
        /// </summary>
        internal static string SelectAccountTransferFrom {
            get {
                return ResourceManager.GetString("SelectAccountTransferFrom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select an account to transfer into..
        /// </summary>
        internal static string SelectAccountTransferTo {
            get {
                return ResourceManager.GetString("SelectAccountTransferTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select an account to view..
        /// </summary>
        internal static string SelectAccountView {
            get {
                return ResourceManager.GetString("SelectAccountView", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error occurred. This service is temporarily unavailable..
        /// </summary>
        internal static string ServiceUnavailable {
            get {
                return ResourceManager.GetString("ServiceUnavailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Funds in a term deposit account cannot be withdrawn before maturity except under specific conditions specified in your account agreement. Please speak to a store associate for details..
        /// </summary>
        internal static string TermDepositWithdraw {
            get {
                return ResourceManager.GetString("TermDepositWithdraw", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to How much would you like to transfer from {0} to {1}?.
        /// </summary>
        internal static string TransferAmount {
            get {
                return ResourceManager.GetString("TransferAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The amount transferred will not be more than the amount owed. IS that okay?.
        /// </summary>
        internal static string TransferNotMoreThanOwedConfirm {
            get {
                return ResourceManager.GetString("TransferNotMoreThanOwedConfirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A transfer requires two accounts..
        /// </summary>
        internal static string TransferRequiresTwoAccounts {
            get {
                return ResourceManager.GetString("TransferRequiresTwoAccounts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} was transferred successfully from {1} to {2}..
        /// </summary>
        internal static string TransferSuccessful {
            get {
                return ResourceManager.GetString("TransferSuccessful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Funds could not be transferred from the account provided..
        /// </summary>
        internal static string TransferWithdrawalFailed {
            get {
                return ResourceManager.GetString("TransferWithdrawalFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to That username is taken. Please enter another username..
        /// </summary>
        internal static string UsernameUnavailable {
            get {
                return ResourceManager.GetString("UsernameUnavailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No account matches that username..
        /// </summary>
        internal static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter valid dollar amount..
        /// </summary>
        internal static string ValidAmount {
            get {
                return ResourceManager.GetString("ValidAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome to Console First Bank - banking the way it used to be..
        /// </summary>
        internal static string Welcome {
            get {
                return ResourceManager.GetString("Welcome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What type of account would you like to open?.
        /// </summary>
        internal static string WhatAccountType {
            get {
                return ResourceManager.GetString("WhatAccountType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What is the size of the certificate deposit term account requested?.
        /// </summary>
        internal static string WhatSizeCD {
            get {
                return ResourceManager.GetString("WhatSizeCD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What is the size of the loan you would like to take out? All loans subject to approval..
        /// </summary>
        internal static string WhatSizeLoan {
            get {
                return ResourceManager.GetString("WhatSizeLoan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Immature funds cannot be withdrawn. Funds can be withdrawn if the account is closed prematurely..
        /// </summary>
        internal static string WithdrawalImmatureFunds {
            get {
                return ResourceManager.GetString("WithdrawalImmatureFunds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Insufficient funds for requested withdrawal..
        /// </summary>
        internal static string WithdrawalInsufficientFunds {
            get {
                return ResourceManager.GetString("WithdrawalInsufficientFunds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Withdrawal succeeded. Interest will be charged on the resulting negative balance..
        /// </summary>
        internal static string WithdrawalSuccessBorrow {
            get {
                return ResourceManager.GetString("WithdrawalSuccessBorrow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Requested funds will be deducted from account balance..
        /// </summary>
        internal static string WithdrawalSuccessNoBorrow {
            get {
                return ResourceManager.GetString("WithdrawalSuccessNoBorrow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to yes.
        /// </summary>
        internal static string yes {
            get {
                return ResourceManager.GetString("yes", resourceCulture);
            }
        }
    }
}
