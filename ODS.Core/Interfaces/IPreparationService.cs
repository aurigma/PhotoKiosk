// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Core
{
    public interface IPreparationService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="inputFiles"></param>
        /// <param name="documents"></param>
        /// <param name="rule"></param>
        /// <param name="status"></param>
        /// <returns>Path to result zip file or folder</returns>
        string Prepare(IEnumerable<IFile> inputFiles, IEnumerable<IFile> documents, ITransformationRule rule);
    }
}