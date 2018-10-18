// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Text.RegularExpressions;

namespace Aurigma.PhotoKiosk
{
    internal static class EmailValidator
    {
        private const string EmailExpression = @"^[\s]*([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))[\s]*$";

        public static bool IsValid(string email)
        {
            var regex = new Regex(EmailExpression);

            var isValid = regex.IsMatch(email);

            return isValid;
        }
    }
}