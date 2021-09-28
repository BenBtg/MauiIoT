using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Maui.Essentials;
using System.Threading.Tasks;

namespace MauiIoT.Services
{
    public class DeviceConfigService : IDeviceConfigService
    {
        private string deviceId;
        private string scopeId;
        private string deviceKey;

        public string DeviceId { get => deviceId; set => deviceId = value; }
        public string ScopeId { get => scopeId; set => scopeId = value; }
        public string ModelId { get => "dtmi:azureiot:PhoneAsADevice;2"; }
        public string DeviceKey { get => deviceKey; set => deviceKey = value; }


        public async Task InitAsync()
        {
            DeviceId = await LoadValue(nameof(DeviceId));
            ScopeId = await LoadValue(nameof(ScopeId));
            DeviceKey = await LoadValue(nameof(DeviceKey));
        }

        public void ResetConfig()
        {
            SecureStorage.RemoveAll();
        }

        public async Task SaveAsync()
        {
            await SaveValue(DeviceId, nameof(DeviceId));
            await SaveValue(ScopeId, nameof(ScopeId));
            await SaveValue(DeviceKey, nameof(DeviceKey));
        }

        private async Task<string> LoadValue(string propertyName)
        {
            return await SecureStorage.GetAsync(propertyName);
        }

        private async Task SaveValue(string value, string propertyName)
        {
            await SecureStorage.SetAsync(propertyName, value);
        }

        public string DpsGlobalEndpoint => "global.azure-devices-provisioning.net";
                
        public string AssignedEndPoint { get; set; }

    }
}
