using System;
using System.Text.RegularExpressions;
using Sandland.EditorUI.Core.Editor.Utils;
using Sandland.PackageManager.Editor.Logic;
using UnityEngine.UIElements;

namespace Sandland.PackageManager.UI.Editor.Views
{
    public class NewPackageTabController : IDisposable
    {
        private const string ValidationResultElementId = "package-validation-result";
        private const string DisplayNameElementId = "package-display-name";
        private const string BundleIdElementId = "package-bundle-id";
        private const string RootNamespaceElementId = "package-root-namespace";
        private const string DescriptionElementId = "package-description";
        private const string CreateButtonElementId = "package-create-button";

        private readonly Label _validationResultText;
        private readonly TextField _displayNameText;
        private readonly TextField _bundleIdText;
        private readonly TextField _rootNamespaceText;
        private readonly TextField _descriptionText;
        private readonly Button _createButton;

        private readonly IPackagesService _packagesService;

        public NewPackageTabController(VisualElement tabRoot, IPackagesService packagesService)
        {
            if (packagesService == null)
            {
                throw new Exception($"Missing implementation for {nameof(IPackagesService)}");
            }

            _packagesService = packagesService;

            if (tabRoot == null)
            {
                throw new Exception("Tab Root not found");
            }

            _validationResultText = tabRoot.Q<Label>(ValidationResultElementId) ??
                                    throw new Exception($"Can't find {ValidationResultElementId}");
            _displayNameText = tabRoot.Q<TextField>(DisplayNameElementId) ??
                               throw new Exception($"Can't find {DisplayNameElementId}");
            _bundleIdText = tabRoot.Q<TextField>(BundleIdElementId) ??
                            throw new Exception($"Can't find {BundleIdElementId}");
            _rootNamespaceText = tabRoot.Q<TextField>(RootNamespaceElementId) ??
                                 throw new Exception($"Can't find {RootNamespaceElementId}");
            _descriptionText = tabRoot.Q<TextField>(DescriptionElementId) ??
                               throw new Exception($"Can't find {DescriptionElementId}");
            _createButton = tabRoot.Q<Button>(CreateButtonElementId) ??
                            throw new Exception($"Can't find {CreateButtonElementId}");

            _createButton.clicked += CreatePackage;
        }

        private void CreatePackage()
        {
            SetEnabledState(false);

            try
            {
                CleanErrors();
                ValidateInput();
                _packagesService.CreateNewPackage(_bundleIdText.text, _displayNameText.text,
                    _descriptionText.text ?? string.Empty, _rootNamespaceText.text);
            }
            finally
            {
                SetEnabledState(true);
            }
        }

        private void CleanErrors()
        {
            _validationResultText.AddToClassList(UssClasses.Invisible);
            _displayNameText.RemoveFromClassList(UssClasses.Error);
            _bundleIdText.RemoveFromClassList(UssClasses.Error);
            _rootNamespaceText.RemoveFromClassList(UssClasses.Error);
            _descriptionText.RemoveFromClassList(UssClasses.Error);
        }

        private void SetEnabledState(bool isEnabled)
        {
            _displayNameText.SetEnabled(isEnabled);
            _bundleIdText.SetEnabled(isEnabled);
            _rootNamespaceText.SetEnabled(isEnabled);
            _descriptionText.SetEnabled(isEnabled);
            _createButton.SetEnabled(isEnabled);
        }

        private void ValidateInput()
        {
            try
            {
                ValidateDisplayName();
                ValidateBundleId();
                ValidateNamespace();
            }
            catch (DisplayNameException ex)
            {
                DisplayError(_displayNameText, ex.Message);
                throw;
            }
            catch (RootNamespaceException ex)
            {
                DisplayError(_rootNamespaceText, ex.Message);
                throw;
            }
            catch (BundleIdException ex)
            {
                DisplayError(_bundleIdText, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                DisplayError(null, ex.Message);
                throw;
            }
        }

        private void DisplayError(VisualElement elementWithError, string message)
        {
            elementWithError?.AddToClassList(UssClasses.Error);
            _validationResultText.text = message;
            _validationResultText.RemoveFromClassList(UssClasses.Invisible);
        }

        private void ValidateDisplayName()
        {
            if (string.IsNullOrEmpty(_displayNameText.text))
            {
                throw new DisplayNameException("Display Name is empty");
            }
        }

        private void ValidateBundleId()
        {
            if (string.IsNullOrEmpty(_bundleIdText.text))
            {
                throw new BundleIdException("Bundle ID is empty");
            }

            var pattern = @"^([a-z]{2,}\.)+[a-z]{2,}$";
            var isValid = Regex.IsMatch(_bundleIdText.text, pattern, RegexOptions.IgnoreCase);

            if (!isValid)
            {
                throw new BundleIdException("Bundle ID must be in reverse domain format");
            }
        }

        private void ValidateNamespace()
        {
            if (string.IsNullOrEmpty(_rootNamespaceText.text))
            {
                throw new RootNamespaceException("Root Namespace is empty");
            }

            var pattern = @"^([a-zA-Z_][a-zA-Z0-9_]*)+(\.[a-zA-Z_][a-zA-Z0-9_]*)*$";
            var isValid = Regex.IsMatch(_rootNamespaceText.text, pattern);

            if (!isValid)
            {
                throw new RootNamespaceException("Incorrect namespace format");
            }
        }

        public void Dispose()
        {
            _createButton.clicked -= CreatePackage;
        }

        private class BundleIdException : Exception
        {
            public BundleIdException(string message) : base(message)
            {
            }
        }

        private class RootNamespaceException : Exception
        {
            public RootNamespaceException(string message) : base(message)
            {
            }
        }

        private class DisplayNameException : Exception
        {
            public DisplayNameException(string message) : base(message)
            {
            }
        }
    }
}