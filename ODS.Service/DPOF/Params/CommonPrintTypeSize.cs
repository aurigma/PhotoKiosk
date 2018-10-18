// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    public enum CommonPrintTypeSize
    {
        PrinterDependent = 0,
        From12To38 = 1,
        From38To64 = 2,
        From64To89 = 3,
        From89To114 = 4,
        From114To140 = 5,
        From140To165 = 6,
        From190To216 = 8,
        From241To267 = 10,
        From297To318 = 12,

        IsoA6 = From89To114,
        IsoA5 = From140To165,
        IsoA4 = From190To216,
        IsoA3 = From297To318,

        Us3R = From64To89,
        Us4R = From89To114,
        Us5R = From114To140,
        Us6R = From140To165,
        Us8R = From190To216
    }
}