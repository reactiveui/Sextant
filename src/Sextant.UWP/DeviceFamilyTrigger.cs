// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Sextant.UWP
{
    /// <summary>
    /// Trigger for device type.
    /// </summary>
    public class DeviceFamilyTrigger : StateTriggerBase
    {
        private string _currentDeviceFamily;
        private string _queriedDeviceFamily;

        /// <summary>
        /// Gets or sets the DeviceFamily.
        /// </summary>
        public string DeviceFamily
        {
            get
            {
                return _queriedDeviceFamily;
            }

            set
            {
                _queriedDeviceFamily = value;

                // Get the current device family
                _currentDeviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;

                // The trigger will be activated if the current device family matches the device family value in XAML
                SetActive(_queriedDeviceFamily == _currentDeviceFamily);
            }
        }
    }
}
