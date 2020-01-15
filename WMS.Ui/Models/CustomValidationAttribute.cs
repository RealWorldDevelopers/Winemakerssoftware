using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace WMS.Ui.Models
{
    public sealed class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            if (value is IList list)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }

    public sealed class EnsureMaximumElementsAttribute : ValidationAttribute
    {
        private readonly int _maxElements;
        public EnsureMaximumElementsAttribute(int maxElements)
        {
            _maxElements = maxElements;
        }

        public override bool IsValid(object value)
        {
            if (value is IList list)
            {
                return list.Count <= _maxElements;
            }
            return true;
        }
    }

    public sealed class EnsureFileExtensionsAttribute : ValidationAttribute
    {
        private readonly List<string> _allowedExtensions;

        /// <summary>
        /// Ensure a list of files all have acceptable file extensions.
        /// </summary>
        /// <param name="allowedExtensions">Pipe Separated List of Acceptable File Extensions as <see cref="string"/></param>
        public EnsureFileExtensionsAttribute(string allowedExtensions)
        {
            if (string.IsNullOrWhiteSpace(allowedExtensions))
                throw new ArgumentNullException(nameof(allowedExtensions));

            _allowedExtensions = allowedExtensions.Split("|").ToList();
        }

        /// <summary>
        /// Test a list of files all have acceptable file extensions.
        /// </summary>
        /// <param name="value">Object to test as <see cref="object"/></param>
        /// <returns>Result of test as <see cref="bool"/></returns>
        public override bool IsValid(object value)
        {
            if (value is IList list)
            {
                foreach (FormFile file in list)
                {
                    var ext = Path.GetExtension(file.FileName);
                    if (!_allowedExtensions.Any(e => e.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                        return false;
                }
            }

            return true;
        }
    }

    public sealed class EnsureFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSizeBytes;

        /// <summary>
        /// Ensure a list of files all are smaller than max size.
        /// </summary>
        /// <param name="maxFileSizeBytes">Max bytes per file as <see cref="long"/></param>
        public EnsureFileSizeAttribute(long maxFileSizeBytes)
        {
            _maxFileSizeBytes = maxFileSizeBytes;
        }

        /// <summary>
        /// Test a list of files all are under max size.
        /// </summary>
        /// <param name="value">Object to test as <see cref="object"/></param>
        /// <returns>Result of test as <see cref="bool"/></returns>
        public override bool IsValid(object value)
        {
            if (value is IList list)
            {
                foreach (FormFile file in list)
                {
                    if (file.Length > _maxFileSizeBytes)
                        return false;
                }
            }

            return true;
        }
    }

}
