﻿namespace dis.cdcavell.dev.Models
{
    /// <summary>
    /// Redirect link information.
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 09/24/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class RedirectViewModel
    {
        /// <value>string</value>
        public string RedirectUrl { get; set; } = string.Empty;
    }
}